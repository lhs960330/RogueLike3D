using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerAnimator))]
public class PlayerController : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerAnimator animator;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashSpeed = 10f;

    private Vector2 moveInput;
    private Camera mainCamera;
    private Vector3 worldMoveDir;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        animator = GetComponent<PlayerAnimator>();
        mainCamera = Camera.main;
    }

    private void OnMove( InputValue value )
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnDash( InputValue value )
    {
        if (worldMoveDir.sqrMagnitude < 0.01f) return;
        
        if (!movement.isDashing)
        {
            animator.DashAnimation();
            movement.Dash(worldMoveDir, dashSpeed);
        }
    }

    private void Update()
    {
        LookAtMouse();

        // [수정] 대시 중일 때와 아닐 때의 로직을 분리합니다.
        if (movement.isDashing)
        {
            // 대시 중일 때는 PlayerMovement에 저장된 대시 방향과 대시 속도를 애니메이션에 전달합니다.
            // 이렇게 해야 키보드에서 손을 떼도 애니메이션 방향이 유지됩니다.
            animator.UpdateAnimation(movement.DashDirection, dashSpeed, moveInput);
        }
        else
        {
            // 평상시에는 카메라 기준의 이동 방향과 현재 속도를 전달합니다.
            Vector3 camForward = mainCamera.transform.forward;
            camForward.y = 0;
            camForward.Normalize();

            Vector3 camRight = mainCamera.transform.right;
            camRight.y = 0;
            camRight.Normalize();

            worldMoveDir = (camForward * moveInput.y + camRight * moveInput.x).normalized;

            movement.SetMoveDirection(worldMoveDir);
            animator.UpdateAnimation(worldMoveDir, movement.CurrentSpeed, moveInput);
        }
    }

    private void FixedUpdate()
    {
        movement.Move(moveSpeed);
    }

    private void LookAtMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, transform.position);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 worldPoint = ray.GetPoint(distance);
            Vector3 lookDirection = worldPoint - transform.position;
            lookDirection.y = 0f;

            if (lookDirection.sqrMagnitude > 0.01f)
            {
                movement.Rotate(lookDirection.normalized);
            }
        }
    }
}
