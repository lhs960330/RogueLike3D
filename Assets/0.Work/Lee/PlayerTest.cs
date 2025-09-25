using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTest : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] Animator animator;

    private Vector2 moveInput;
    private void Start()
    {
        // 초기 활성화
        Manager.EXInput.ChangeInput(Define.ActionMap.Player);
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        Move();
    }
    private void OnMove( InputValue value )
    {
        // Player Map이 활성화된 상태에서만 처리
        if ( Manager.EXInput.CurrentMap != Define.ActionMap.Player ) return;

        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
        // 캐릭터 이동 처리

    }
    private void Move()
    {
        AniMove();
        if ( moveInput != Vector2.zero )
        {
          
            Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y);
            transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            animator.SetFloat("MoveX", moveInput.x);
            animator.SetFloat("MoveY", moveInput.y);
        }
    }
    private void AniMove()
    {
        // Animator 파라미터 전달
        animator.SetFloat("MoveX", moveInput.x);
        animator.SetFloat("MoveY", moveInput.y);

    }
    private void OnCancel()
    {
        if ( Manager.EXInput.CurrentMap == Define.ActionMap.Player )
            Manager.EXInput.OnUIOpen();
        else if ( Manager.EXInput.CurrentMap == Define.ActionMap.UI )
            Manager.EXInput.OnUIClose();
    }
}
