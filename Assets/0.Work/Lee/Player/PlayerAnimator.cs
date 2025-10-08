using UnityEngine;

// [추가] Animator 컴포넌트가 필수로 존재하도록 보장합니다.
[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;

    [Header("애니메이션 기준 이동 속도")]
    [SerializeField] private float forwardAnimDistancePerSecond = 1.5f;
    [SerializeField] private float strafeAnimDistancePerSecond = 1.0f;
    // [추가] 의미를 알 수 없던 숫자(0.1f) 대신, Inspector에서 조절 가능한 배율 변수를 추가했습니다.
    [SerializeField, Tooltip("애니메이션 재생 속도에 곱해지는 보정값입니다.")]
    private float animationSpeedMultiplier = 1.0f;

    private void Awake() // [수정] Start -> Awake로 변경하여 다른 스크립트와 통일
    {
        animator = GetComponent<Animator>();
    }

    public void UpdateAnimation( Vector3 moveDir, float moveSpeed, Vector2 moveInput )
    {
        Vector3 localDir = transform.InverseTransformDirection(moveDir);

        float threshold = 0.1f;
        float moveX = Mathf.Abs(localDir.x) > threshold ? localDir.x : 0f;
        float moveY = Mathf.Abs(localDir.z) > threshold ? localDir.z : 0f;

        animator.SetFloat("MoveX", moveX);
        animator.SetFloat("MoveY", moveY);

        // [추가] 캐릭터가 멈춰있을 때는 불필요한 계산을 하지 않도록 조건을 추가했습니다.
        if (moveSpeed > 0.1f)
        {
            float animDistancePerSecond = GetAnimDistancePerSecond(moveInput);
            if ( animDistancePerSecond > 0.0001f )
            {
                animator.speed = (moveSpeed / animDistancePerSecond) * animationSpeedMultiplier;
            }
            else
            {
                animator.speed = 1f;
            }
        }
        else
        {
            // [추가] 멈췄을 때는 애니메이션 재생 속도를 기본값(1)으로 되돌립니다.
            animator.speed = 1f;
        }
    }

    public float GetAnimDistancePerSecond( Vector2 moveInput )
    {
        if ( Mathf.Abs(moveInput.y) > Mathf.Abs(moveInput.x) )
            return forwardAnimDistancePerSecond;
        else if ( Mathf.Abs(moveInput.x) > 0.01f )
            return strafeAnimDistancePerSecond;
        return 1f;
    }

    public void DashAnimation()
    {
        animator.SetTrigger("Dash");
    }
}