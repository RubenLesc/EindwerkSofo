using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [SerializeField] private int damage;

    private float cooldownTimer = Mathf.Infinity;
    [SerializeField] private Animator anim;
    private Collider2D playerCollider;
    private EnemyHealth enemyHealth;
    
    private bool canattack = true;
    

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();  
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
        {
            // Increment the cooldown timer by the time elapsed since the last frame
            cooldownTimer += Time.deltaTime;

            // Check if the player is in sight
            if (PlayerInSight())
            {
                // Check if the player collider is not null
                if (playerCollider != null)
                {
                    if (canattack == true)
                    {
                        // Check if the cooldown timer has reached or exceeded the attack cooldown
                        if (cooldownTimer >= attackCooldown)
                        {
                            Debug.Log("Attack player");
                            anim.SetTrigger("Attack");
                            // Reset the cooldown timer for the next cooldown cycle
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
    }
    void AttackEnd()
    {   
        canattack = false;
        StartCoroutine(WaitForSeconds(2f));
        canattack = true;
    }
    private IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    private bool PlayerInSight()
    {
        Vector2 offset = transform.right * range * colliderDistance;
        Vector2 startPosition = (Vector2)transform.position + offset - Vector2.up;

        RaycastHit2D hit = Physics2D.BoxCast(
            startPosition,
            new Vector2(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y),
            0f,
            transform.right,
            range,
            LayerMask.GetMask("Player"));

        return hit.collider != null && hit.collider == playerCollider;
    }

    private void DamagePlayer()
    {
        if (playerCollider != null && PlayerInSight())
        {
            // Deal damage to player
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


    private void OnDrawGizmos()
    {   //visuals for debugging
        float flippedMultiplier = Mathf.Sign(transform.localScale.x);
        Vector2 offset = transform.right * range * colliderDistance * flippedMultiplier;
        Vector2 startPosition = (Vector2)transform.position + offset;
        Vector2 redCubeCenter = startPosition - Vector2.up * 1f;

        Gizmos.color = Color.red;
        Vector3 playerScale = transform.localScale;
        Vector2 adjustedPosition = startPosition + new Vector2(range * transform.right.x * playerScale.x, 0f);
        Vector3 adjustedSize = new Vector3(range * boxCollider.bounds.size.x * Mathf.Abs(playerScale.x),boxCollider.bounds.size.y,0f);
        Gizmos.DrawWireCube(adjustedPosition, adjustedSize);
    }
}
