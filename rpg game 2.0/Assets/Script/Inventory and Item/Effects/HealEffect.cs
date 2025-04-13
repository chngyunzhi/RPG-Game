using Unity.VisualScripting;
using UnityEngine;



[CreateAssetMenu(fileName = "Heal Effect", menuName = "Data/Item Effect/Heal Effect")]
public class HealEffect : ItemEffect
{
    [Range(0f,1f)]
    [SerializeField] private float healPercent;

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        PlayerStats playerstats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        int healAmount = Mathf.RoundToInt(playerstats.GetMaxHealthValue() * healPercent);

        playerstats.IncreaseHealthBy(healAmount);
    }
}
