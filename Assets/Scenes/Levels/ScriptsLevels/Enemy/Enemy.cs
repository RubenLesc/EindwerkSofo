using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MeleeEnemy : MonoBehaviour
{
    // Attack Parameters
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range; 

    // Collider Parameters
    [SerializeField] private float colliderDistance; 
    [SerializeField] private BoxCollider2D boxCollider;

    [SerializeField] private int damage;

    // Timer for attack cooldown
    private float cooldownTimer = Mathf.Infinity;

    [SerializeField] private Animator anim;
    private Collider2D playerCollider; 
    private EnemyHealth enemyHealth;

    // ´kan aanvallen
    private bool canattack = true;

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void Start()
    {   //vind object
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

        cooldownTimer += Time.deltaTime;

        // kijkt of de speler in de visionbox is
        if (PlayerInSight())
        {
            // collider mag niet NULL zijn
            if (playerCollider != null)
            {
                if (canattack == true)
                {
                    //kijkt of cooldown is actief
                    if (cooldownTimer >= attackCooldown)
                    {
                        Debug.Log("Attack player");
                        anim.SetTrigger("Attack");
                        // Reset cooldown timer
                        cooldownTimer = 0f;
                    }
                    else
                    {
                        Debug.Log("Timer is still waiting");
                    }
                }
                Debug.Log("Attack is not finished");
            }
            else
            {
                Debug.Log("Enemy can't see you");
            }
        }
    }

    //animation bij einde 
    void AttackEnd()
    {
        canattack = false;
        StartCoroutine(WaitForSeconds(2f));
        canattack = true;
    }

    //wacht voor seconden
    private IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    // checkt of speler in de visionbox zit van de enemy
    private bool PlayerInSight()
    {
        // Calculate offset based on range and collider distance
        Vector2 offset = transform.right * range * colliderDistance;
        Vector2 startPosition = (Vector2)transform.position + offset - Vector2.up;

        // Cast a box in the direction of the player
        RaycastHit2D hit = Physics2D.BoxCast(
            startPosition,
            new Vector2(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y),
            0f,
            transform.right,
            range,
            LayerMask.GetMask("Player"));

        return hit.collider != null && hit.collider == playerCollider;
    }

    // damage speler
    private void DamagePlayer()
    {
        if (playerCollider != null && PlayerInSight())
        {
            // damage speler
            Healthclass playerHealth = playerCollider.GetComponent<Healthclass>();
            if (playerHealth != null)
            {
                Debug.Log("Hit enemy " + damage + " Damage");
                playerHealth.TakeDamage(damage);
            }
        }
        else
        {
            Debug.LogWarning("Player not in sight. Cannot damage.");
        }
    }

    // Visualize attack range in scene view
    private void OnDrawGizmos()
    {
        // Calculate Gizmos parameters
        float flippedMultiplier = Mathf.Sign(transform.localScale.x);
        Vector2 offset = transform.right * range * colliderDistance * flippedMultiplier;
        Vector2 startPosition = (Vector2)transform.position + offset;
        Vector2 redCubeCenter = startPosition - Vector2.up * 1f;

        // Draw wire cube to represent attack range
        Gizmos.color = Color.red;
        Vector3 playerScale = transform.localScale;
        Vector2 adjustedPosition = startPosition + new Vector2(range * transform.right.x * playerScale.x, 0f);
        Vector3 adjustedSize = new Vector3(range * boxCollider.bounds.size.x * Mathf.Abs(playerScale.x), boxCollider.bounds.size.y, 0f);
        Gizmos.DrawWireCube(adjustedPosition, adjustedSize);
    }
}
