using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Finish_Saving : MonoBehaviour
{
    // Reference objecten
    public GameManager gameManager;
    private Animator playerAnimator;
    private player_Movement playerMovement;

    private void Start()
    {
        // krijg componenten
        GameObject player = GameObject.Find("Player");
        playerAnimator = player.GetComponent<Animator>();
        playerMovement = player.GetComponent<player_Movement>(); // Get the player_Movement component
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            Debug.Log("Finished");

            //krijg tijd en coins voor completion van gamemanager
            int collectedCoins = gameManager.CoinsCollected;
            float elapsedTime = gameManager.GetElapsedTime();

            //save de coins en tijd in de local database
            DBmanager.coins = collectedCoins;
            DBmanager.elapsedTime = elapsedTime;

            Winanimation();

            //zet de tijd in de juiste tijd voor level in local
            switch (DBmanager.level)
            {
                case 1:
                    DBmanager.levelTime1 = elapsedTime.ToString();
                    break;
                case 2:
                    DBmanager.levelTime2 = elapsedTime.ToString();
                    break;
                case 3:
                    DBmanager.levelTime3 = elapsedTime.ToString();
                    break;
                case 4:
                    DBmanager.levelTime4 = elapsedTime.ToString();
                    break;
            }
            StartCoroutine(SendDataToDB(collectedCoins, elapsedTime));
            Invoke("GoToMain", 2f);
        }
    }
    void Winanimation()
    {
        if (playerMovement != null)
        {
            playerMovement.StopMovement();
        }
        playerAnimator.SetTrigger("Win");
    }

    void GoToMain()
    {
        // Load lain menu
        SceneManager.LoadScene(2);
    }
    IEnumerator SendDataToDB(int collectedCoins, float elapsedTime)
    {
        //formobject voor php script
        WWWForm form = new WWWForm();
        form.AddField("coins", collectedCoins);
        form.AddField("username", DBmanager.username);
        form.AddField("elapsedTime", elapsedTime.ToString());
        form.AddField("id", DBmanager.playerId);
        form.AddField("level", DBmanager.level);

        // zend request naar php script
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
