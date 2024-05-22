using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spikebehavior : MonoBehaviour
{
    public Animator playerAnimator;
    public player_Movement playerMovement;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            Debug.Log("Player died from spike");
            Die();
        }
    }

    public void Die()
    {
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
