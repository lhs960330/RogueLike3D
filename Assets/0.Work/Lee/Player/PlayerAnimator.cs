using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float forwardAnimDistancePerSecond = 1.5f;
    [SerializeField] private float strafeAnimDistancePerSecond = 1.0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void UpdateAnimation( Vector2 moveInput, float moveSpeed )
    {
        animator.SetFloat("MoveX", moveInput.x);
        animator.SetFloat("MoveY", moveInput.y);

        float animDistancePerSecond = 0f;
        if ( Mathf.Abs(moveInput.y) > Mathf.Abs(moveInput.x) )
            animDistancePerSecond = forwardAnimDistancePerSecond;
        else if ( Mathf.Abs(moveInput.x) > 0.01f )
            animDistancePerSecond = strafeAnimDistancePerSecond;

        if ( animDistancePerSecond > 0.0001f )
            animator.speed = moveSpeed / animDistancePerSecond;
        else
            animator.speed = 1f;
    }
    public void DashAnimation()
    {
        animator.SetTrigger("Dash");
    }
}
