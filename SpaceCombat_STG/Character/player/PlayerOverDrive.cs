using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerOverDrive : MonoBehaviour
{
    [SerializeField] private GameObject triggerVFX;
    [SerializeField] private GameObject engineVFXNormal;
    [SerializeField] private GameObject engineVFXOverDrive;
    [SerializeField] private AudioData onSFX;
    [SerializeField] private AudioData offSFX;
    
    //控制开关
    public static UnityAction on = delegate {  };   //希望被外部调用，且减少耦合
    public static UnityAction off = delegate {  };
    
    private void Awake()
    {
        on += On;
        off += Off;
    }

    private void OnDestroy()
    {
        on -= On;
        off -= Off;
    }
    
    private void On()
    {
        triggerVFX.SetActive(true);
        engineVFXNormal.SetActive(false);
        engineVFXOverDrive.SetActive(true);
        AudioManager.Instance.PlayRandomSFX(onSFX);
    }
    
    private void Off()
    {
        engineVFXNormal.SetActive(true);
        engineVFXOverDrive.SetActive(false);
        AudioManager.Instance.PlayRandomSFX(offSFX);
    }
}
