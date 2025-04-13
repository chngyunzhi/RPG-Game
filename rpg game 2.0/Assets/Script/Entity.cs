using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour     // for every object
{
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public CharacterStats stats { get; private set; }
    public CapsuleCollider2D capsule { get; private set; }

    #endregion

    [Header("Knockback Info")]
    [SerializeField] protected Vector2 knockBackPower;
    [SerializeField] protected float knockbackDuration;
    protected bool isKnocked;

    [Header("Collision Info")]
    public Transform attackCheck;
    public float attackCheckRadius;

    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask ground;

    public int knockBackDir {  get; private set; }
    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    public System.Action onFlipped;

    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        stats = GetComponentInChildren<CharacterStats>();
        sr = GetComponentInChildren<SpriteRenderer>();
        capsule = GetComponent<CapsuleCollider2D>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

    }

    protected virtual void Update()
    {

    }

    public virtual void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        
    }

    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1;
    }

    public virtual void DamageImpact() => StartCoroutine(HitKnockback());

    public virtual void SetupKnockBackDir(Transform _damageDirection)
    {
        if (_damageDirection.position.x > transform.position.x)  //damage come from right site       
            knockBackDir = -1;
        else if (_damageDirection.position.x < transform.position.x)
            knockBackDir = 1;
        

    }

    public void SetupKnockbackPower(Vector2 _knockbackPower) => knockBackPower = _knockbackPower;

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;

        rb.linearVelocity = new Vector2(knockBackPower.x * knockBackDir, knockBackPower.y);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;

        SetupZeroKnockbackPower();
    }
    protected virtual void SetupZeroKnockbackPower()
    {

    }

    #region Collision
    public virtual bool isGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, ground);
    public virtual bool isWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, ground);



    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);

    }

    #endregion

    #region Flip
    public void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        if(onFlipped != null)
            onFlipped();
    }


    public void FlipController(float _x)
    {
        if (_x > 0 && !facingRight) Flip();
        else if (_x < 0 && facingRight) Flip();
    }

    #endregion

    #region Velocity
    public void SetZeroVelocity()
    {
        if (isKnocked)  return;
        rb.linearVelocity = new Vector2(0, 0);
    }     
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked) return;
        rb.linearVelocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    #endregion


    public virtual void Die()
    {

    }

}
