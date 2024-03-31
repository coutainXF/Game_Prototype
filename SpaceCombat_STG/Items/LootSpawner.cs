using UnityEngine;
public class LootSpawner : MonoBehaviour
{
    [SerializeField] LootSetting[] _lootSettings;
    public virtual void Spawn(Vector2 position)
    {
        foreach (var lootItem in _lootSettings)
        {
            lootItem.Spawn(position+Random.insideUnitCircle);
        }
    }
}
