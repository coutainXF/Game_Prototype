using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    [Header("PlayerInput")]
    [SerializeField] PlayerInput _playerInput;

    [Header("Audio Data")]
    [SerializeField] AudioData pauseSFX;
    [SerializeField] AudioData unPauseSFX;
    
    [Header("Canvas")]
    [SerializeField] Canvas hudCanvas;
    [SerializeField] Canvas menusCanvas;

    [Header("Player Input Button")] 
    [SerializeField] Button resumeBtn;
    [SerializeField] Button optionsBtn;
    [SerializeField] Button mainMenuBtn;

    private int buttonPressedHashID = Animator.StringToHash("Pressed");
    void OnEnable()
    {
        _playerInput.onPause += Pause;
        _playerInput.onUnPause += UnPause;
        
        // resumeBtn.onClick.AddListener(OnResumeButtonClick);
        // settingsBtn.onClick.AddListener(OnOptionsButtonClick);
        // mainMenuBtn.onClick.AddListener(OnMainMenuButtonClick);
        
        ButtonPressedBehavior.buttonFunctionTable.Add(resumeBtn.gameObject.name,OnResumeButtonClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(optionsBtn.gameObject.name,OnOptionsButtonClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(mainMenuBtn.gameObject.name,OnMainMenuButtonClick);
    }
    
    void OnDisable()
    {
        _playerInput.onPause -= Pause;
        _playerInput.onUnPause -= UnPause;
        
        ButtonPressedBehavior.buttonFunctionTable.Clear();//清空
        // resumeBtn.onClick.RemoveAllListeners();
        // settingsBtn.onClick.RemoveAllListeners();
        // mainMenuBtn.onClick.RemoveAllListeners();
    }

    //取消暂停
    public void UnPause()
    {
        resumeBtn.Select();
        resumeBtn.animator.SetTrigger(buttonPressedHashID);
        AudioManager.Instance.PlaySFX(unPauseSFX);
    }

    //暂停
    void Pause()
    {
        GameManager.GameState = GameState.Paused;
        TimeController.Instance.Pause();
        hudCanvas.enabled = false;
        menusCanvas.enabled = true;
        _playerInput.EnableUIMenuInput();
        _playerInput.SwitchToDynamicUpdate();
        AudioManager.Instance.PlaySFX(pauseSFX);
        UIInput.Instance.SelectUI(resumeBtn);//自动选择按钮
    }
    
    //继续游戏按钮
    void OnResumeButtonClick()
    {
        GameManager.GameState = GameState.Playing;
        TimeController.Instance.UnPause();
        hudCanvas.enabled = true;
        menusCanvas.enabled = false;
        _playerInput.EnableGameplayInput();
        _playerInput.SwitchToFixedUpdate();
    }

    //设置按钮
    void OnOptionsButtonClick()
    {
        //options
        UIInput.Instance.SelectUI(optionsBtn);
        _playerInput.EnableUIMenuInput();
    }

    //主菜单按钮
    void OnMainMenuButtonClick()
    {
        menusCanvas.enabled = false;
        SceneLoader.Instance.LoadMainMenuScene();
    }
}
