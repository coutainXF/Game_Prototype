using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerProjectileOverdrive : PlayerProjectile
{
    //target?
    //EnemyList,EnemyManger提供随机敌人目标

    [SerializeField] private ProjectileGuidanceSystem _guidanceSystem;
    
    
    protected override void OnEnable()
    {
        SetTarget(EnemyManager.Instance.RandomEnemy);
        transform.rotation = quaternion.identity;
        if (target == null)
        {
            base.OnEnable();
        }
        else
        {
            //track target;
            StartCoroutine(_guidanceSystem.HomingCoroutine(target));
        }
        
    }
}
