using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float dashForce = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    
    [Header("Attraction Settings")]
    public bool canBeAttracted = true;
    public float resistanceForce = 5f;
    
    [Header("Death Settings")]
    public Transform respawnPoint;
    
    private Rigidbody2D rb;
    private bool canDash = true;
    private bool isDashing = false;
    private Vector3 initialPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.freezeRotation = true;
        }
        initialPosition = transform.position;
    }
    
    void Update()
    {
        if (!isDashing)
        {
            float moveHorizontal = Input.GetAxis("Horizontal") * speed;
            float moveVertical = Input.GetAxis("Vertical") * speed;
            rb.linearVelocity = new Vector2(moveHorizontal, moveVertical);
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && canDash && !isDashing)
        {
            StartCoroutine(PerformDash());
        }
    }
    
    void FixedUpdate()
    {
        if (canBeAttracted && rb.linearVelocity.magnitude > 0.1f)
        {
            rb.AddForce(-rb.linearVelocity * resistanceForce * 0.1f);
        }
    }
    
    private IEnumerator PerformDash()
    {
        canDash = false;
        isDashing = true;
        
        Vector2 originalVelocity = rb.linearVelocity;
        Vector2 dashDirection = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
        ).normalized;
        
        if (dashDirection == Vector2.zero && originalVelocity != Vector2.zero)
        {
            dashDirection = originalVelocity.normalized;
        }
        else if (dashDirection == Vector2.zero)
        {
            dashDirection = Vector2.right;
        }
        
        rb.linearVelocity = dashDirection * dashForce;
        yield return new WaitForSeconds(dashDuration);
        
        isDashing = false;
        if (originalVelocity != Vector2.zero)
        {
            rb.linearVelocity = originalVelocity.normalized * speed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
        
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle") || other.CompareTag("Enemy"))
        {
            Die();
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Enemy"))
        {
            Die();
        }
    }
    
    private void Die()
    {
        Debug.Log("�Player dead!");

        DeathCounter.Instance?.IncrementDeaths();
        
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
        }
        else
        {
            transform.position = initialPosition;
        }
        
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}