using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : Singleton<PlayerEnergy>
{
    [SerializeField] private EnergyBar _energyBar;
    [SerializeField] private float overdriveInterval = .1f;

    private WaitForSeconds waitForOverdriveInterval;
    
    public const int MAX = 100;
    public const int PERCENT = 1;
    [SerializeField,Range(0,100)] private int energy;

    private bool available = true;     //是否能够获取能量

    protected override void Awake()
    {
        base.Awake();
        waitForOverdriveInterval = new WaitForSeconds(overdriveInterval);
    }

    private void Start()
    {
        _energyBar.Initialize(energy,MAX);
    }

    #region 订阅暴走状态委托
    private void OnEnable()
    {
        PlayerOverDrive.on += PlayerOverDriveOn;
        PlayerOverDrive.off += PlayerOverDriveOff;
    }
    
    private void OnDisable()
    {
        PlayerOverDrive.on -= PlayerOverDriveOn;
        PlayerOverDrive.off -= PlayerOverDriveOff;
    }
    #endregion
    

    //获取能量
    public void Obtain(int value)
    {
        if (energy == MAX || !available || !gameObject.activeSelf) return;
        energy = Mathf.Clamp(energy + value, 0, MAX);
        _energyBar.UpdateStats(energy,MAX);
    }

    //使用（消耗）能量
    public void Use(int value)
    {
        energy -= value;
        _energyBar.UpdateStats(energy,MAX);
        if (energy == 0 && !available)
        {
            PlayerOverDrive.off.Invoke();
        }
    }

    //是否能够使用
    public bool IsEnough(int value) => energy >= value;
    
    
    private void PlayerOverDriveOn()
    {
        available = false;//禁止获取能量
        StartCoroutine(KeepUsingCoroutine());
    }

    private void PlayerOverDriveOff()
    {
        available = true;//允许获取能量
        StopCoroutine(KeepUsingCoroutine());
    }

    IEnumerator KeepUsingCoroutine()
    {
        while (gameObject.activeSelf && energy >0)
        {
            yield return waitForOverdriveInterval;
            Use(PERCENT);
        }
    }
}
