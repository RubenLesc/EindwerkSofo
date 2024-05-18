using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Finish_Saving : MonoBehaviour
{
    //gamemanager
    public GameManager gameManager;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            Debug.Log("Finished");

            // Get the collected coins from the GameManager
            int collectedCoins = gameManager.CoinsCollected;

            // Save the collected coins in the DBmanager
            DBmanager.coins = collectedCoins;

            // Send collected coins to the database
            StartCoroutine(SendDataToDB(collectedCoins));
            

            // Load the next scene
            SceneManager.LoadScene(2);
        }
    }

    IEnumerator SendDataToDB(int collectedCoins)
    {
        // Create a form object for sending data to the PHP script
        WWWForm form = new WWWForm();
        form.AddField("coins", collectedCoins);
        form.AddField("username", DBmanager.username);

        // Send the request to the PHP script
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/coins.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                if (www.downloadHandler.text.Contains("3"))
                {
                    Debug.Log("Opgeslagen" + www.downloadHandler.text);
                }
                else
                {
                    Debug.Log("Coins opslaan mislukt" + www.downloadHandler.text);
                }
            }
            else
            {
                Debug.Log("Kan coins.php niet vinden" + www.downloadHandler.text);
            }
        }
    }

}
