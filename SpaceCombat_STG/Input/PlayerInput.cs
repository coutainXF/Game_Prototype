using System;
using System.Collections;
using System.Collections.Generic;using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Input/PlayerInput",fileName = "PlayerInput")]
public class PlayerInput : 
    ScriptableObject,
    InputActions.IGamePlayActions,
    InputActions.IPauseMenuActions,
    InputActions.IGameOverScreenActions
{
    InputActions _inputActions;
    public event UnityAction<Vector2> onMove = delegate{  }; //移动
    public event UnityAction onStopMove = delegate{  }; //停止移动
    public event UnityAction onFire = delegate { };//开火
    public event UnityAction onStopFire = delegate {  };//停止开火
    public event UnityAction onDodge = delegate {  };//闪避
    public event UnityAction onOverdrive = delegate {  };//能量爆发
    public event UnityAction onPause = delegate {  };//开启暂停菜单
    public event UnityAction onUnPause = delegate {  };//关闭暂停菜单
    public event UnityAction onLaunchMissile = delegate {  };//发射导弹
    public event UnityAction onConfirmGameOver = delegate {  };//确认游戏结束
    
    void OnEnable()
    {
        _inputActions = new InputActions();
        
        _inputActions.GamePlay.SetCallbacks(this);//回调
        _inputActions.PauseMenu.SetCallbacks(this);//回调
        _inputActions.GameOverScreen.SetCallbacks(this);//回调
    }
    
    void OnDisable()
    {
        DisableAllInputs();
    }
    
    void SwitchActionMap(InputActionMap actionMap,bool isUIInput)
    {
        _inputActions.Disable();
        actionMap.Enable();
        if (isUIInput)
        {
            //开启鼠标
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; 
        }else
        {
            //隐藏鼠标
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked; 
        }
    }
    
    public void DisableAllInputs()
    {
        _inputActions.Disable();//关闭所有输入
    }

    public void EnableGameplayInput()
    {
        //启用游戏时动作表
        SwitchActionMap(_inputActions.GamePlay,false);
    }
    
    public void EnableUIMenuInput()
    {
        //启用暂停菜单时动作表
        SwitchActionMap(_inputActions.PauseMenu,true);

    }

    public void EnableGameOverScreenInput()
    {
        //启用游戏结束的动作表
        SwitchActionMap(_inputActions.GameOverScreen,false);
        
    }

    public void SwitchToDynamicUpdate() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;

    public void SwitchToFixedUpdate() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onMove.Invoke(context.ReadValue<Vector2>());
        }

        if (context.canceled)
        {
            onStopMove.Invoke();
        }
        
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            onFire.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            onStopFire.Invoke();
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onDodge.Invoke();
        }
    }

    public void OnOverdrive(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onOverdrive.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onPause.Invoke();
        }
    }
    
    public void OnUnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onUnPause.Invoke();
        }
    }
    
    public void OnLaunchMissile(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onLaunchMissile.Invoke();
        }
    }

    public void OnConfirmGameOver(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onConfirmGameOver.Invoke();
        }
    }
}