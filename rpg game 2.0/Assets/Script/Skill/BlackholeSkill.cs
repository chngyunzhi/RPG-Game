using UnityEngine;
using UnityEngine.UI;

public class BlackholeSkill : Skill
{

    [SerializeField] private UI_SkillTreeSlot blackholeUnlockButton;
    public bool blackholeUnlock {  get; private set; }
    [SerializeField] private int amountOfAttack;
    [SerializeField] private float cloneAttackCooldown;
    [SerializeField] private float blackholeDuration;
    [Space]
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;


    BlackholeSkillController currentBlackhole;


    private void UnlockBlackhole()
    {
        if (blackholeUnlockButton.unlock)
            blackholeUnlock = true;
    }

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackHole = Instantiate(blackholePrefab,player.transform.position,Quaternion.identity);

        currentBlackhole = newBlackHole.GetComponent<BlackholeSkillController>();

        currentBlackhole.SetupBlackHole(maxSize,growSpeed,shrinkSpeed,amountOfAttack,cloneAttackCooldown,blackholeDuration);
    }

    protected override void Start()
    {
        base.Start();

        blackholeUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBlackhole);
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool SkillCompleted()
    {
        if(!currentBlackhole) 
            return false;

        if (currentBlackhole.playerCanExitState)
        {
            currentBlackhole = null;
            return true;
        }

        return false;
    }

    protected override void CheckUnlock()
    {

        UnlockBlackhole();
    }

    public float GetBlackholeRadius()
    {
        return maxSize / 2;
    }

}
