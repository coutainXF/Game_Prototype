using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : PersistentSingleton<ScoreManager>
{
    #region Score Display
    public int Score => score;
    private int score;
    private int currentScore;
    private Vector3 scoreTextScale = new Vector3(1.2f,1.2f,1f);

    public void SetPlayerName(string newName)
    {
        this.playerName = newName;
    }
    
    //重置分数
    public void ResetScore()
    {
        score = 0;
        currentScore = 0;
        ScoreDisplay.UpdateText(score);
    }

    //得分
    public void AddScore(int scorePoint)
    {
        currentScore += scorePoint;
        StartCoroutine(AddScoreCoroutine());
    }

    IEnumerator AddScoreCoroutine()
    {
        ScoreDisplay.ScaleText(scoreTextScale);
        while (score<currentScore)
        {
            score += 1;
            ScoreDisplay.UpdateText(score);
            yield return null;
        }
        ScoreDisplay.ScaleText(Vector3.one);
    }

    #endregion


    #region HIGH SCORE SYSTEM

    [System.Serializable]public class PlayerScore
    {
        public int score;
        public string playerName;

        public PlayerScore(int score, string playerName)
        {
            this.score = score;
            this.playerName = playerName;
        }
    }

    //用于持久化到json文件中
    [System.Serializable]public class PlayerScoreData
    {
        public List<PlayerScore> list = new List<PlayerScore>();
    }

    readonly string SaveFileName = "player_score.json";

    string playerName = "No Name";
    
    
    //读取玩家得分数据
    public PlayerScoreData LoadPlayerScoreData()
    {
        var playerScoreData = new PlayerScoreData();
        if (SaveSystem.SaveFileExists(SaveFileName))
        {
            playerScoreData = SaveSystem.Load<PlayerScoreData>(SaveFileName);
        }
        else
        {
            while (playerScoreData.list.Count < 10)
            {
                playerScoreData.list.Add(new PlayerScore(0,playerName));
            }
            SaveSystem.Save(SaveFileName,playerScoreData);
        }
        return playerScoreData;
    }
    
    //存储玩家得分数据
    public void SavePlayerScoreData()
    {
        var playerScoreData = LoadPlayerScoreData();
        playerScoreData.list.Add(new PlayerScore(score,playerName));
        //根据玩家分数进行排序
        playerScoreData.list.Sort((x,y) => y.score.CompareTo(x.score));
        SaveSystem.Save(SaveFileName,playerScoreData);
    }

    public bool HasNewHighScore => score > LoadPlayerScoreData().list[9].score;

    #endregion
}
