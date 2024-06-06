using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] public Animator anim;
    public Transform Attackpoint;
    [SerializeField] public float AttackRange = 0.5f;
    public LayerMask enemylayer;
    public float attackCooldown = 1f;
    private float cooldownTimer = Mathf.Infinity;
    //facing direction
    private bool facingRight = true;
    private int damage = DBmanager.damage;

    void Update()
    {
        //cooldownn
        cooldownTimer += Time.deltaTime;

        // Check if the cooldown time has passed
        if (cooldownTimer >= attackCooldown)
        {
            // Check if the player pressed spacee
            if (Input.GetKeyDown(KeyCode.Space))
            {
                cooldownTimer = 0f;
                Attack();
            }
        }
    }

    void Attack()
    {
        // Play attack animation
        anim.SetTrigger("Attack");

        // Detect enemies in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(Attackpoint.position, AttackRange, enemylayer);

        // Damage enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Enemy hit");
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

            // Check if the enemy has health and is alive
            if (enemyHealth != null && enemyHealth.IsAlive())
            {
                Debug.Log(damage + "Damage");
                Debug.Log("Enemy hit: " + enemy.name);
                enemyHealth.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (Attackpoint == null)
            return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Attackpoint.position, AttackRange);
    }

    
    public void Flip()
    {
        // Flip the attack point's position
        Vector3 attackPointPosition = Attackpoint.localPosition;
        attackPointPosition.x *= -1;
        Attackpoint.localPosition = attackPointPosition;
        

        // Update facing direction
        facingRight = !facingRight;
    }
}
