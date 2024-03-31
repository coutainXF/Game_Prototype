using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickUp : LootItem
{
    [SerializeField] int fullHealthScoreBonus = 200;
    [SerializeField] float shieldBonus = 20f;
    [SerializeField] AudioData fullHealthSFX;
    protected override void PickUp()
    {
        if (player.IsFullhealth)
        {
            lootMessage.text = $"SCORE + {fullHealthScoreBonus}";
            ScoreManager.Instance.AddScore(fullHealthScoreBonus);
            AudioManager.Instance.PlayRandomSFX(fullHealthSFX);
        }
        else
        {
            lootMessage.text = $"SHIELD + {shieldBonus}";
            player.RestoreHealth(shieldBonus);
        }
        base.PickUp();
    }
}
