using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class UIInput : Singleton<UIInput>
{
    [SerializeField] PlayerInput _playerInput;
    InputSystemUIInputModule UIInputModule;
    
    protected void OnEnable()
    {
        base.Awake();
        UIInputModule = GetComponent<InputSystemUIInputModule>();
        UIInputModule.enabled = false;//只在需要时启用
    }

    public void SelectUI(Selectable UIObject)//将UI选中
    {
        UIObject.Select();
        UIObject.OnSelect(null);
        UIInputModule.enabled = true;
    }

    public void DisableAllUIInput()
    {
        _playerInput.DisableAllInputs();
        UIInputModule.enabled = false;
    }
}
