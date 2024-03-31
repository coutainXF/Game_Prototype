using System.Collections;
using UnityEngine;

public class PlayerMissile : PlayerProjectileOverdrive
{
    
    [SerializeField] AudioData targetAcquiredVoice = null;
    [SerializeField] float lowSpeed = 8f;
    [SerializeField] float highSpeed = 25f;
    [SerializeField] float variableSpeedDelay = .5f;
    
    [Header("Explosion")]
    [SerializeField] GameObject explosionVFX;//爆炸特效
    [SerializeField] AudioData explosionSFX;//爆炸音效
    [SerializeField] float explosionRadius = 3f;//爆炸半径
    [SerializeField] LayerMask enemyLayerMask;//指定敌人层
    [SerializeField] int explosionDamage = 100;//爆炸伤害
    WaitForSeconds waitVariableSpeedDelay;

    protected override void Awake()
    {
        base.Awake();
        waitVariableSpeedDelay = new WaitForSeconds(variableSpeedDelay);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(VariableSpeedCoroutine());
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        //1、爆炸（视觉特效
        PoolManager.Release(explosionVFX, transform.position);
        //2、爆炸音效
        AudioManager.Instance.PlayRandomSFX(explosionSFX);
        //3、范围伤害
        //实现方式：1、添加一个trigger，通过ontriggerEnter2D来检查
        //2、通过计算距离检查，通过自定义函数进行进行检测
        //3、通过OverlapCircleAll物理重叠检测函数
        var colliders = Physics2D.OverlapCircleAll(transform.position,explosionRadius,enemyLayerMask);

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(explosionDamage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position,explosionRadius);
    }

    //变速协程
    IEnumerator VariableSpeedCoroutine()
    {
        moveSpeed = lowSpeed;
        yield return waitVariableSpeedDelay;
        moveSpeed = highSpeed;
        if (target != null)
        {
            AudioManager.Instance.PlaySFX(targetAcquiredVoice);
        }
    }
}
