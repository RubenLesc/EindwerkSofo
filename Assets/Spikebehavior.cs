using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikebehavior : MonoBehaviour
{
    public GameManager GameManager;
    private Animator animator;
    public GameObject player;

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
        }
        else
        {
            Debug.LogWarning("Animator component is not assigned to the player GameObject.");
        }
    }
}
