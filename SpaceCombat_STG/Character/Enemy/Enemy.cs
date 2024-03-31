using UnityEngine;

public class Enemy : Character
{
   [SerializeField] private int deathEnergyBonus = 3;
   [SerializeField] private int scorePoint = 100;
   [SerializeField] protected int healthFactor;

   LootSpawner _lootSpawner;

   protected virtual void Awake()
   {
      _lootSpawner = GetComponent<LootSpawner>();
   }

   protected override void OnEnable()
   {
      SetHealth();
      base.OnEnable();
   }

   public override void Die()
   {
      ScoreManager.Instance.AddScore(scorePoint);
      PlayerEnergy.Instance.Obtain(deathEnergyBonus);
      EnemyManager.Instance.RemoveFromList(gameObject);
      _lootSpawner.Spawn(transform.position);
      base.Die();
   }

   protected virtual void OnCollisionEnter2D(Collision2D col)
   {
      if (col.gameObject.TryGetComponent<PlayerController>(out PlayerController playerController))
      {
         playerController.Die();
         Die();
      }
   }

   protected virtual void SetHealth()
   {
      maxHealth += (int)(EnemyManager.Instance.WaveNumber/healthFactor);
   }
}
