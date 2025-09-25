using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [Header("애니메이션 단위 이동 속도")]
    [SerializeField] private float forwardAnimDistancePerSecond = 1.5f;
    [SerializeField] private float strafeAnimDistancePerSecond = 1.0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // 현재 이동 입력 기준으로 애니메이션 단위 속도 가져오기
    public float GetAnimDistancePerSecond( Vector2 moveInput )
    {
        if ( Mathf.Abs(moveInput.y) > Mathf.Abs(moveInput.x) )
            return forwardAnimDistancePerSecond;
        else if ( Mathf.Abs(moveInput.x) > 0.01f )
            return strafeAnimDistancePerSecond;
        return 1f; // 정지 상태일 때
    }

    // 이동 속도에 맞춰 Animator 속도 동적 싱크
    public void UpdateAnimation( Vector3 moveDir, float moveSpeed, Vector2 moveInput )
    {
        // 로컬 방향 기준
        Vector3 localDir = transform.InverseTransformDirection(moveDir);

        float threshold = 0.1f;
        float moveX = Mathf.Abs(localDir.x) > threshold ? localDir.x : 0f;
        float moveY = Mathf.Abs(localDir.z) > threshold ? localDir.z : 0f;

        animator.SetFloat("MoveX", moveX);
        animator.SetFloat("MoveY", moveY);

        // 애니메이션 속도 보정
        float animDistancePerSecond = GetAnimDistancePerSecond(moveInput);
        if ( animDistancePerSecond > 0.0001f )
        {
            animator.speed = (moveSpeed / animDistancePerSecond)*0.1f;
            Debug.Log(moveSpeed / animDistancePerSecond);
        }
        else
            animator.speed = 1f; // 정지 상태
    }

    public void DashAnimation()
    {
        animator.SetTrigger("Dash");
    }
}
