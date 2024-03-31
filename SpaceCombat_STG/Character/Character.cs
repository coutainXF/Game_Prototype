using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("-----生命值系统-----")]
    [SerializeField] protected float maxHealth;//最大生命值
    [SerializeField] protected GameObject deathVFX;//死亡特效
    [SerializeField] AudioData deathSFX;//死亡音效
    
    [SerializeField] bool showOnHeadHealthBar = true;//显示血条的开关
    [SerializeField] StatsBar onHeadHealthBar;//头顶血条
    
    protected float health;//当前生命值
    
    protected virtual void OnEnable()
    {
        health = maxHealth;

        if (showOnHeadHealthBar)
        {
            ShowOnHeadHealthBar();
        }
        else
        {
            HideOnHeadHealthBar();
        }
    }

    public void ShowOnHeadHealthBar()
    {
        onHeadHealthBar.gameObject.SetActive(true);
        onHeadHealthBar.Initialize(health,maxHealth);
    }

    public void HideOnHeadHealthBar()
    {
        onHeadHealthBar.gameObject.SetActive(false);
    }
    
    #region 生命值相关/受伤、恢复、死亡等
    //受伤
    public virtual void TakeDamage(float damage)
    {
        if(health == 0) return;
        
        health -= damage;
        
        if (showOnHeadHealthBar)
        {
            onHeadHealthBar.UpdateStats(health,maxHealth);
        }
        
        if (health <= 0f)
        {
            Die();
        }
    }
    //死亡
    public virtual void Die()
    {
        health = 0f;
        AudioManager.Instance.PlayRandomSFX(deathSFX);
        PoolManager.Release(deathVFX, transform.position);
        gameObject.SetActive(false);
    }
    //生命值恢复
    public virtual void RestoreHealth(float value)
    {
        if (health == maxHealth) return;
        // health += value;
        // health = Mathf.Clamp(health, 0f, maxHealth);
        //限定health的范围
        health = Mathf.Clamp(health + value, 0f, maxHealth);
        
        if (showOnHeadHealthBar)
        {
            onHeadHealthBar.UpdateStats(health,maxHealth);
        }
    }
    //持续恢复
    protected IEnumerator HealthRegenerateCoroutine(WaitForSeconds waitTime,float precent)
    {
        while (health < maxHealth)
        {
            yield return waitTime;
            
            RestoreHealth(maxHealth * precent);
        }
    }
    //持续受伤
    protected IEnumerator DamageOverTimeCoroutine(WaitForSeconds waitTime,float precent)
    {
        while (health > 0f)
        {
            yield return waitTime;
            
            RestoreHealth(maxHealth * precent);
        }
    }
    #endregion
    
}
