using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [Header("canvas")] 
    [SerializeField] Canvas _mainmenuCanvas;
    
    [Header("buttons")]
    [SerializeField] Button startGameBtn;
    [SerializeField] Button optionsBtn;
    [SerializeField] Button quitBtn;
    void OnEnable()
    {
        ButtonPressedBehavior.buttonFunctionTable.Add(optionsBtn.gameObject.name,OnButtonOptionClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(startGameBtn.gameObject.name,OnStartGameBtnClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(quitBtn.gameObject.name,OnButtonQuitClicked);
    }

    void OnDisable()
    {
        ButtonPressedBehavior.buttonFunctionTable.Clear();
    }

    void Start()
    {
        Time.timeScale = 1f;
        GameManager.GameState = GameState.Playing;
        UIInput.Instance.SelectUI(startGameBtn);//默认选中开始游戏
    }
    
    void OnStartGameBtnClick()
    {
        _mainmenuCanvas.enabled = false;
        SceneLoader.Instance.LoadGamePlayScene();
    }

    void OnButtonOptionClick()
    {
        UIInput.Instance.SelectUI(optionsBtn);
    }

    void OnButtonQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
