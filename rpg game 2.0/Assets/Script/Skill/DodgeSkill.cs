using UnityEngine;
using UnityEngine.UI;

public class DodgeSkill : Skill
{
    [Header("Dodge")]
    [SerializeField] private UI_SkillTreeSlot unlockDodgeButton;
    [SerializeField] private int evasionAmount;
    public bool dodgeUnlock;

    [Header("Mirage Dodge")]
    [SerializeField] private UI_SkillTreeSlot unlockMirageDodgeButton;
    public bool MirageDodgeUnlock;


    protected override void Start()
    {
        base.Start();

        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMirageDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockMirageDodge);
    }

    protected override void CheckUnlock()
    {
        UnlockDodge();
        UnlockMirageDodge();
    }

    private void UnlockDodge()
    {
        if (unlockDodgeButton.unlock && !dodgeUnlock)
        {
            player.stats.evasion.AddModifier(evasionAmount);
            Inventory.instance.UpdateStatsUI();
            dodgeUnlock = true;
        }
    }

    private void UnlockMirageDodge()
    {
        if(unlockMirageDodgeButton.unlock)
            MirageDodgeUnlock = true;
    }

    public void CreateMirageOnDodge()
    {
        if (MirageDodgeUnlock)
            SkillManager.instance.clone.CreateClone(player.transform,new Vector3(2 * player.facingDir,0));
    }
}
