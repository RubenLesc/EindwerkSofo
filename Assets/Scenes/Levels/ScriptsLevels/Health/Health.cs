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
    {   //formule om speler 1 starthealth te geven en voor elk level +0.5 bij te doen
        StartingHealth = 1 + 0.5f *(DBmanager.health - 1);
        
        CurrentHealth = StartingHealth;
    }

    public void TakeDamage(float _damage)
    {   
        //ondergrends en bovengrens
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
    {   //check als het boject nog leeft
        return CurrentHealth > 0;
    }
    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
