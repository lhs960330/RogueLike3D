using NUnit.Framework;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement playerMovement;
    private Rigidbody rb;


    private void Awake() // [수정] Start -> Awake로 변경하여 다른 스크립트와 통일
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        UpdateJumpAndGroundedStatus();
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

    // 점프 시작 시 Controller에서 호출
    public void JumpAnimation()
    {
         animator.SetTrigger("Jump");
    }

    // 매 프레임 호출되며, 공중 상태와 착지 상태를 감지하여 파라미터를 넘김
    private void UpdateJumpAndGroundedStatus()
    {
        animator.SetBool("IsGrounded", playerMovement.IsGrounded);
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