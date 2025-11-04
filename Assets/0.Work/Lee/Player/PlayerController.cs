using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerAnimator))]
public class PlayerController : MonoBehaviour, IDamageable
{
    private PlayerInput playerInput;
    private PlayerMovement movement;
    private PlayerAnimator animator;

    // 플레이어 정보(Test용)
    [Header("플레이어 스텟 정보")]
    [SerializeField] int hp = 100;
    [SerializeField] int currenthp;
    [SerializeField] int baseDamage = 10;
    [SerializeField] PlayerAttack attack;

    [Header("플레이어 속도")]
    [SerializeField] private float moveForce = 50f; // 가하는 힘의 크기                                                    
    [SerializeField] private float maxSpeed = 5f; // 최대 속도         
    [SerializeField] private float dashSpeed = 10f;
    private Vector3 moveInput;
    private Camera mainCamera;
    private Vector3 worldMoveDir;

    //OnEnable/OnDisable에서 InputManager의 이벤트를 구독/해제
    private void OnEnable()
    {
        if (InputManager.Instance != null)
        {
            Debug.Log("음/");
            InputManager.Instance.OnMoveEvent += OnMove;
            InputManager.Instance.OnAttackEvent += OnAttack;
            InputManager.Instance.OnDashEvent += OnDash;
        }
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnMoveEvent -= OnMove;
            InputManager.Instance.OnAttackEvent -= OnAttack;
            InputManager.Instance.OnDashEvent -= OnDash;
        }
    }
    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        animator = GetComponent<PlayerAnimator>();
        mainCamera = Camera.main;
        playerInput = GetComponent<PlayerInput>();
        attack = GetComponent<PlayerAttack>();
        currenthp = hp;
    }


    private void Update()
    {
        if (Manager.EXInput.CurrentMap == Define.ActionMap.Player)
            LookAtMouse();

        if (movement.isDashing)
        {
            // 대시 중일 때는 PlayerMovement에 저장된 대시 방향과 대시 속도를 애니메이션에 전달합니다.
            // 이렇게 해야 키보드에서 손을 떼도 애니메이션 방향이 유지됩니다.
            animator.UpdateAnimation(movement.DashDirection);
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
            animator.UpdateAnimation(worldMoveDir);
        }
    }

    private void FixedUpdate()
    {
        movement.Move(maxSpeed, moveForce);
    }



    // 마우스 방향으로 캐릭이 바라보기
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
    #region Input
    private void OnMove(Vector2 value)
    {
        moveInput = value;
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if (worldMoveDir.sqrMagnitude < 0.01f) return;

        if (!movement.isDashing)
        {
            animator.DashAnimation();
            movement.Dash(worldMoveDir, dashSpeed);
        }
    }
    private void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
             attack.StartAttack();
        else if (context.canceled)
            attack.CancelAttack();
    }

    #endregion
    public void TakeDamage(int damage)
    {
        currenthp -= damage;
    }
}
