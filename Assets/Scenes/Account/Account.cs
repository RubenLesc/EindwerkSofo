using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Account : MonoBehaviour
{
    public Text Username;
    public Text Level1;
    public Button Logout;
    public InputField currentPasswordInput;
    public InputField newPasswordInput;
    public InputField confirmPasswordInput;
    public Text error;

    public void GotoMain()
    {
        SceneManager.LoadScene(2);
    }

    public void Awake()
    {
        string StrUsername = DBmanager.username;
        if (StrUsername == null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            StrUsername = StrUsername.ToUpper();
            Username.text = "Username: " + StrUsername;
            StartCoroutine(PersonalLeaderboard());
        }
    }

    public void ChangePassword()
    {
        string username = DBmanager.username;
        string currentPassword = currentPasswordInput.text;
        string newPassword = newPasswordInput.text;
        string confirmPassword = confirmPasswordInput.text;

        StartCoroutine(ChangePasswordCoroutine(username, currentPassword, newPassword, confirmPassword));
    }

    private IEnumerator ChangePasswordCoroutine(string username, string currentPassword, string newPassword, string confirmPassword)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", username);
        form.AddField("current_password", currentPassword);
        form.AddField("new_password", newPassword);
        form.AddField("confirm_password", confirmPassword);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/changepassword.php", form))
        {
            Debug.Log("Sending request");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Request failed: " + www.error);
                error.text = "Error: " + www.error;
            }
            else
            {
                string responseText = www.downloadHandler.text;
                Debug.Log("Response: " + responseText);

                switch (responseText)
                {
                    // Handle different response cases
                }
            }
        }
    }

    IEnumerator PersonalLeaderboard()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", DBmanager.username);
        form.AddField("id", DBmanager.playerId);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/personalrecords.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError ||
                www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
                yield break;
            }

            string response = www.downloadHandler.text;
            string[] times = response.Split('|');

            if (times.Length >= 4)
            {
                string level1Time = times[0].Trim();
                string level2Time = times[1].Trim();
                string level3Time = times[2].Trim();
                string level4Time = times[3].Trim();

                Debug.Log(level1Time);
                Debug.Log(level2Time);
                Debug.Log(level3Time);
                Debug.Log(level4Time);

                Level1.text = level1Time + "\n\n" + level2Time + "\n\n" + level3Time + "\n\n" + level4Time;
            }
            else
            {
                Debug.LogError("Invalid response format");
            }
        }
    }

    public void Loggout()
    {
        DBmanager.LogOut();
        SceneManager.LoadScene(0);
    }
}
