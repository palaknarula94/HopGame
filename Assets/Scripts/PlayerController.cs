using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float upwardForce = 7f;      // Force applied when jumping
    [SerializeField] private float forwardSpeed = 4.6f;      // Forward movement speed
    [SerializeField] private float gravityMultiplier = 2f; // Factor to increase gravity

    [Header("Drag Settings")]
    [SerializeField] private float dragSpeed = 2.5f;       // Speed of left/right drag
    [SerializeField] private float minX = -4f;            // Minimum X position
    [SerializeField] private float maxX = 4f;             // Maximum X position

    private Rigidbody rb;              // Reference to the Rigidbody
    private bool isGrounded = true;    // Grounded check
    private Vector3 dragStartPos;      // Drag start position
    private Vector3 ballStartPos;      // Ball's start position during drag
    private GameManager gameManager;   // Reference to the GameManager

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene.");
        }
    }

    private void FixedUpdate()
    {
        if (gameManager == null || gameManager.isGameOver)
            return;

        ApplyForwardMovement();
        ApplyEnhancedGravity();
        HandleDragMovement();

        // Check for falling below platforms
        if (transform.position.y < -5f)
        {
            gameManager.GameOver();
        }
    }

    /// <summary>
    /// Applies constant forward movement to the player.
    /// </summary>
    private void ApplyForwardMovement()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, forwardSpeed);
    }

    /// <summary>
    /// Increases the gravity effect when the player is falling.
    /// </summary>
    private void ApplyEnhancedGravity()
    {
        if (!isGrounded && rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (gravityMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    /// <summary>
    /// Handles left/right drag movement using touch or mouse input.
    /// </summary>
    private void HandleDragMovement()
    {
        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            Vector3 inputPosition = Input.GetMouseButton(0) 
                ? Input.mousePosition 
                : (Vector3)Input.GetTouch(0).position;

            TouchPhase touchPhase = Input.GetMouseButton(0) 
                ? TouchPhase.Moved 
                : Input.GetTouch(0).phase;

            if (touchPhase == TouchPhase.Began)
            {
                dragStartPos = Camera.main.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, 10f));
                ballStartPos = transform.position;
            }
            else if (touchPhase == TouchPhase.Moved)
            {
                Vector3 currentDragPos = Camera.main.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, 10f));
                float dragDeltaX = currentDragPos.x - dragStartPos.x;

                Vector3 newPosition = ballStartPos + new Vector3(dragDeltaX * dragSpeed, 0, 0);
                newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
                newPosition.y = transform.position.y;
                newPosition.z = transform.position.z;

                transform.position = newPosition;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;

            // Reset Y velocity and apply upward force
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * upwardForce, ForceMode.VelocityChange);

            gameManager?.IncreaseScore();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = false;
        }
    }
}
