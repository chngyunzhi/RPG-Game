using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CrystalSkill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] GameObject crystalPrefab;
    private GameObject currentCrystal;

    [Header("Crystal mirage")]
    [SerializeField] private UI_SkillTreeSlot unlockCloneInsteadButton;
    [SerializeField] private bool cloneInsteadOfCrystal;

    [Header("Crystal simple")]
    [SerializeField] private UI_SkillTreeSlot unlockCrystalButton;
    public bool crystalUnlock {  get; private set; }

    [Header("Explosive Crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockExplosiveButton;
    [SerializeField] private bool canExplode;


    [Header("Moving Crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockMovingCrystalButton;
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi Stacking Crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockMultiStackCrystalButton;
    [SerializeField] private bool canUseMultiStack;
    [SerializeField] private float useTimeWindow; 
    [SerializeField] private int amountOfStack;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();

    protected override void Start()
    {
        base.Start();

        unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        unlockCloneInsteadButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMirage);
        unlockExplosiveButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalExplosive);
        unlockMovingCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMoving);
        unlockMultiStackCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMulti);
    }

    protected override void CheckUnlock()
    {

        UnlockCrystal();
        UnlockCrystalMirage();
        UnlockCrystalExplosive();
        UnlockCrystalMirage();
        UnlockCrystalMulti();
    }

    #region unlock skill region

    private void UnlockCrystal()
    {
        if (unlockCrystalButton.unlock)
            crystalUnlock = true;
    }

    private void UnlockCrystalMirage()
    {
        if(unlockCloneInsteadButton.unlock)
            cloneInsteadOfCrystal = true;
    }

    private void UnlockCrystalExplosive()
    {
        if(unlockExplosiveButton.unlock)
            canExplode = true;
    }

    private void UnlockCrystalMoving()
    {
        if(unlockMovingCrystalButton.unlock)
            canMoveToEnemy = true;
    }

    private void UnlockCrystalMulti()
    {
        if(unlockMultiStackCrystalButton.unlock)
            canUseMultiStack = true;
    }

    #endregion

    public override void UseSkill()
    {
        base.UseSkill();

        if(CanUseMultiCrystal())
            return;

        if (currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (canMoveToEnemy)
                return;

            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if (cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<CrystalSkiillController>()?.FinishCrystal();

            }
        }

    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        CrystalSkiillController currentCrystalScript = currentCrystal.GetComponent<CrystalSkiillController>();

        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform), player);
       // currentCrystalScript.ChooseRandomEnemy();
        
    }

    public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<CrystalSkiillController>().ChooseRandomEnemy();
    

    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStack)
        {
            if(crystalLeft.Count > 0)
            {

                if (crystalLeft.Count == amountOfStack)
                    Invoke("ResetAbility", useTimeWindow);
                
                cooldown = 0;
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];     //choose last one
                GameObject newCrystal = Instantiate(crystalToSpawn,player.transform.position, Quaternion.identity);
                
                crystalLeft.Remove(crystalToSpawn);

                newCrystal.GetComponent<CrystalSkiillController>().
                    SetupCrystal(crystalDuration,canExplode,canMoveToEnemy,moveSpeed,FindClosestEnemy(newCrystal.transform), player); 

                if(crystalLeft.Count <= 0)
                {
                    cooldown = multiStackCooldown;
                    RefillCrystal();

                }
            return true;
                    
            }

        }

        return false;
    }

    private void RefillCrystal()
    {
        int amountToAdd = amountOfStack - crystalLeft.Count;

        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);    
        }
    }

    private void ResetAbility()
    {
        if(cooldownTimer > 0)
            return;

        cooldownTimer = multiStackCooldown;
        RefillCrystal();
    }


}
