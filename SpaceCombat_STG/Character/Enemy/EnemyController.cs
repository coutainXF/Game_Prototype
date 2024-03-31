using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    [Header("-----------敌人移动控制-----------")]
    protected float paddingX;
    protected float paddingY;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float moveRotationAngle = 25f;
    protected Vector3 targetPos;

    [Header("-----------敌人开火参数-----------")]
    [SerializeField] protected GameObject[] projectiles;
    [SerializeField] protected AudioData[] projectileLaunchSFX;//不同敌人子弹发射音效
    [SerializeField] protected Transform muzzle;
    [SerializeField] protected float minFireInterval;
    [SerializeField] protected float maxFireInterval;
    [SerializeField] protected ParticleSystem muzzleVFX;
    
    protected WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();

    protected virtual void Awake()
    {
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2;
        paddingY = size.y / 2;
    }

    protected virtual void OnEnable()
    {
        StartCoroutine(nameof(RandomlyMovingCoroutine));
        StartCoroutine(nameof(RandomlyFireCoroutine));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    //ai随机移动脚本
    protected virtual IEnumerator RandomlyMovingCoroutine()
    {
        //指定生成敌人的位置
        transform.position = ViewPort.Instance.RandomEnemySpawnPosition(paddingX, paddingY);

        targetPos = ViewPort.Instance.RandomRightHalfPosition(paddingX, paddingY);

        while (gameObject.activeSelf)
        {
            //if has not arrived targetPos
            if (Vector3.Distance(transform.position, targetPos) >= moveSpeed * Time.fixedDeltaTime)
            {
                //keep moving to targetPos
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.fixedDeltaTime);
                transform.rotation = Quaternion.AngleAxis((targetPos - transform.position).normalized.y * moveRotationAngle,Vector3.right);
            }
            else
            {
                targetPos = ViewPort.Instance.RandomRightHalfPosition(paddingX, paddingY);
            }
            
            //else
            //set a new targetPos

            yield return _waitForFixedUpdate;
        }
    }

    protected virtual IEnumerator RandomlyFireCoroutine()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(minFireInterval,maxFireInterval));
            
            if (GameManager.GameState == GameState.GameOver) yield break;//如果游戏状态为游戏结束时，结束这个协程
            
            foreach (var projectile in projectiles)
            {
                PoolManager.Release(projectile, muzzle.position);
            }
            AudioManager.Instance.PlayRandomSFX(projectileLaunchSFX);
            muzzleVFX.Play();
        }
    }
}
