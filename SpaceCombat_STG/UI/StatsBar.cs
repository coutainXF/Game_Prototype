using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{
    [SerializeField] Image fillImageBack;
    [SerializeField] Image fillImageFront;
    [SerializeField] bool delayFill = true;//是否开启状态条填充延迟
    [SerializeField] float fillDelay = .5f;//状态条更新延迟
    [SerializeField] float fillSpeed = .1f;//状态条填充速度
    
    float t;
    WaitForSeconds waitForDelayFill; 
    Coroutine bufferedFillingCoroutine;
    
    float currentFillAmount;//图片的当前填充值
    float previousFillAmount;
    protected float targetFillAmount;//图片的目标填充值
    
    void Awake()
    {
        if (TryGetComponent<Canvas>(out Canvas _canvas))
        {
            _canvas.worldCamera = Camera.main;
        }
        waitForDelayFill = new WaitForSeconds(fillDelay);
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// 实现状态条初始化的函数
    /// </summary>
    /// <param name="currentValue">当前值</param>
    /// <param name="maxValue">最大值</param>
    public virtual void Initialize(float currentValue,float maxValue)
    {
        currentFillAmount = currentValue / maxValue;
        targetFillAmount = currentFillAmount;
        fillImageBack.fillAmount = currentFillAmount;
        fillImageFront.fillAmount = currentFillAmount;
    }

    public void UpdateStats(float currentValue,float maxValue)
    {
        targetFillAmount = currentValue / maxValue;

        if (bufferedFillingCoroutine != null)
        {
            StopCoroutine(bufferedFillingCoroutine);
        }
        
        //if stats reduce   如果状态减少
        if (currentFillAmount > targetFillAmount)
        {
            //fill image front' fill amount = target fill amount  立即更新前面图片的填充值
            fillImageFront.fillAmount = targetFillAmount;
            //slowly reduce fill image back's fill amount 缓慢更新后面图片的填充值
            bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageBack));
            return;
        }
        
        //if stats increase
        if (currentFillAmount < targetFillAmount)
        {
            //fill image back' fill amount = target fill amount
            fillImageBack.fillAmount = targetFillAmount;
            
            //slowly increase fill image front's fill amount
            bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageFront));
        }
    }

    protected virtual IEnumerator BufferedFillingCoroutine(Image image)
    {
        if (delayFill)
        {
            yield return waitForDelayFill;
        }

        previousFillAmount = currentFillAmount;
        t = 0f;
        
        while (t<1f)
        {
            t += Time.deltaTime * fillSpeed;
            currentFillAmount = Mathf.Lerp(previousFillAmount, targetFillAmount, t);
            image.fillAmount = currentFillAmount;
            yield return null;
        }
    }
}