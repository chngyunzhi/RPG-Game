using UnityEngine;
using UnityEngine.UI;

public class ParrySkill : Skill
{
    [Header("Parry")]
    [SerializeField] private UI_SkillTreeSlot parryUnlockButton;
    public bool parryUnlock {  get; private set; }

    [Header("Parry restore")]
    [SerializeField] private UI_SkillTreeSlot restoreUnlockButton;      // restore health
    [Range(0f, 1f)]
    [SerializeField] private float restoreHealthAmount;
    public bool restoreUnlock {  get; private set; }



    [Header("Parry With Mirage")]
    [SerializeField] private UI_SkillTreeSlot parryWithMirageUnlockButton;
    public bool parryWithMirageUnlock{  get; private set; }



    public override void UseSkill()
    {
        base.UseSkill();

        if (restoreUnlock)
        {
            int restoreAmount = Mathf.RoundToInt(player.stats.GetMaxHealthValue() * restoreHealthAmount);
            player.stats.IncreaseHealthBy(restoreAmount);
        }
    }

    protected override void Start()
    {
        base.Start();

        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        restoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestore);
        parryWithMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithMirage);
    }

    protected override void CheckUnlock()
    {
        UnlockParry();
        UnlockParryRestore();
        UnlockParryWithMirage();
    }

    private void UnlockParry()
    {
        if (parryUnlockButton.unlock)
            parryUnlock = true;

    }

    private void UnlockParryRestore()
    {
        if(restoreUnlockButton.unlock)
            restoreUnlock = true;
    }

    private void UnlockParryWithMirage()
    {
        if(parryWithMirageUnlockButton.unlock)
            parryWithMirageUnlock = true;
    }

    public void MakeMirageOnParry(Transform _respawnTransform)
    {
        if (parryWithMirageUnlock)
            SkillManager.instance.clone.CreateCloneWithDelay(_respawnTransform);
    }

}
