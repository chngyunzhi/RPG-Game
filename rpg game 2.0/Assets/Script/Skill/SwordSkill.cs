using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}
public class SwordSkill : Skill
{
    public SwordType swordType = SwordType.Regular;

    [Header("Bounce Info")]
    [SerializeField] private UI_SkillTreeSlot bounceUnlockButton;
    [SerializeField] private int BounceAmount;
    [SerializeField] private float BounceGravity;

    [Header("Pierce Info")]
    [SerializeField] private UI_SkillTreeSlot pierceUnlockButton;
    [SerializeField] private int PierceAmount;
    [SerializeField] private float pierceGravity;

    [Header("Spin Info")]
    [SerializeField] private UI_SkillTreeSlot spinUnlockButton;
    [SerializeField] private float hitCooldown = .35f;
    [SerializeField] private float maxTravelDistance = 7;
    [SerializeField] private float spinDuration = 2;
    [SerializeField] private float spinGravity = 1;

    [Header("Skill Info")]
    [SerializeField] private UI_SkillTreeSlot swordUnlockButton;
    public bool swordUnlock { get; private set; }
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeTimeDuration;

    [Header("Passive Skill")]
    [SerializeField] private UI_SkillTreeSlot timeStopUnlockButton;
    public bool timeStopUnlock {  get; private set; }
    [SerializeField] private UI_SkillTreeSlot vulnerableUnlockButton;
    public bool vulnerableUnlock {  get; private set; }

    [Header("Aim Dots")]
    [SerializeField] private int numOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotsPrefab;
    [SerializeField] private Transform dotsParent;


    private GameObject[] dots;

    private Vector2 finalDir;

    protected override void Start()
    {
        base.Start();

        GenerateDots();

        swordUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSword);
        bounceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBounce);
        pierceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockPierce);
        spinUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSpin);
        timeStopUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockTimeStop);
        vulnerableUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockVulnerable);

    }

    private void SetupGravity()     // can use switch
    {
        if (swordType == SwordType.Bounce)
            swordGravity = BounceGravity;
        else if (swordType == SwordType.Pierce)
            swordGravity = pierceGravity;
        else if (swordType == SwordType.Spin)
            swordGravity = spinGravity;
    }

    protected override void Update()
    {
        SetupGravity();
        if (Input.GetKeyUp(KeyCode.Mouse1))
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);
        if (Input.GetKey(KeyCode.Mouse1))
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
    }
    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        swordSkillController newSwordScript = newSword.GetComponent<swordSkillController>();

        if (swordType == SwordType.Bounce)
            newSwordScript.SetupBounce(true, BounceAmount);
        else if (swordType == SwordType.Pierce)
            newSwordScript.SetupPierce(PierceAmount);
        else if (swordType == SwordType.Spin)
            newSwordScript.SetupSpin(true, maxTravelDistance, spinDuration,hitCooldown);



        newSwordScript.SetupSword(finalDir, swordGravity, player, freezeTimeDuration);

        player.AssignNewSword(newSword);

        DotsActive(false);
    }

    #region Unlock Region

    protected override void CheckUnlock()
    {
        UnlockSword();
        UnlockBounce();
        UnlockSpin();
        UnlockPierce();
        UnlockTimeStop();
        UnlockVulnerable();
    }

    private void UnlockTimeStop()
    {
        if (timeStopUnlockButton.unlock)
            timeStopUnlock = true;
    }

    private void UnlockVulnerable()
    {
        if(vulnerableUnlockButton.unlock)
            vulnerableUnlock = true;
    }

    private void UnlockSword()
    {
        if (swordUnlockButton.unlock)
        {
            swordType = SwordType.Regular;
            swordUnlock = true;
        }
    }

    private void UnlockBounce()
    {
        if(bounceUnlockButton.unlock)
            swordType = SwordType.Bounce;
    }

    private void UnlockPierce()
    {
        if(pierceUnlockButton.unlock)
            swordType = SwordType.Pierce;
    }

    private void UnlockSpin()
    {
        if(spinUnlockButton.unlock)
            swordType= SwordType.Spin;
    }


    #endregion

    #region Aim region
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        return direction;
    }

    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }
    private void GenerateDots()
    {
        dots = new GameObject[numOfDots];
        for (int i = 0; i < numOfDots; i++)
        {
            dots[i] = Instantiate(dotsPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }

    #endregion

}
