using UnityEngine;

public class AttractorObject : MonoBehaviour
{
    [Header("Attraction Settings")]
    public float attractionForce = 50f; // Attraction force
    public float attractionRadius = 3f; // RADIO ADJUSTABLE 
    public bool constantAttraction = true;
    public bool showDebug = true;
    
    private void Update()
    {
        if (constantAttraction)
        {
            ApplyAttraction();
        }
    }
    
    private void ApplyAttraction()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);

            
            if (distance < attractionRadius) 
            {
                Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    
                    // With the distance the force decrease
                    float forceMultiplier = 1f - (distance / attractionRadius);
                    Vector2 direction = (transform.position - player.transform.position).normalized;
                    playerRb.AddForce(direction * attractionForce * forceMultiplier);
                }
            }
        }
    }
    
    // Draw the radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attractionRadius);
    }
}