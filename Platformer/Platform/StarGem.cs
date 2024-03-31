using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGem : MonoBehaviour
{
    [SerializeField]
    private float resetTime = 3.0f;

    [SerializeField] private AudioClip pickupSFX;
    [SerializeField] private ParticleSystem pickupVFX;

    private new Collider _collider;

    private MeshRenderer _meshRenderer;

    private WaitForSeconds _waitResetTime;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _waitResetTime = new WaitForSeconds(resetTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerController>(out PlayerController player))
        {
            player.canAirJump = true;
            SFXPlayer.audioSource.PlayOneShot(pickupSFX);//播放声效
            Instantiate(pickupVFX, transform.position, transform.rotation);
            //拾取到宝石之后允许玩家进行空中跳跃，同时关闭宝石碰撞体和渲染器
            _collider.enabled = false;
            _meshRenderer.enabled = false;
            //Invoke(nameof(Reset),resetTime);  Invoke函数实际上有性能上的问题，故使用协程替代
            StartCoroutine(ResetCoroutine());
        }
    }

    private void Reset()
    {
        _collider.enabled = true;
        _meshRenderer.enabled = true;
    }

    //通过协程实现延迟调用
    IEnumerator ResetCoroutine()
    {
        yield return _waitResetTime;
        Reset();
    }
}
