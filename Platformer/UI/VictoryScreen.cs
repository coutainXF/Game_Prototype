using UnityEngine;
using UnityEngine.UI;
public class VictoryScreen : MonoBehaviour
{
    [SerializeField] private VoidEventChannel victoryEventChannel;
    [SerializeField] private StringEventChannel levelClearEventChannel;
    [SerializeField] private Text timeTxt;
    [SerializeField] private Button nextLevelBtn;
    private void OnEnable()
    {
        victoryEventChannel.AddListener(ShowUI);
        levelClearEventChannel.AddListener(UpdateTimeTxt);
        nextLevelBtn.onClick.AddListener(SceneLoader.LoadNextScene);
    }
    
    private void OnDisable()
    {
        victoryEventChannel.RemoveListener(ShowUI);
        levelClearEventChannel.RemoveListener(UpdateTimeTxt);
        nextLevelBtn.onClick.RemoveListener(SceneLoader.LoadNextScene);
    }
    
    private void ShowUI()
    {
        GetComponent<Canvas>().enabled = true;
        GetComponent<Animator>().enabled = true;
    }
    
    private void UpdateTimeTxt(string obj)
    {
        timeTxt.text = obj;
    }
}
