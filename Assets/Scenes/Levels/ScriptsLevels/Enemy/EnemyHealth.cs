using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] public float StartingHealth;
    public Animator enemyAnimator;
    public float CurrentHealth { get; private set; }

    private void Awake()
    {
        CurrentHealth = StartingHealth;
        
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, StartingHealth);

        if (CurrentHealth <= 0)
        {
            Die();
            Debug.Log("Enemy died");
            EnemyPatrol enemyPatrol = GetComponentInParent<EnemyPatrol>();
            if (enemyPatrol != null)
            {
                enemyPatrol.enabled = false;
            }
            MeleeEnemy meleeEnemy = GetComponent<MeleeEnemy>();
            if (meleeEnemy != null)
            {
                meleeEnemy.enabled = false;
            }
        }
        else
        {
            Debug.Log("Enemy currentHealth " +  CurrentHealth);
            enemyAnimator.SetTrigger("hurt"); 
        }
    }

    private void Die()
    {
        enemyAnimator.SetTrigger("death");
        
    }
    public bool IsAlive()
    {
        return CurrentHealth > 0;
    }
}
