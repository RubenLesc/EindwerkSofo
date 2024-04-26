using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spikebehavior : MonoBehaviour
{
    public GameManager GameManager;
    private Animator animator;
    public GameObject player;
    private AnimationEvent die;

    void Start()
    {
            animator = player.GetComponent<Animator>();
       
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            Debug.Log("Dood door spike");
            Die();
        }
    }
    public void Die()
    {
        if (animator != null)
        {
            animator.SetTrigger("death");
            
            StartCoroutine(WaitForSeconds());

            
        }

        IEnumerator WaitForSeconds()
        {
            
            //wachten voor 2 seconden
            yield return new WaitForSeconds(0.7f) ;
            //level opnieuw starten
            SceneManager.LoadScene(6);
        }


    }
        
}

