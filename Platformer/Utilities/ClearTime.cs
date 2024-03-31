using UnityEngine;
using UnityEngine.UI;

public class ClearTime : MonoBehaviour
{
    [SerializeField] private Text timeTxt;
    [SerializeField] private VoidEventChannel levelStartEventChannel;
    [SerializeField] private VoidEventChannel levelClearEventChannel;

    [SerializeField] private StringEventChannel clearTimeEventChannel;
    
    private bool stop = true;
    private float clearTime;
    
    private void OnEnable()
    {
        levelStartEventChannel.AddListener(OnLevelStart);
        levelClearEventChannel.AddListener(OnLevelClear);
    }

    private void OnDisable()
    {
        levelStartEventChannel.RemoveListener(OnLevelStart);
        levelClearEventChannel.RemoveListener(OnLevelClear);
    }
    
    private void OnLevelClear()
    {
        stop = true;
        clearTimeEventChannel.Broadcast(timeTxt.text);
    }

    private void OnLevelStart()
    {
        stop = false;
    }
    
    private void Update()
    {
        if (stop) return;

        clearTime += Time.deltaTime;
        timeTxt.text = System.TimeSpan.FromSeconds(clearTime).ToString(@"mm\:ss\:ff");
    }
}
