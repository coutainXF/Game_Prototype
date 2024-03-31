using UnityEngine;
public class WeaponPowerPickUp : LootItem
{
    [SerializeField] int fullPowerScoreBonus = 200;
    [SerializeField] AudioData fullPowerSFX;
    protected override void PickUp()
    {
        if (player.IsFullPower)
        {
            lootMessage.text = $"SCORE + {fullPowerScoreBonus}";
            ScoreManager.Instance.AddScore(fullPowerScoreBonus);
            AudioManager.Instance.PlayRandomSFX(fullPowerSFX);
        }
        else
        {
            lootMessage.text = $"Weapon Power UP!";
            player.WeaponPowerUp1Rank();
        }
        base.PickUp();
    }
    
    
    
}
