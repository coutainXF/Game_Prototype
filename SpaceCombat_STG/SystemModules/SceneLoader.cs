using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : PersistentSingleton<SceneLoader>
{ 
    const string GAMEPLAY = "GamePlay"; 
    const string MAIN_MENU = "MainMenu"; 
    const string SCORING = "Scoring";
    [SerializeField] Image transitionImage;
    [SerializeField] float fadeTime = 3.5f;

    Color _color;
    
    void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator LoadingCoroutine(string sceneName)
    {
        //load new scene in background and
        var loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        //set this scene inactive
        loadingOperation.allowSceneActivation = false;
        
        transitionImage.gameObject.SetActive(true);
        //Fade out
        while (_color.a<1f)
        {
            _color.a = Mathf.Clamp01(_color.a += Time.unscaledDeltaTime / fadeTime);
            transitionImage.color = _color;
            yield return null;
        }
        //判断加载进度是否超过90%，如果是则往下执行激活场景，否则继续异步加载直到达到要求进度
        yield return new WaitUntil( () => loadingOperation.progress >= .9f );
        //active the new scene
        loadingOperation.allowSceneActivation = true;
        
        //Fade in
        while (_color.a>0f)
        {
            _color.a = Mathf.Clamp01(_color.a -= Time.unscaledDeltaTime / fadeTime);
            transitionImage.color = _color;
            yield return null;
        }
        transitionImage.gameObject.SetActive(false);
    }

    public void LoadGamePlayScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(GAMEPLAY));
    }
    
    //加载主菜单场景
    public void LoadMainMenuScene()
    {        
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(MAIN_MENU));
    }

    public void LoadScoringScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(SCORING));
    }
}