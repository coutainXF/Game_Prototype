using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : Singleton<EnemyManager>
{
    public bool IsBossWave => waveNum%bossWaveNumber==0;
    public GameObject RandomEnemy => enemyList.Count == 0 ? null : enemyList[Random.Range(0,enemyList.Count)];
    public int WaveNumber => waveNum;
    public float TimeBetweenWaves => timeBetweenWaves;
    
    [SerializeField] private GameObject[] enemyPrefabs;//敌人预制体数组
    
    [SerializeField] private float timeBetweenSpawns = 1f;//生成间隔
    [SerializeField] private int minEnemyAmount = 4;
    [SerializeField] private int maxEnemyAmount = 10;
    [SerializeField] private bool spawnEnemy = true;//是否生成敌人
    [SerializeField] private float timeBetweenWaves = 1f;
    [SerializeField] private GameObject waveUI;
    
    [Header("boss Wave number")]
    [SerializeField] GameObject bossPrefab;//boss预制体
    [SerializeField] int bossWaveNumber = 3;
    
    
    private List<GameObject> enemyList;
    private WaitForSeconds waitTimeBetweenSpawns;
    private WaitForSeconds waitTimeBetweenWave;
    private WaitUntil _waitUntilListEmpty;//直到没有敌人时。
    private int waveNum = 1;//敌人波数
    private int enemyAmount;//敌人数目

    
    
    protected override void Awake()
    {
        base.Awake();
        enemyList = new List<GameObject>();
        waitTimeBetweenSpawns = new WaitForSeconds(timeBetweenSpawns);
        waitTimeBetweenWave = new WaitForSeconds(timeBetweenWaves);
        _waitUntilListEmpty = new WaitUntil(() => enemyList.Count==0);//直到enemyList.Count==0时调用
    }

    private IEnumerator Start()
    {
        while (spawnEnemy && GameManager.GameState!=GameState.GameOver)
        {
            waveUI.SetActive(true);
            yield return waitTimeBetweenWave;
            
            waveUI.SetActive(false);
            yield return StartCoroutine(RandomlySpawnCoroutine());
            
        }
    }

    //随机生成敌人
    IEnumerator RandomlySpawnCoroutine()
    {
        if (waveNum % bossWaveNumber == 0)
        {
            //Spawn Boss
            var boss= PoolManager.Release(bossPrefab);
            enemyList.Add(boss);
            //Add Express
            StartCoroutine(Express.Instance.ExpressCoroutine());
        }
        else
        {
            enemyAmount = Mathf.Clamp(enemyAmount, minEnemyAmount + waveNum / 3, maxEnemyAmount);
            for (int i = 0; i < enemyAmount; i++)
            {
                //var enemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                enemyList.Add(PoolManager.Release(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]));
                yield return waitTimeBetweenSpawns;
            }
        }
        yield return _waitUntilListEmpty;
        waveNum++;
    }

    public void RemoveFromList(GameObject enemy) => enemyList.Remove(enemy);
    
}