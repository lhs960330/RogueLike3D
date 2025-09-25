using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerAnimator animator;

    // �̺κ��� ĳ�� ������ ������ �ѱ� ����
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.2f;

    private Vector2 moveInput;
    private Vector2 dashInput;
    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
        animator = GetComponent<PlayerAnimator>();
    }
    private void OnMove( InputValue value )
    {
        moveInput = value.Get<Vector2>();
    }
    IEnumerator TestCoroine()
    {
        yield return new WaitForSeconds(1f);
    }
    private void OnDash( InputValue value )
    {
        if ( moveInput == Vector2.zero ) return;
        if ( !movement.isDashing )
        {
            animator.DashAnimation();
            movement.Dash(dashInput, dashSpeed);
        }
    }

    private void Update()
    {
        Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y);

        //movement.Move(moveDir, moveSpeed);

        // �ִϸ��̼� �ӵ� ����ȭ
        animator.UpdateAnimation(moveDir, moveSpeed, moveInput);
    }

}
