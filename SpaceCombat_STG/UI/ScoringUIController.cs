using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ScoringUIController : MonoBehaviour
{
    [Header("bakcground")]
    [SerializeField] Image background;
    [SerializeField] Sprite[] backgroundImages;

    [Header("scoring screen")] 
    [SerializeField] Canvas scoringScreenCanvas;
    [SerializeField] Text playerScoreText;
    [SerializeField] Button buttonMainMenu;
    [SerializeField] Button buttonQuit;
    [SerializeField] Transform highScoreLeaderBoardContainer;

    [Header("High Score screen")] 
    [SerializeField] Canvas newHighScoreScreenCanvas;
    [SerializeField] Button buttonCancel;
    [SerializeField] Button buttonSubmit;
    [SerializeField] InputField playerNameInputField;
    void Start()
    {
        ShowRandomBackground();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        if (ScoreManager.Instance.HasNewHighScore)
        {
            ShowHasNewHighScoreScreen();
        }
        else
        {
            ShowScoringScreen();
        }
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonMainMenu.gameObject.name,OnButtonMainMenuClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonQuit.gameObject.name,OnButtonQuitClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonSubmit.gameObject.name,OnButtonSubmitClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonCancel.gameObject.name,HideNewHighScoreScreen);
        GameManager.GameState = GameState.Scoring;
    }
    
    void OnButtonQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void OnDisable()
    {
        ButtonPressedBehavior.buttonFunctionTable.Clear();
    }

    void ShowRandomBackground()
    {
        background.sprite = backgroundImages[Random.Range(0,backgroundImages.Length)];
    }

    void ShowScoringScreen()
    {
        scoringScreenCanvas.enabled = true;
        playerScoreText.text = ScoreManager.Instance.Score.ToString();
        UIInput.Instance.SelectUI(buttonMainMenu);
        //TODO: Update high score leaderboard UI
        UpdateHighScoreLeaderBoard();
    }

    void ShowHasNewHighScoreScreen()
    {
        newHighScoreScreenCanvas.enabled = true;
        UIInput.Instance.SelectUI(buttonSubmit);
    }
    void OnButtonMainMenuClicked()
    {
        scoringScreenCanvas.enabled = false;
        SceneLoader.Instance.LoadMainMenuScene();
    }

    
    void UpdateHighScoreLeaderBoard()
    {
        var playerScoreList = ScoreManager.Instance.LoadPlayerScoreData().list;
        
        for(int i=0; i<highScoreLeaderBoardContainer.childCount; i++)
        {
            var child = highScoreLeaderBoardContainer.GetChild(i);
            child.Find("Rank").GetComponent<Text>().text = (i+1).ToString();
            child.Find("Score").GetComponent<Text>().text = playerScoreList[i].score.ToString();
            child.Find("Name").GetComponent<Text>().text = playerScoreList[i].playerName;
        }
    }

    public void OnButtonSubmitClicked()
    {
        if (!string.IsNullOrEmpty(playerNameInputField.text))
        {
            ScoreManager.Instance.SetPlayerName(playerNameInputField.text);
        }
        HideNewHighScoreScreen();
    }

    public void HideNewHighScoreScreen()
    {
        newHighScoreScreenCanvas.enabled = false;//隐藏新高分UI
        ScoreManager.Instance.SavePlayerScoreData();
        ShowRandomBackground();
        ShowScoringScreen();
    }
    
}
