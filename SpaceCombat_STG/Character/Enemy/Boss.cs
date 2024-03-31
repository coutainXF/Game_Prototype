using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] BossHealthBar _healthBar; 
    Canvas healthBarCanvas;

    protected override void Awake()
    {
        base.Awake();
        _healthBar = FindObjectOfType<BossHealthBar>();
        healthBarCanvas = _healthBar.GetComponentInChildren<Canvas>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        _healthBar.Initialize(health,maxHealth);
        healthBarCanvas.enabled = true;
    }

    protected override void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.TryGetComponent<PlayerController>(out PlayerController playerController))
        {
            playerController.Die();
        }
    }

    public override void Die()
    {
        healthBarCanvas.enabled = false;
        base.Die();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        _healthBar.UpdateStats(health,maxHealth);
    }

    protected override void SetHealth()
    {
        maxHealth += (int)(EnemyManager.Instance.WaveNumber * healthFactor);
    }
}
