using UnityEngine.SceneManagement;
using UnityEngine;
public class SceneLoader
{
    public static void ReloadScene()
    {
        int sceneIndex=SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneBuildIndex:sceneIndex);
    }
    
    public static void LoadNextScene()
    {
        int sceneIndex=SceneManager.GetActiveScene().buildIndex + 1;
        if (sceneIndex > SceneManager.sceneCount-1)
        {
            sceneIndex=SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(sceneBuildIndex:sceneIndex);
            return;
        }
        SceneManager.LoadScene(sceneBuildIndex:sceneIndex);
    }

    public static void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

