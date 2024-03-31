using UnityEngine;
using UnityEngine.UI;

public class DefeatScreen : MonoBehaviour
{
    [SerializeField] private VoidEventChannel lostEventChannel;

    [SerializeField] private Button retryBtn;
    [SerializeField] private Button quitBtn;
    
    private void OnEnable()
    {
        lostEventChannel.AddListener(ShowUI);
        retryBtn.onClick.AddListener(SceneLoader.ReloadScene);
        quitBtn.onClick.AddListener(SceneLoader.QuitGame);
        
    }
    
    private void OnDisable()
    {
        lostEventChannel.RemoveListener(ShowUI);
        retryBtn.onClick.RemoveListener(SceneLoader.ReloadScene);
        quitBtn.onClick.RemoveListener(SceneLoader.QuitGame);
    }
    
    private void ShowUI()
    {
        GetComponent<Canvas>().enabled = true;
        GetComponent<Animator>().enabled = true;
    }
}
