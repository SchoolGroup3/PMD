using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    
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

    [Header("Appearance Settings")]
    public Sprite playerSprite; 
    
    private Rigidbody2D rb;
    private bool canDash = true;
    private bool isDashing = false;
    private Vector3 initialPosition;
    private SpriteRenderer spriteRenderer;
    float moveHorizontal;
    float moveVertical;
    private Animator animator;

    void Awake()
    {
        // Delete duplicate players
        PlayerController[] existingPlayers = FindObjectsOfType<PlayerController>();
        animator= this.GetComponent<Animator>();
        foreach (PlayerController player in existingPlayers)
        {
            if (player != this) 
            {
                Destroy(player.gameObject);
            }
        }

        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.freezeRotation = true;
        }
        initialPosition = transform.position;

        // Configure the sprite
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && playerSprite != null)
        {
            spriteRenderer.sprite = playerSprite;
        }
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindRespawnPointInNewScene();
    }
    
    void FindRespawnPointInNewScene()
    {
        GameObject respawnObject = GameObject.FindGameObjectWithTag("Respawn");
        if (respawnObject != null)
        {
            respawnPoint = respawnObject.transform;
            transform.position = respawnPoint.position;
        }
    }
    
    void Update()
    {
        if (!isDashing)
        {
            moveHorizontal = Input.GetAxis("Horizontal") * speed;
            moveVertical = Input.GetAxis("Vertical") * speed;
            rb.linearVelocity = new Vector2(moveHorizontal, moveVertical);

        }
        if(moveHorizontal > 0) {
            animator.SetBool("Walk_right", true);
        }
        else
        {
            animator.SetBool("Walk_right", false);

        }
        if (moveHorizontal < 0)
        {
            animator.SetBool("Walk_left", true);
        }
        else
        {
            animator.SetBool("Walk_left", false);

        }
        if (moveVertical > 0)
        {
            animator.SetBool("Walk_up", true);
        }
        else
        {
            animator.SetBool("Walk_up", false);
        }
        if (moveVertical < 0)
        {
            animator.SetBool("Walk_down", true);
        }
        else
        {
            animator.SetBool("Walk_down", false);
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
        Debug.Log("Player died!");

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
    
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}