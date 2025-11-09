using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerAnimator), typeof(PlayerAttack))]
public class PlayerController : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerAnimator playerAnimator;
    private PlayerAttack playerAttack;

    private Vector2 moveInput;
    private Camera mainCamera;

    [Header("State Checks")]
    public bool IsGrounded;

    private void OnEnable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnMoveEvent += OnMove;
            InputManager.Instance.OnAttackEvent += OnAttack;
            InputManager.Instance.OnDashEvent += OnDash;
            InputManager.Instance.OnJumpEvent += OnJump;
        }
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnMoveEvent -= OnMove;
            InputManager.Instance.OnAttackEvent -= OnAttack;
            InputManager.Instance.OnDashEvent -= OnDash;
            InputManager.Instance.OnJumpEvent -= OnJump;
        }
    }

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        playerAnimator = GetComponent<PlayerAnimator>();
        playerAttack = GetComponent<PlayerAttack>();
        mainCamera = Camera.main;
}
            

    private void Update()
    {
        // Manager.EXInput이 유효하다고 가정합니다.
        if (Manager.EXInput.CurrentMap == Define.ActionMap.Player)
            LookAtMouse();

        // 카메라 방향을 기준으로 월드 이동 방향 계산
        Vector3 camForward = mainCamera.transform.forward;
        camForward.y = 0;
        Vector3 camRight = mainCamera.transform.right;
        camRight.y = 0;
        Vector3 worldMoveDir = (camForward * moveInput.y + camRight * moveInput.x).normalized;

        // 이동 방향과 애니메이션 업데이트
        movement.SetMoveDirection(worldMoveDir);
        playerAnimator.UpdateAnimation(worldMoveDir);
    }

    private void FixedUpdate()
    {
        // 물리 기반 이동 실행 (이제 파라미터가 필요 없습니다)
        movement.Move();
    }

    private void LookAtMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane groundPlane = new Plane(Vector3.up, transform.position);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 worldPoint = ray.GetPoint(distance);
            Vector3 lookDirection = worldPoint - transform.position;
            lookDirection.y = 0f;

            if (lookDirection.sqrMagnitude > 0.01f)
            {
                movement.Rotate(lookDirection);
            }
        }
    }

    #region Input Handlers
    private void OnMove(Vector2 value)
    {
        moveInput = value;
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            playerAnimator.DashAnimation();
            movement.Dash(); // 파라미터가 필요 없습니다.
        }
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
            playerAttack.StartAttack();
        else if (context.canceled)
            playerAttack.CancelAttack();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        
    }
    #endregion
}
