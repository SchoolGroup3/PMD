using UnityEngine;

public class GoalPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LevelManager.Instance?.LoadNextLevel();
        }
    }
}