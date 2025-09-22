using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    // ���� �������ִ� inputAction ������ �ֱ�
    [SerializeField] InputActionAsset inputActions;
    // ���� InputMap�� ����Ű�� �ִ� ����?(Player, UI��)
    public Define.ActionMap CurrentMap { get; private set; }

    // InputMap ���� �Լ�
    public void ChangeInput( Define.ActionMap map )
    {
        if ( CurrentMap == map ) return;

        switch ( map )
        {
            case Define.ActionMap.Player:
                inputActions.FindActionMap("UI").Disable();
                inputActions.FindActionMap("Player").Enable();
                break;
            case Define.ActionMap.UI:
                inputActions.FindActionMap("Player").Disable();
                inputActions.FindActionMap("UI").Enable();
                break;
        }

        CurrentMap = map;
        Debug.Log(CurrentMap);
    }

    // �̰ɷ� UI Open Close�ϸ�ɵ�?
    // ������ : ����Ű �̸��� ���ƾߵ� InputAction�� Player�� UI�� ������ �־ ����Ű�� ���� Ű�������� ��ȯ�� ����
    public void OnUIOpen()
    {
        ChangeInput(Define.ActionMap.UI);
        Debug.Log("UI Ȱ��ȭ");
    }

    public void OnUIClose()
    {
        ChangeInput(Define.ActionMap.Player);
        Debug.Log("Player Ȱ��ȭ");
    }
}
