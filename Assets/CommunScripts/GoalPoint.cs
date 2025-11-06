using UnityEngine;

public class GoalPoint : MonoBehaviour
{
    [Header("Goal Settings")]
    public bool requiresAllCollectibles = true; // ← AÑADE esta línea
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (CanPassGoal())
            {
                LevelManager.Instance?.LoadNextLevel();
            }
            else
            {
                Debug.Log("¡You need to collect all the collectionables!");
            }
        }
    }
    
    // ← AÑADE este método nuevo
    private bool CanPassGoal()
    {
        if (!requiresAllCollectibles)
            return true;
            
        return LevelManager.Instance?.CanPassToNextLevel() ?? false;
    }
}