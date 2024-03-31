using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitoryGem : MonoBehaviour
{
    [SerializeField] private VoidEventChannel victoryEventChannel;
    
    [SerializeField] private AudioClip audioClip;

    [SerializeField] private ParticleSystem pickUpvfx;
    private void OnTriggerEnter(Collider other)
    {
        victoryEventChannel.Broadcast();
        SFXPlayer.audioSource.PlayOneShot(audioClip);//播放音效
        Instantiate(pickUpvfx, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
