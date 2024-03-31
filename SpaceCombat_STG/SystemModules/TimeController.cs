using System.Collections;
using UnityEngine;

public class TimeController : Singleton<TimeController>
{
    [SerializeField, Range(0, 2)] private float bulletTimeScale = .1f;

    float defaultFixedDeltaTime;
    float timeScaleBeforePause;
    float t = 0f;
    
    protected override void Awake()
    {
        base.Awake();
        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void Pause()
    {
        timeScaleBeforePause = Time.timeScale;//保存暂停时子弹时间  时间刻度
        Time.timeScale = 0f;
    }
    
    public void UnPause()
    {
        Time.timeScale = timeScaleBeforePause;//继续时，恢复时间刻度。
    }

    //子弹时间
    public void BulletTime(float duration)
    {
        Time.timeScale = bulletTimeScale;

        StartCoroutine(SlowOutCoroutine(duration));
    }
    
    public void BulletTime(float inDuration,float outDuration)
    {
        StartCoroutine(SlowInAndOutCoroutine(inDuration,outDuration));
    }
    
    public void BulletTime(float inDuration,float keepingDuration ,float outDuration)
    {
        StartCoroutine(SlowInKeepAndOutDuration(inDuration,keepingDuration,outDuration));
    }


    public void SlowIn(float duration)
    {
        StartCoroutine(SlowInCoroutine(duration));
    }
    
    public void SlowOut(float duration)
    {
        StartCoroutine(SlowOutCoroutine(duration));
    }

    IEnumerator SlowInKeepAndOutDuration(float inDuration, float keepingDuration, float outDuration)
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));
        yield return new WaitForSecondsRealtime(keepingDuration);
        StartCoroutine(SlowOutCoroutine(outDuration));
    }
    

    IEnumerator SlowInAndOutCoroutine(float inDuration, float outDuration)
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));
        StartCoroutine(SlowOutCoroutine(outDuration));
    }
    
    
    IEnumerator SlowOutCoroutine(float duration)
    {
        t = 0f;
        //float startTime = Time.unscaledTime;
        while (t < 1f)
        {
            if (GameManager.GameState != GameState.Paused)
            {
                t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(bulletTimeScale, 1f, t);
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
            }
            yield return null;
        }
        //Debug.Log("time waste:"+(Time.unscaledTime-startTime));
    }
    
    IEnumerator SlowInCoroutine(float duration)
    {
        t = 0f;
        //float startTime = Time.unscaledTime;
        while (t < 1f)
        {
            if (GameManager.GameState != GameState.Paused)
            {
                t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(1f,bulletTimeScale, t);
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
            }
            yield return null;
        }
        //Debug.Log("time waste:"+(Time.unscaledTime-startTime));
    }
}
