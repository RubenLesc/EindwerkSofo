using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    private float cooldownTimer = Mathf.Infinity;
    // References
    private Animator anim;
    private Collider2D playerCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        player_Movement playerMovement = FindObjectOfType<player_Movement>();

        if (playerMovement != null)
        {
            playerCollider = playerMovement.PlayerCollider;
            if (playerCollider == null)
            {
                Debug.LogError("Player Collider is not set. Melee attack cannot proceed.");
            }
        }
        else
        {
            Debug.LogError("player_Movement script not found in the scene.");
        }
    }

    private void Update()
    {
        // Attack only when player in sight
        if (playerCollider != null && PlayerInSight())
        {
            cooldownTimer += Time.deltaTime;

            // Attack only when player in sight?
            if (PlayerInSight())
            {
                if (cooldownTimer >= attackCooldown)
                {
                    cooldownTimer = 0;
                    anim.SetTrigger("Attack");
                }
            }
        }
    }

    private bool PlayerInSight()
    {
        // Calculate the offset from the enemy's center to start the boxcast
        Vector2 offset = transform.right * range * colliderDistance;


        Vector2 startPosition = (Vector2)transform.position + offset - Vector2.up;

        // Perform a boxcast to detect the player
        RaycastHit2D hit = Physics2D.BoxCast(
           startPosition, // Start position of the boxcast (offset from enemy's center)
            new Vector2(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y), // Size of the boxcast (player's collider size)
            0f, // No rotation
            transform.right, // Cast direction (in the direction the enemy is facing)
            range, // Maximum distance of the cast
            LayerMask.GetMask("Player")); // Layer mask for player's collider

        // Check if player's collider is detected
        if (hit.collider != null && hit.collider == playerCollider)
        {
            Debug.Log("Player detected: " + hit.collider.gameObject.name);
            return true;
        }
        else
        {
            Debug.Log("Player not detected");
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        // Calculate the offset from the enemy's center to start the boxcast
        float flippedMultiplier = Mathf.Sign(transform.localScale.x); // Get the sign of the scale to handle flipping
        Vector2 offset = transform.right * range * colliderDistance * flippedMultiplier;

        // Calculate the starting position of the boxcast
        Vector2 startPosition = (Vector2)transform.position + offset;

        // Calculate the center of the red wireframe cube with additional downward offset
        Vector2 redCubeCenter = startPosition - Vector2.up * 1f; // Adjust the multiplier to move the red cube center further down

        // Draw the wireframe cube representing the boxcast area with the red cube's center at the same position as the blue sphere
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(redCubeCenter, new Vector3(boxCollider.size.x * range, boxCollider.size.y, 0f));
    }



    private void DamagePlayer()
    {
        if (playerCollider != null && PlayerInSight())
        {
            Healthclass playerHealth = playerCollider.GetComponent<Healthclass>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            else
            {
                Debug.LogWarning("Player's health component not found.");
            }
        }
        else
        {
            Debug.LogWarning("Player not in sight. Cannot damage.");
        }
    }
}
