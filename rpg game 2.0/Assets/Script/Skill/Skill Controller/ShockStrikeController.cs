using UnityEngine;

public class ShockStrikeController : MonoBehaviour
{
    [SerializeField] private CharacterStats targetStats;
    [SerializeField] private float speed;
    private int damage;


    private Animator anim;
    private bool triggered;



    public void Setup(int _damage, CharacterStats _targetStats)
    {
        damage = _damage;
        targetStats = _targetStats;
    }
    void Start()
    {
        anim = GetComponentInChildren<Animator>();    
    }

    void Update()
    {
        if(!targetStats)
            return;

        if (triggered)
            return;

        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - targetStats.transform.position;
        if (Vector2.Distance(transform.position, targetStats.transform.position) < .1f)
        {
            anim.transform.localRotation = Quaternion.identity;
            transform.localRotation = Quaternion.identity;


            transform.localScale = new Vector3(2, 2);
            anim.transform.localPosition = new Vector3(0, .5f);

            Invoke("DamageSelfDestroy", .2f);
            triggered = true;
            anim.SetTrigger("Hit");
        }   
    }

    private void DamageSelfDestroy()
    {
        targetStats.ApplyShock(true);
        targetStats.TakeDamage(damage);
        Destroy(gameObject,.4f);

    }
}
