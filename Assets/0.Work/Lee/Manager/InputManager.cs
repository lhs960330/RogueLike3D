using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    // 에셋 폴더에있는 inputAction 가지고 있기
    [SerializeField] InputActionAsset inputActions;
    // 현재 InputMap이 가리키고 있는 상태?(Player, UI등)
    public Define.ActionMap CurrentMap { get; private set; }

    // InputMap 변경 함수
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

    // 이걸로 UI Open Close하면될듯?
    // 주의점 : 설정키 이름이 같아야됨 InputAction에 Player랑 UI로 나눠져 있어서 설정키가 같은 키여야지만 전환이 가능
    public void OnUIOpen()
    {
        ChangeInput(Define.ActionMap.UI);
        Debug.Log("UI 활성화");
    }

    public void OnUIClose()
    {
        ChangeInput(Define.ActionMap.Player);
        Debug.Log("Player 활성화");
    }
}
