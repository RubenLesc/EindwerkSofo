using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Healthclass : MonoBehaviour
{
    private float StartingHealth;
    public Animator playerAnimator;
    public float CurrentHealth { get; private set; }

    private void Awake()
    {
        StartingHealth = 1 + 0.5f *(DBmanager.health - 1);
        
        CurrentHealth = StartingHealth;
    }

    public void TakeDamage(float _damage)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - _damage, 0, StartingHealth);

        if (CurrentHealth > 0)
        {
            playerAnimator.SetTrigger("hurt");
        }
        else
        {
            playerAnimator.SetTrigger("death");
            if (GetComponent<player_Movement>() != null)
            {
                GetComponent<player_Movement>().enabled = false;
            }

        }
    }



    public bool IsAlive()
    {
        return CurrentHealth > 0;
    }
    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
