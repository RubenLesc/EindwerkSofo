using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class Login : MonoBehaviour
{   
    //references naar de objecten in unity
    public InputField Usernamefield;
    public InputField Passwordfield;
    public Button txtRegister;
    public Text error;

    public void CallLogin()
    {
        //wordt opgroepen als je op de login knop drukt
        StartCoroutine(LoginUser());
    }

    IEnumerator LoginUser()
    {   
        
        string username = Usernamefield.text.Trim();
        string password = Passwordfield.text.Trim();

        //checkt als de velden leeg zijn
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            error.text = "The fields cannot be empty";
            yield break;
        }
        //form voor post
        WWWForm form = new WWWForm();
        form.AddField("name", username);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/login.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string responseText = www.downloadHandler.text.Trim();

                try
                {
                    //check als antwoord een error aangeeft
                    if (responseText.StartsWith("0\t"))
                    {
                        //splits antwoord
                        string[] php = responseText.Split('\t');

                        if (php.Length == 11)
                        {
                            DBmanager.username = username;
                            DBmanager.admin = int.Parse(php[1]);
                            DBmanager.coins = int.Parse(php[2]);
                            DBmanager.damage = int.Parse(php[3]);
                            DBmanager.speed = int.Parse(php[4]);
                            DBmanager.health = int.Parse(php[5]);
                            DBmanager.playerId = int.Parse(php[6]);
                            DBmanager.levelTime1 = php[7] != "null" ? php[7] : null;
                            DBmanager.levelTime2 = php[8] != "null" ? php[8] : null;
                            DBmanager.levelTime3 = php[9] != "null" ? php[9] : null;
                            DBmanager.levelTime4 = php[10] != "null" ? php[10] : null;
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
                    else if (responseText.StartsWith("7:"))
                    {
                        Debug.Log("Cannot be empty");
                        error.text = "The fields cannot be empty";
                    }
                    else
                    {
                        Debug.LogError("Error parsing response from server: " + responseText);
                        error.text = "Login failed\n Please try again later";
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("Exception caught while processing response: " + ex.Message);
                    error.text = "Login failed\n Please try again later";
                }
            }
            else
            {
                Debug.LogError("Failed to send web request: " + www.error);
                error.text = "Connection error";
                //checks if you have connection with the database
                switch (www.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                        error.text = "Check if you have connection with the database";
                        break;
                }
            }
        }
    }
    //verify input
    public void Verifyinput()
    {
        txtRegister.interactable = (Usernamefield.text.Length >= 3 && Passwordfield.text.Length >= 5);
    }
}
