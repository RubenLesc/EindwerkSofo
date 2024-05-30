using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Leaderboard : MonoBehaviour
{
    public Text leaderboardcoins;
    public Text leaderboardTimeLevel1;
    public Text leaderboardTimeLevel2;
    public Text leaderboardTimeLevel3;
    public Text leaderboardTimeLevel4;

    void Awake()
    {
        if (DBmanager.username == null)
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
                // Process the response
                string response = www.downloadHandler.text.Trim();
                Debug.Log("Response: " + response);

                // Split the response into sections
                string[] sections = response.Split(new string[] { "Time Level 1", "Time Level 2", "Time Level 3", "Time Level 4" }, StringSplitOptions.None);

                if (sections.Length >= 5)
                {
                    // Coins section
                    leaderboardcoins.text = sections[0].Trim();

                    // Time level 1 section
                    leaderboardTimeLevel1.text = "" + sections[1].Trim();

                    // Time level 2 section
                    leaderboardTimeLevel2.text = "" + sections[2].Trim();

                    // Time level 3 section
                    leaderboardTimeLevel3.text = "" + sections[3].Trim();

                    // Time level 4 section
                    leaderboardTimeLevel4.text = "" + sections[4].Trim();
                }
                else
                {
                    Debug.LogError("Unexpected response format.");
                }
            }
        }
    }

    public void GotoMain()
    {
        SceneManager.LoadScene(2); // Go to main menu scene (2) (build settings in Unity) File -> Build Settings)
    }
}
