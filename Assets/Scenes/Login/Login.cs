using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public InputField Usernamefield;
    public InputField Passwordfield;
    public Button txtRegister;
    public Text error;

    public void CallLogin()
    {
        StartCoroutine(LoginUser());
    }

    IEnumerator LoginUser()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", Usernamefield.text);
        form.AddField("password", Passwordfield.text);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/login.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string responseText = www.downloadHandler.text.Trim();
                Debug.Log("Response from server: " + responseText);

                // Check if the response starts with an error code
                if (responseText.StartsWith("0\t"))
                {
                    // Split the response text by tab character
                    string[] php = responseText.Split('\t');
                    Debug.Log("Response split length: " + php.Length);

                    if (php.Length == 7)
                    {
                        DBmanager.username = Usernamefield.text;
                        DBmanager.admin = int.Parse(php[1]);
                        DBmanager.coins = int.Parse(php[2]);
                        DBmanager.damage = int.Parse(php[3]);
                        DBmanager.speed = int.Parse(php[4]);
                        DBmanager.health = int.Parse(php[5]);
                        DBmanager.playerId = int.Parse(php[6]);
                        SceneManager.LoadScene(2); // Go to the main menu scene
                    }
                    else
                    {
                        Debug.LogError("Unexpected response format: " + responseText);
                        error.text = "Login failed\n Please try again later";
                    }
                }
                else if (responseText.StartsWith("3:"))
                {
                    Debug.Log("Username doesn't exist");
                    error.text = "Username doesn't exist\n Please try again";
                }
                else if (responseText.StartsWith("6:"))
                {
                    Debug.Log("Incorrect password");
                    error.text = "Incorrect password\n Try again";
                }
                else
                {
                    Debug.LogError("Error parsing response from server: " + responseText);
                    error.text = "Login failed\n Please try again later";
                }
            }
            else
            {
                Debug.LogError("Failed to send web request: " + www.error);
                error.text = "Connection error\n Please try again later";
            }
        }
    }

    public void Verifyinput()
    {
        txtRegister.interactable = (Usernamefield.text.Length >= 3 && Passwordfield.text.Length >= 5);
    }
}
