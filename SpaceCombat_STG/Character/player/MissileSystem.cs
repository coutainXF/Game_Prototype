using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSystem : MonoBehaviour
{
    [SerializeField] GameObject missilePrefab = null;
    [SerializeField] AudioData lauchSFX = null;

    [SerializeField] float cooldownTime = 3f;
    [SerializeField] bool isReady = true;//是否准备
    
    [SerializeField] int defaultAmount = 5;

    [SerializeField] AudioData inform;//提醒音
    
    int amount;

    void Awake()
    {
        amount = defaultAmount;
    }

    void Start()
    {
        MissileDisplay.UpdateAmountText(amount);
    }

    public void Lauch(Transform muzzle)
    {
        if (amount == 0 || !isReady)
        {
            AudioManager.Instance.PlaySFX(inform);
            
            return;
        }
        
        //set ready to fasle
        isReady = false;
        
        //Release a missile clone from object pool
        PoolManager.Release(missilePrefab, muzzle.position);
        //play the sfx;
        AudioManager.Instance.PlayRandomSFX(lauchSFX);

        amount--;
        
        MissileDisplay.UpdateAmountText(amount);

        if (amount == 0)
        {
            MissileDisplay.UpdateCooldownImage(1f);
        }
        else
        {
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown()
    {
        var cooldownValue = cooldownTime;
        while (cooldownValue > 0f)
        {
            MissileDisplay.UpdateCooldownImage(cooldownValue/cooldownTime);
            cooldownValue = Mathf.Max(cooldownValue-Time.deltaTime,0f);
            yield return null;
        }
        isReady = true;
    }

    public void PickUp()
    {
        amount++;
        MissileDisplay.UpdateAmountText(amount);
        if (amount == 1)
        {
            MissileDisplay.UpdateCooldownImage(0f);
            isReady = true;
        }
    }
}
