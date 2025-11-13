using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : Singleton<InputManager>
{
    [SerializeField] InputActionAsset inputActions;

    [SerializeField] PlayerInput playerInput;

    public Define.ActionMap CurrentMap { get; private set; }
    private bool isSwitchingMap = false;
    public event Action<Vector2> OnMoveEvent;
    public event Action<InputAction.CallbackContext> OnAttackEvent;
    public event Action<InputAction.CallbackContext> OnDashEvent;
    public event Action<InputAction.CallbackContext> OnJumpEvent;
    private void OnEnable()
    {
        // Player 액션 맵의 각 액션에 private 핸들러 함수들을 구독
        inputActions.FindActionMap("Player").FindAction("Move").performed += OnMove;
        inputActions.FindActionMap("Player").FindAction("Move").canceled += OnMove;
        inputActions.FindActionMap("Player").FindAction("Attack").started += OnAttack;
        inputActions.FindActionMap("Player").FindAction("Attack").canceled += OnAttack;
        inputActions.FindActionMap("Player").FindAction("Dash").started += OnDash;
        inputActions.FindActionMap("Player").FindAction("OpenMenu").performed += OnMenuOpen;
        inputActions.FindActionMap("Player").FindAction("Jump").started += OnJump;

        inputActions.FindActionMap("UI").FindAction("CloseMenu").performed += OnMenuClose;

    }

    private void OnDisable()
    {
        // 구독했던 모든 이벤트를 '구독 취소'
        // (메모리 누수 방지)
        inputActions.FindActionMap("Player").FindAction("Move").performed -= OnMove;
        inputActions.FindActionMap("Player").FindAction("Move").canceled -= OnMove;
        inputActions.FindActionMap("Player").FindAction("Attack").started -= OnAttack;
        inputActions.FindActionMap("Player").FindAction("Attack").canceled -= OnAttack;
        inputActions.FindActionMap("Player").FindAction("Dash").started -= OnDash;
        inputActions.FindActionMap("Player").FindAction("OpenMenu").performed -= OnMenuOpen;
        inputActions.FindActionMap("Player").FindAction("Jump").started -= OnJump;

        inputActions.FindActionMap("UI").FindAction("CloseMenu").performed -= OnMenuClose;
    }

    public void ChangeInput(Define.ActionMap map)
    {
        if (CurrentMap == map) return;

        isSwitchingMap = true;

        switch (map)
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
        StartCoroutine(ResetSwitchingFlag());
    }
    private IEnumerator ResetSwitchingFlag()
    {
        // 현재 프레임의 모든 처리가 끝날 때까지 기다립니다.
        yield return new WaitForEndOfFrame();
        isSwitchingMap = false;
    }

    #region Input
    //InputActionAsset으로부터 받은 입력을 외부 C# 이벤트로 다시 전달
    private void OnMenuOpen(InputAction.CallbackContext context)
    {
        if (isSwitchingMap) return;
        {
            ChangeInput(Define.ActionMap.UI);
            Debug.Log("UI 맵으로 변경");
        }
    }

    private void OnMenuClose(InputAction.CallbackContext context)
    {
        if (isSwitchingMap) return;
        {
            ChangeInput(Define.ActionMap.Player);
            Debug.Log("Player 맵으로 변경");
        }
    }


    private void OnMove(InputAction.CallbackContext context)
    {
        // OnMoveEvent를 구독한 모든 대상에게 값을 전달
        OnMoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        OnAttackEvent?.Invoke(context);
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        OnDashEvent?.Invoke(context);
    }
    private void OnJump(InputAction.CallbackContext context)
    {
        OnJumpEvent?.Invoke(context);
    }
      #endregion
}
