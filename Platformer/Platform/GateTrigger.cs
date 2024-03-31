using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTrigger : MonoBehaviour
{
    [SerializeField]
    private AudioClip _audioClip;

    [SerializeField] 
    private ParticleSystem pickUPVFX;

    [SerializeField] 
    private VoidEventChannel gateEventChannel;

    //public event Action gateTrigger;
    
    private void OnTriggerEnter(Collider other)
    {
        //gateTrigger?.Invoke();
        gateEventChannel.Broadcast();
        SFXPlayer.audioSource.PlayOneShot(_audioClip);//播放音效
        Instantiate(pickUPVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
