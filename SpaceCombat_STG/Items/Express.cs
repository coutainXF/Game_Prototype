using System.Collections;
using UnityEngine;

public class Express : Singleton<Express>
{
    //旨在boss战时给玩家提供补给
    [SerializeField] float expressTime = 8f;
    LootSpawner _lootSpawner;
    Vector2 expressPos;//快递范围
    
    WaitForSeconds _waitForExpress;//等待一段时间生成一个快递

    protected override void Awake()
    {
        base.Awake();
        _waitForExpress = new WaitForSeconds(expressTime);
        _lootSpawner = GetComponent<LootSpawner>();
    }

    void OnEnable()
    {
        StartCoroutine(nameof(ExpressCoroutine));
    }

    public IEnumerator ExpressCoroutine()
    {
        while (gameObject.activeSelf)
        {
            if(!EnemyManager.Instance.IsBossWave) yield break;
            expressPos = ViewPort.Instance.RandomExpression();
            yield return _waitForExpress;
            _lootSpawner.Spawn(expressPos);
        }
    }
    
}
