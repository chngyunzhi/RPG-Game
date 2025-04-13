using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    private ItemDrop myDropSystem;
    public Stat soulDropAmount;

    [Header("Level Detial")]
    [SerializeField] private int level = 1;

    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier = .4f;

    protected override void Start()
    {
        soulDropAmount.SetDefaultValue(100);
        
        ApplyLevelModifier();

        base.Start();


        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();

    }

    private void ApplyLevelModifier()
    {
        Modify(strength);
        Modify(vitality);
        Modify(intelligence);
        Modify(agility);

        Modify(damage);
        Modify(critChance);
        Modify(critPower);

        Modify(health);
        Modify(armor);
        Modify(evasion);
        Modify(magicRes);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightningDamage);


        Modify(soulDropAmount);
    }

    private void Modify(Stat _stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = _stat.GetValue() * percentageModifier;

            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();

        PlayerManager.instance.currency += soulDropAmount.GetValue();
        myDropSystem.GenerateDrop();

        Destroy(gameObject, .5f);   
    }
}
