using UnityEngine;

// [추가] Animator 컴포넌트가 필수로 존재하도록 보장합니다.
[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;


    private void Awake() // [수정] Start -> Awake로 변경하여 다른 스크립트와 통일
    {
        animator = GetComponent<Animator>();
    }

    public void UpdateAnimation(Vector3 moveDir)
    {
        Vector3 localDir = transform.InverseTransformDirection(moveDir);

        float threshold = 0.1f;
        float moveX = Mathf.Abs(localDir.x) > threshold ? localDir.x : 0f;
        float moveY = Mathf.Abs(localDir.z) > threshold ? localDir.z : 0f;

        animator.SetFloat("MoveX", moveX);
        animator.SetFloat("MoveY", moveY);

    }


    public void DashAnimation()
    {
        animator.SetTrigger("Dash");
    }

    public void AttackAnimation(bool isAttack)
    {
        animator.SetBool("Attack", isAttack);
    }

}