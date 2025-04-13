using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{
    [Header("Dash")]
    [SerializeField] private UI_SkillTreeSlot dashUnlockButton;
    public bool dashUnlock {  get; private set; }

    [Header("Clone on Dash")]
    [SerializeField] private UI_SkillTreeSlot CloneOnDashUnlockButton;
    public bool cloneOnDashUnlock {  get; private set; }

    [Header("Clone on Arrival")]
    [SerializeField] private UI_SkillTreeSlot CloneOnArrivalUnlockButton;
    public bool cloneOnArrivalUnlock {  get; private set; }

    public override void UseSkill()
    {
        base.UseSkill();
    }

    protected override void Start()
    {

        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        CloneOnDashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        CloneOnArrivalUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);
    }

    protected override void CheckUnlock()
    {
        UnlockDash();
        UnlockCloneOnDash();
        UnlockCloneOnArrival();
    }

    private void UnlockDash()
    {
        Debug.Log("attempt to unlock dash");

        if (dashUnlockButton.unlock)
        {
            Debug.Log("dash unlock"); 
            dashUnlock = true;
        }
    }

    private void UnlockCloneOnDash()
    {
        if(CloneOnDashUnlockButton.unlock)
            cloneOnDashUnlock = true;
    }

    private void UnlockCloneOnArrival()
    {
        if(CloneOnArrivalUnlockButton.unlock)
            cloneOnArrivalUnlock = true;
    }

    public void CloneOnDash()
    {
        if (cloneOnDashUnlock)
           SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
    }

    public void CloneOnArrival()
    {
        if (cloneOnArrivalUnlock)
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
    }


}
