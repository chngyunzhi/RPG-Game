using UnityEngine;

public class Deadzone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CharacterStats>() != null)
            collision.GetComponent<CharacterStats>().KillEntity();
        else
            Destroy(collision.gameObject);

    }
}
