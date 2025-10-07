using UnityEngine;
using UnityEngine.InputSystem;

// [수정] 이 스크립트에 필요한 컴포넌트가 없으면 자동으로 추가해주는 어트리뷰트입니다.
// PlayerMovement나 PlayerAnimator가 실수로 삭제되어도 null 예외가 발생하는 것을 방지합니다.
[RequireComponent(typeof(PlayerMovement), typeof(PlayerAnimator))]
public class PlayerController : MonoBehaviour
{
    // [수정] Inspector에서 할당하는 [SerializeField]를 제거하고 private으로 변경했습니다.
    // 컴포넌트 참조는 Awake에서 GetComponent로 통일하여 코드의 일관성을 높입니다.
    private PlayerMovement movement;
    private PlayerAnimator animator;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashSpeed = 10f;
    // [제거] dashDuration 변수는 PlayerMovement 스크립트로 이동하여 역할을 중앙에서 관리하도록 했습니다.

    private Vector2 moveInput;

    // [수정] Start 대신 Awake에서 컴포넌트를 찾는 것이 더 안전합니다.
    // 다른 스크립트의 Awake에서 이 컴포넌트를 참조할 때 null이 되는 것을 방지합니다.
    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        animator = GetComponent<PlayerAnimator>();
    }

    private void OnMove( InputValue value )
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnDash( InputValue value )
    {
        if ( moveInput == Vector2.zero ) return;
        if ( !movement.isDashing )
        {
            // [수정] InputSystem의 Vector2 값을 올바른 3D 월드 방향(x, 0, z)으로 변환합니다.
            // 이전 코드에서는 Vector2(x, y)가 Vector3(x, y, 0)으로 잘못 변환되는 버그가 있었습니다.
            Vector3 dashDirection = new Vector3(moveInput.x, 0, moveInput.y);
            animator.DashAnimation();
            movement.Dash(dashDirection, dashSpeed); // 수정된 3D 방향 벡터를 전달합니다.
        }
    }

    private void Update()
    {
        Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y);
        movement.SetMoveDirection(moveDir);

        // [수정] 애니메이션 속도 계산을 위해 PlayerMovement에서 실제 이동 속도(CurrentSpeed)를 가져옵니다.
        // 이렇게 하면 대시 같은 특수한 움직임의 속도도 애니메이션에 정확히 반영할 수 있습니다.
        animator.UpdateAnimation(moveDir, movement.CurrentSpeed, moveInput);
    }

    private void FixedUpdate()
    {
        movement.Move(moveSpeed);
    }
}