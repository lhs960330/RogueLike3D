using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [Header("�ִϸ��̼� ���� �̵� �ӵ�")]
    [SerializeField] private float forwardAnimDistancePerSecond = 1.5f;
    [SerializeField] private float strafeAnimDistancePerSecond = 1.0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // ���� �̵� �Է� �������� �ִϸ��̼� ���� �ӵ� ��������
    public float GetAnimDistancePerSecond( Vector2 moveInput )
    {
        if ( Mathf.Abs(moveInput.y) > Mathf.Abs(moveInput.x) )
            return forwardAnimDistancePerSecond;
        else if ( Mathf.Abs(moveInput.x) > 0.01f )
            return strafeAnimDistancePerSecond;
        return 1f; // ���� ������ ��
    }

    // �̵� �ӵ��� ���� Animator �ӵ� ���� ��ũ
    public void UpdateAnimation( Vector3 moveDir, float moveSpeed, Vector2 moveInput )
    {
        // ���� ���� ����
        Vector3 localDir = transform.InverseTransformDirection(moveDir);

        float threshold = 0.1f;
        float moveX = Mathf.Abs(localDir.x) > threshold ? localDir.x : 0f;
        float moveY = Mathf.Abs(localDir.z) > threshold ? localDir.z : 0f;

        animator.SetFloat("MoveX", moveX);
        animator.SetFloat("MoveY", moveY);

        // �ִϸ��̼� �ӵ� ����
        float animDistancePerSecond = GetAnimDistancePerSecond(moveInput);
        if ( animDistancePerSecond > 0.0001f )
        {
            animator.speed = (moveSpeed / animDistancePerSecond)*0.1f;
            Debug.Log(moveSpeed / animDistancePerSecond);
        }
        else
            animator.speed = 1f; // ���� ����
    }

    public void DashAnimation()
    {
        animator.SetTrigger("Dash");
    }
}
