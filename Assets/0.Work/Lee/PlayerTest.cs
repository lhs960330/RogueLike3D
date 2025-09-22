using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTest : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 moveInput;
    private void Start()
    {
        // �ʱ� Ȱ��ȭ
        Manager.EXInput.ChangeInput(Define.ActionMap.Player);
    }
    private void Update()
    {
        Move();
    }
    private void OnMove( InputValue value )
    {
        // Player Map�� Ȱ��ȭ�� ���¿����� ó��
        if ( Manager.EXInput.CurrentMap != Define.ActionMap.Player ) return;

        moveInput = value.Get<Vector2>();
        Debug.Log("�̵�");
        // ĳ���� �̵� ó��
    }
    private void Move()
    {
        if ( moveInput != Vector2.zero )
        {
            Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y);
            transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    private void OnCancel()
    {
        if ( Manager.EXInput.CurrentMap == Define.ActionMap.Player )
            Manager.EXInput.OnUIOpen();
        else if ( Manager.EXInput.CurrentMap == Define.ActionMap.UI )
            Manager.EXInput.OnUIClose();
    }
}
