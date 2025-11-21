using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    [Header("UI Buttons - Assign in Inspector")]
    public Button rightButton;
    public Button leftButton;
    public Button upButton;
    public Button downButton;
    public Button dashButton;

    private Rigidbody2D rb;
    private bool canDash = true;
    private bool isDashing = false;
    private Vector3 initialPosition;
    private SpriteRenderer spriteRenderer;
    float moveHorizontal;
    float moveVertical;
    private Animator animator;
    [SerializeField] private AudioClip andarSFX;
    [SerializeField] private AudioClip MorirSFX;

    // Variables para control de botones
    private bool buttonRightPressed = false;
    private bool buttonLeftPressed = false;
    private bool buttonUpPressed = false;
    private bool buttonDownPressed = false;

    void Awake()
    {
        // Delete duplicate players
        PlayerController[] existingPlayers = FindObjectsOfType<PlayerController>();
        animator = this.GetComponent<Animator>();
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

    void Start()
    {
        SetupButtonListeners();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindRespawnPointInNewScene();
        // Re-conectar los botones después de cargar una nueva escena
        SetupButtonListeners();
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

    void SetupButtonListeners()
    {
        // Limpiar listeners existentes
        if (rightButton != null)
        {
            rightButton.onClick.RemoveAllListeners();
            // Agregar EventTrigger para detección de mantener presionado
            AddEventTrigger(rightButton, OnRightButtonPressed, EventTriggerType.PointerDown);
            AddEventTrigger(rightButton, OnRightButtonReleased, EventTriggerType.PointerUp);
            AddEventTrigger(rightButton, OnRightButtonReleased, EventTriggerType.PointerExit);
        }
        if (leftButton != null)
        {
            leftButton.onClick.RemoveAllListeners();
            AddEventTrigger(leftButton, OnLeftButtonPressed, EventTriggerType.PointerDown);
            AddEventTrigger(leftButton, OnLeftButtonReleased, EventTriggerType.PointerUp);
            AddEventTrigger(leftButton, OnLeftButtonReleased, EventTriggerType.PointerExit);
        }
        if (upButton != null)
        {
            upButton.onClick.RemoveAllListeners();
            AddEventTrigger(upButton, OnUpButtonPressed, EventTriggerType.PointerDown);
            AddEventTrigger(upButton, OnUpButtonReleased, EventTriggerType.PointerUp);
            AddEventTrigger(upButton, OnUpButtonReleased, EventTriggerType.PointerExit);
        }
        if (downButton != null)
        {
            downButton.onClick.RemoveAllListeners();
            AddEventTrigger(downButton, OnDownButtonPressed, EventTriggerType.PointerDown);
            AddEventTrigger(downButton, OnDownButtonReleased, EventTriggerType.PointerUp);
            AddEventTrigger(downButton, OnDownButtonReleased, EventTriggerType.PointerExit);
        }
        if (dashButton != null)
        {
            dashButton.onClick.RemoveAllListeners();
            dashButton.onClick.AddListener(OnDashButtonPressed);
        }
    }

    void AddEventTrigger(Button button, System.Action action, EventTriggerType eventType)
    {
        // Obtener o agregar el componente EventTrigger
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = button.gameObject.AddComponent<EventTrigger>();
        }

        // Crear una nueva entrada para el evento
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback.AddListener((eventData) => { action(); });

        // Agregar la entrada al trigger
        trigger.triggers.Add(entry);
    }

    void Update()
    {
        HandleMovement();
        HandleAnimations();

        if (Input.GetKeyDown(KeyCode.Space) && canDash && !isDashing)
        {
            StartCoroutine(PerformDash());
        }
    }

    void HandleMovement()
    {
        if (!isDashing)
        {
            // Obtener input del teclado
            float keyboardHorizontal = Input.GetAxis("Horizontal");
            float keyboardVertical = Input.GetAxis("Vertical");

            // Inicializar con input de teclado
            float finalHorizontal = keyboardHorizontal;
            float finalVertical = keyboardVertical;

            // SOBRESCRIBIR completamente con input de botones si están presionados
            if (buttonRightPressed || buttonLeftPressed || buttonUpPressed || buttonDownPressed)
            {
                finalHorizontal = 0f;
                finalVertical = 0f;

                if (buttonRightPressed) finalHorizontal = 1f;
                if (buttonLeftPressed) finalHorizontal = -1f;
                if (buttonUpPressed) finalVertical = 1f;
                if (buttonDownPressed) finalVertical = -1f;
            }

            moveHorizontal = finalHorizontal * speed;
            moveVertical = finalVertical * speed;
            rb.linearVelocity = new Vector2(moveHorizontal, moveVertical);
        }
    }

    void HandleAnimations()
    {
        // Resetear todas las animaciones primero
        animator.SetBool("Walk_right", false);
        animator.SetBool("Walk_left", false);
        animator.SetBool("Walk_up", false);
        animator.SetBool("Walk_down", false);

        // Determinar si hay movimiento
        bool isMoving = Mathf.Abs(moveHorizontal) > 0.1f || Mathf.Abs(moveVertical) > 0.1f;

        if (isMoving)
        {
            ControladorSFX.instance.EjecutarSonido(andarSFX);

            // Priorizar dirección horizontal
            if (Mathf.Abs(moveHorizontal) > Mathf.Abs(moveVertical))
            {
                if (moveHorizontal > 0)
                    animator.SetBool("Walk_right", true);
                else
                    animator.SetBool("Walk_left", true);
            }
            else
            {
                if (moveVertical > 0)
                    animator.SetBool("Walk_up", true);
                else
                    animator.SetBool("Walk_down", true);
            }
        }
        else
        {
            ControladorSFX.instance.PararSonido();
        }
    }

    // ========== MÉTODOS PARA BOTONES ==========

    public void OnRightButtonPressed()
    {
        buttonRightPressed = true;
    }

    public void OnRightButtonReleased()
    {
        buttonRightPressed = false;
    }

    public void OnLeftButtonPressed()
    {
        buttonLeftPressed = true;
    }

    public void OnLeftButtonReleased()
    {
        buttonLeftPressed = false;
    }

    public void OnUpButtonPressed()
    {
        buttonUpPressed = true;
    }

    public void OnUpButtonReleased()
    {
        buttonUpPressed = false;
    }

    public void OnDownButtonPressed()
    {
        buttonDownPressed = true;
    }

    public void OnDownButtonReleased()
    {
        buttonDownPressed = false;
    }

    public void OnDashButtonPressed()
    {
        if (canDash && !isDashing)
        {
            StartCoroutine(PerformDash());
        }
    }

    // Métodos alternativos para mantener compatibilidad
    public void Der() => OnRightButtonPressed();
    public void StopDer() => OnRightButtonReleased();
    public void Izq() => OnLeftButtonPressed();
    public void StopIzq() => OnLeftButtonReleased();
    public void Arb() => OnUpButtonPressed();
    public void StopArb() => OnUpButtonReleased();
    public void Bajar() => OnDownButtonPressed();
    public void StopBajar() => OnDownButtonReleased();
    public void dash() => OnDashButtonPressed();

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
        Vector2 dashDirection = Vector2.zero;

        // Determinar dirección del dash basado en botones presionados
        if (buttonRightPressed) dashDirection.x = 1f;
        if (buttonLeftPressed) dashDirection.x = -1f;
        if (buttonUpPressed) dashDirection.y = 1f;
        if (buttonDownPressed) dashDirection.y = -1f;

        // Si no hay botones presionados, usar input del teclado
        if (dashDirection == Vector2.zero)
        {
            dashDirection = new Vector2(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical")
            ).normalized;
        }

        if (dashDirection == Vector2.zero && originalVelocity != Vector2.zero)
        {
            dashDirection = originalVelocity.normalized;
        }
        else if (dashDirection == Vector2.zero)
        {
            dashDirection = Vector2.right;
        }
        else
        {
            dashDirection = dashDirection.normalized;
        }

        rb.linearVelocity = dashDirection * dashForce;
        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        // Restaurar el movimiento basado en los botones actualmente presionados
        HandleMovement();

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
        ControladorSFX.instance.EjecutarSonido(MorirSFX);
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

        // Resetear estados de botones al morir
        buttonRightPressed = false;
        buttonLeftPressed = false;
        buttonUpPressed = false;
        buttonDownPressed = false;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}