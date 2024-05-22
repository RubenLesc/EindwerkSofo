using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Leaderboard : MonoBehaviour
{
    public Text leaderboardcoins;
    public Text leaderboardTime;

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

                // Split the response into lines
                string[] lines = response.Split('\n');

                // Concatenate the lines within each section
                string coinsText = string.Join("\n", lines, 1, Array.IndexOf(lines, "Time") - 1).Trim();
                string timeText = string.Join("\n", lines, Array.IndexOf(lines, "Time") + 1, lines.Length - Array.IndexOf(lines, "Time") - 1).Trim();

                // Update leaderboard coins and time texts
                leaderboardcoins.text = coinsText;
                leaderboardTime.text = timeText;
            }
        }
    }

    public void GotoMain()
    {
        SceneManager.LoadScene(2); // Go to main menu scene (2) (build settings in Unity) File -> Build Settings)
    }
}
