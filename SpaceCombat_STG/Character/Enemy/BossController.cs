using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BossController : EnemyController
{
    [SerializeField] float continuousFireDuration = 1.5f;//持续开火时间

    [Header("Player Detection")] 
    [SerializeField] Transform playerDetectionTransform;
    [SerializeField] Vector3 playerDetectionSize;
    [SerializeField] LayerMask playerLayer;

    [Header("---激光相关的参数---")] 
    [SerializeField] float beamCooldownTime = 12f;

    [FormerlySerializedAs("beamLauchSFX")] [SerializeField] AudioData beamLaunchSFX;
    [SerializeField] AudioData beamChargingSFX;
    
    Animator _animator;
    Transform playerTransform;
    
    bool isBeanReady;
    List<GameObject> magazine;
    AudioData launchSFX;
    WaitForSeconds _waitForContinuousFireInterval;
    WaitForSeconds _waitForFireInterval;
    WaitForSeconds _waitForBeamCooldownTime;



    int BeamHashID = Animator.StringToHash("launchBeam");
    
    
    
    
    #region Eventfunction
    protected override void Awake()
    {
        base.Awake();
        _waitForContinuousFireInterval = new WaitForSeconds(minFireInterval);
        _waitForFireInterval = new WaitForSeconds(maxFireInterval);
        _waitForBeamCooldownTime = new WaitForSeconds(beamCooldownTime);
        magazine = new List<GameObject>(projectiles.Length);
        _animator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void OnEnable()
    {
        isBeanReady = false;
        muzzleVFX.Stop();
        StartCoroutine(nameof(BeamCooldownCoroutine));
        base.OnEnable();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(playerDetectionTransform.position,playerDetectionSize);
    }

    #endregion

    #region Fire
    IEnumerator ContinuousFireCoroutine()
    {
        LoadProjectiles();
        muzzleVFX.Play();
        float continuousFireTimer = 0f;
        
        while (continuousFireTimer<continuousFireDuration)
        {
            foreach (var projectile in magazine)
            {
                PoolManager.Release(projectile, muzzle.position);
            }

            continuousFireTimer += minFireInterval;
            AudioManager.Instance.PlayRandomSFX(launchSFX);
            
            yield return _waitForContinuousFireInterval;
        }
        muzzleVFX.Stop();
    }

    void LoadProjectiles()
    {
        magazine.Clear();
        if (PlayerIsInFrontOfBoss)
        {
            //lauch projectile basic direct
            magazine.Add(projectiles[0]);
            launchSFX = projectileLaunchSFX[0];
        }
        else
        {
            //lauch projectile2 or 3
            if (Random.value <.5f)
            {
                magazine.Add(projectiles[1]);
                launchSFX = projectileLaunchSFX[1];
            }
            else
            {
                for (int i = 2; i < projectiles.Length; i++)
                {
                    magazine.Add(projectiles[i]);
                }

                launchSFX = projectileLaunchSFX[2];
            }
        }
    }

    public bool PlayerIsInFrontOfBoss => Physics2D.OverlapBox(playerDetectionTransform.position,playerDetectionSize,playerLayer);

    protected override IEnumerator RandomlyFireCoroutine()
    {
        while (isActiveAndEnabled)
        {
            if(GameManager.GameState == GameState.GameOver) yield break;
            if (isBeanReady)
            {
                StartCoroutine(nameof(ChasingPlayerCoroutine));//追击玩家
                ActiveBeamWeapon();//激活激光武器
                yield break;
            }
            yield return _waitForFireInterval;
            yield return StartCoroutine(nameof(ContinuousFireCoroutine));
        }
    }
    #endregion
    
    
    protected override IEnumerator RandomlyMovingCoroutine()
    {
        //指定生成敌人的位置
        transform.position = ViewPort.Instance.RandomEnemySpawnPosition(paddingX, paddingY);
        
        Vector3 targetPos = ViewPort.Instance.RandomBossPosition(paddingX, paddingY);

        while (gameObject.activeSelf)
        {
            //if has not arrived targetPos
            if (Vector3.Distance(transform.position, targetPos) >= moveSpeed * Time.fixedDeltaTime)
            {
                //keep moving to targetPos
                transform.position =
                    Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.fixedDeltaTime);
                transform.rotation =
                    Quaternion.AngleAxis((targetPos - transform.position).normalized.y * moveRotationAngle,
                        Vector3.right);
            }
            else
            {
                targetPos = ViewPort.Instance.RandomBossPosition(paddingX, paddingY);
            }

            //else
            //set a new targetPos

            yield return _waitForFixedUpdate;
        }
    }

    IEnumerator BeamCooldownCoroutine()
    {
        yield return _waitForBeamCooldownTime;
        isBeanReady = true;
    }

    void ActiveBeamWeapon()
    {
        isBeanReady = false;
        _animator.SetTrigger(BeamHashID);
        AudioManager.Instance.PlayRandomSFX(beamChargingSFX);
    }

    void AE_LaunchBeam()
    {
        AudioManager.Instance.PlayRandomSFX(beamLaunchSFX);
    }
    
    void AE_StopBeam()
    {
        StopCoroutine(nameof(ChasingPlayerCoroutine));//停止追击
        StartCoroutine(nameof(BeamCooldownCoroutine));
        StartCoroutine(nameof(RandomlyFireCoroutine));
    }

    IEnumerator ChasingPlayerCoroutine()
    {
        while (isActiveAndEnabled)
        {
            targetPos.x = ViewPort.Instance.MaxX - paddingX;
            targetPos.y = playerTransform.position.y;
            yield return null;
        }
    }
}
