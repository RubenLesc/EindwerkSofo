using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class Leaderboard : MonoBehaviour
{
    public Text leaderboardcoins;
    public Text leaderboardTime;

    void Awake()
    {
        if (DBmanager.username ==null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            StartCoroutine(UpdateLeaderboard());
        }
        
    }

    IEnumerator UpdateLeaderboard()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/sqlconnect/leaderboard.php"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                //split responds (echo)
                string response = www.downloadHandler.text.Trim();

                //Split the response into coins and time for seperate labels
                string[] splitResponse = response.Split(new string[] { "Time" }, System.StringSplitOptions.None);

                if (splitResponse.Length >= 2)
                {
                    //Update leaderboardcoins
                    leaderboardcoins.text = splitResponse[0].Trim();
                    //Update leaderboardtime
                    leaderboardTime.text = splitResponse[1].Trim();
                }
                else
                {
                    Debug.Log("Response does not contain both coins and time sections");
                }

                // Log the status message
                Debug.Log("3: Opgeslagen");
            }
        }
    }

    public void GotoMain()
    {
        SceneManager.LoadScene(2); // Go to main menu scene (2) (build settings in Unity) File -> Build Settings)
    }
}
