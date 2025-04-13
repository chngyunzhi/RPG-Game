using UnityEngine;
using UnityEngine.LightTransport;

public class Checkpoint : MonoBehaviour
{
    private Animator anim;
    public string ID;
    public bool activationStatus;

    private void Start()
    {
        anim = GetComponent<Animator>();

    }

    [ContextMenu("Generate checkpoint ID")]
    private void GenerateID()
    {
        ID = System.Guid.NewGuid().ToString();        // generate ID each time you call
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {   
            ActivateCheckpoint();
        }
    }

    public void ActivateCheckpoint()
    {
        if (activationStatus != true)
             AudioManager.instance.PlaySFX(3,null);

        activationStatus = true;
        anim.SetBool("Active", true);
    }
}
