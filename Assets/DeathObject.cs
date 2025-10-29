using UnityEngine;

public class DeathObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            KillPlayer(other.gameObject);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            KillPlayer(collision.gameObject);
        }
    }
    
    private void KillPlayer(GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.SendMessage("Die", SendMessageOptions.DontRequireReceiver);
        }
    }
}