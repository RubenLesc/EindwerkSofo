using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Finish_Saving : MonoBehaviour
{
    // Reference to the GameManager
    public GameManager gameManager;
    private Animator playerAnimator;
    private player_Movement playerMovement;

    private void Start()
    {
        // Get the Animator component from the player character
        GameObject player = GameObject.Find("Player");
        playerAnimator = player.GetComponent<Animator>();
        playerMovement = player.GetComponent<player_Movement>(); // Get the player_Movement component
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            Debug.Log("Finished");

            // Get the collected coins and elapsed time from the GameManager
            int collectedCoins = gameManager.CoinsCollected;
            //See how long you took for completing the level
            float elapsedTime = gameManager.GetElapsedTime();

            // Save the collected coins and elapsed time in the DBmanager
            DBmanager.coins = collectedCoins;
            DBmanager.elapsedTime = elapsedTime; // Send the elapsed time to the database

            //Go to winanimation to start playing the animation
            Winanimation();

            // Send collected coins and elapsed time to the database
            StartCoroutine(SendDataToDB(collectedCoins, elapsedTime));

            //wait 2seconds before switching scene so you see the winning animation
            Invoke("GoToMain", 2f);
        }
    }
    void Winanimation()
    {
        if (playerMovement != null)
        {
            playerMovement.StopMovement();
        }
        //Play win animation
        playerAnimator.SetTrigger("Win");
    }

    void GoToMain()
    {
        // Load the next scene
        SceneManager.LoadScene(2);
    }
    IEnumerator SendDataToDB(int collectedCoins, float elapsedTime)
    {
        // Create a form object for sending data to the PHP script
        WWWForm form = new WWWForm();
        form.AddField("coins", collectedCoins);
        form.AddField("username", DBmanager.username);
        form.AddField("elapsedTime", elapsedTime.ToString());
        form.AddField("id", DBmanager.playerId);

        // Send the request to the PHP script
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/finish.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                if (www.downloadHandler.text.Contains("3"))
                {
                    Debug.Log("Opgeslagen: " + www.downloadHandler.text);
                }
                else
                {
                    Debug.Log("Coins opslaan mislukt: " + www.downloadHandler.text);
                }
            }
            else
            {
                Debug.Log("Kan finish.php niet vinden: " + www.downloadHandler.text);
            }
        }
    }
}
