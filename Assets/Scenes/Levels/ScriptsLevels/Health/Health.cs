
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    //declaring variables For health
    [SerializeField] private float StartingHealth;
    public Animator playerAnimator;
    public player_Movement playerMovement;
    public float CurrentHealth { get; private set; } //Makes it so you can access from anywhere but only modify in this script

    private void Awake()
    {
        CurrentHealth = StartingHealth;
    }
    public void TakeDamage(float _damage)
    {

    //It ensures value stays within specified range (Clamp)
    //If value is less than min (0) returns min
    //If value is greater than max (StartingHealth) returns max (StartingHealth)
    //If value is between min and max (Between 0 and StartingHealth, returns value itself (currenthealth)
        CurrentHealth = Mathf.Clamp(CurrentHealth - _damage, 0, StartingHealth);

        if (CurrentHealth > 0)
        {
            //player gets damaged
        }
        else
        {
            //player dies
            Debug.Log("Die");
            if (playerAnimator != null)
            {
                Debug.Log("Animation");
                playerAnimator.SetTrigger("death");

                if (playerMovement != null)
                {
                    playerMovement.StopMovement();
                }

                StartCoroutine(WaitForSeconds(0.7f));
            }
        }

        IEnumerator WaitForSeconds(float seconds)
        {
            //wait for 0.7 seconds for the animation to end
            yield return new WaitForSeconds(seconds);
            //restart the current scene so the time restarts
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(0.5f); // Call TakeDamage on the playerhealth instance
        }
    }
}