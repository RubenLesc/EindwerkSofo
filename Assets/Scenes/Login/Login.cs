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
                string responseText = www.downloadHandler.text;
                Debug.Log("Response from server: " + responseText);

                string[] php = responseText.Split(':');

                if (php.Length > 1) // Check if response contains an error code
                {
                    int errorCode;
                    if (int.TryParse(php[0], out errorCode))
                    {
                        switch (errorCode)
                        {
                            case 0:
                                string[] userData = php[1].Split('\t');
                                DBmanager.username = Usernamefield.text;
                                DBmanager.playerId = int.Parse(userData[6]);
                                DBmanager.admin = int.Parse(userData[1]);
                                DBmanager.coins = int.Parse(userData[2]);
                                DBmanager.damage = int.Parse(userData[3]);
                                DBmanager.speed = int.Parse(userData[4]);
                                DBmanager.health = int.Parse(userData[5]);
                                SceneManager.LoadScene(2); // Go to the main menu scene
                                break;
                            case 3:
                                Debug.Log("Username doesn't exist");
                                error.text = "Username doesn't exist\n Please try again";
                                break;
                            case 6:
                                Debug.Log("Incorrect password");
                                error.text = "Incorrect password\n Try again";
                                break;
                            default:
                                Debug.Log("Login failed. ERROR #" + errorCode);
                                error.text = "Login failed\n Please try again later";
                                break;
                        }
                    }
                    else
                    {
                        Debug.LogError("Error parsing error code");
                        error.text = "Login failed\n Please try again later";
                    }
                }
                else
                {
                    Debug.LogError("Invalid response format");
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
