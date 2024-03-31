using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LootItem : MonoBehaviour
{
    [SerializeField] float minSpeed = 5f;
    [SerializeField] float maxSpeed = 15f;
    [SerializeField] protected AudioData defaultPickUpSFX;
    int pickAnimID = Animator.StringToHash("PickUp");
    Animator _animator; 
    AudioData pickUpSFX;
    protected PlayerController player;
    protected Text lootMessage;
    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        _animator = GetComponent<Animator>();
        pickUpSFX = defaultPickUpSFX;
        lootMessage = GetComponentInChildren<Text>(true);
    }

    void OnEnable()
    {
        StartCoroutine(MoveCoroutine());
    }
    
    IEnumerator MoveCoroutine()
    {
        float speed = Random.Range(minSpeed, maxSpeed);
        Vector3 direction = Vector3.left;
        while (true)
        {
            if (player.isActiveAndEnabled)
            {
                direction = (player.transform.position - transform.position).normalized;
            }
            transform.Translate(direction * speed * Time.deltaTime);
            yield return null;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PickUp();
    }

    protected virtual void PickUp()
    {
        StopCoroutine(nameof(MoveCoroutine));//停止物体移动
        _animator.Play(pickAnimID);//播放拾取动画
        AudioManager.Instance.PlayRandomSFX(pickUpSFX);//播放拾取音效
    }
}
