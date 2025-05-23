using System.Collections.Generic;
using UnityEngine;

public class swordSkillController : MonoBehaviour
{
    [SerializeField] private float returnSpeed = 12;
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;


    private float freezeTimeDuration;

    [Header("Pierce Info")]
    [SerializeField] private float pierceAmount;


    [Header("Bounce Info")]
    [SerializeField] private float bounceSpeed;
    private bool isBouncing = true;
    private int BounceAmount;
    public List<Transform> enemyTarget;
    private int targetIndex;

    [Header("Spin Info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;

    private float hitTimer;
    private float hitCooldown;

    private float spinDirection;
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    public void SetupSword(Vector2 _direction, float _gravityScale, Player _player, float _freezeTimeDuration)
    {
        player = _player;
        freezeTimeDuration = _freezeTimeDuration;

        rb.linearVelocity = _direction;
        rb.gravityScale = _gravityScale;
        if(pierceAmount <=0)
        anim.SetBool("Rotation", true);

        spinDirection = Mathf.Clamp(rb.linearVelocity.x, -1, 1);
    }

    public void SetupBounce(bool _isBouncing,int _amountOfBounces)
    {
        isBouncing = _isBouncing;
        BounceAmount = _amountOfBounces;

        enemyTarget = new List<Transform>();
    }

    public void SetupPierce(int _PierceAmount)
    {
        pierceAmount = _PierceAmount;
    }

    public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturning = true;
    }


    private void Update()
    {
        if (canRotate)
            transform.right = rb.linearVelocity;

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1)
                player.CatchTheSword();
        }
        BounceLogic();
        SpinLogic();
    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }
            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                // transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);
                if (spinTimer <= 0)                    
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;

                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                            SwordSkillDamage(hit.GetComponent<Enemy>());

                    }
                }
            }
        }
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .5f)
            {

                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());

                targetIndex++;
                BounceAmount--;

                if (BounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTarget.Count)
                    targetIndex = 0;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return;


        if(collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            SwordSkillDamage(enemy);

        }

        SetupTargetForBounce(collision);
        StuckInto(collision);
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();

        player.stats.DoDamage(enemyStats);

        if(player.skill.sword.timeStopUnlock)
            enemy.FreezeTimeFor(freezeTimeDuration);

        if (player.skill.sword.vulnerableUnlock)
            enemyStats.MakeVulnerableFor(freezeTimeDuration + 2);

        ItemDataEquipment equippedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);

        if (equippedAmulet != null)
            equippedAmulet.Effect(enemy.transform);
    }

    private void SetupTargetForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                        enemyTarget.Add(hit.transform);
                }

            }
        }
    }

    private void StuckInto(Collider2D collision)
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }

        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }
        if (isBouncing && enemyTarget.Count > 0)
            return;
            


        canRotate = false;
        circleCollider.enabled = false;
        //rb.isKinematic = true;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        GetComponentInChildren<ParticleSystem>().Play();


        anim.SetBool("Rotation", false);
        transform.SetParent(collision.transform);
        //transform.parent = collision.transform;
    }
}
