using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JetBrains.Annotations;
using UnityEngine.Networking;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class RegisterPage : MonoBehaviour
{
    public InputField Usernamefield;
    public InputField Passwordfield;
    public InputField Passwordfieldconfirm;
    public Button txtRegister;
    public Text lblError;


    public void CallRegister()
    {
        string username = Usernamefield.text;
        string password = Passwordfield.text;
        string passwordConfirm = Passwordfieldconfirm.text;
        //check if username lenght is longer than 3 characters 
        if (username.Length < 3)
        {
            Debug.Log("Username Error");
            lblError.text = "Username must be at least 3 characters long";
            return;
        }
        //check if password lenght is longer than 5 characters 
        if (password.Length < 5)
        {
            Debug.Log("Password Error");
            lblError.text = "Password must be at least 5 characters long";
            return;
        }

        if (password == passwordConfirm)
        {
            Debug.Log("Starting registration process");
            StartCoroutine(Register());
        }
        else
        {
            Debug.Log("Password Error");
            lblError.text = "Passwords do not match";
        }
    }

    IEnumerator Register()
    {
        if (string.IsNullOrEmpty(Usernamefield.text) || string.IsNullOrEmpty(Passwordfield.text))
        {
            lblError.text = "Username or password cannot be empty";
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("name", Usernamefield.text);
        form.AddField("password", Passwordfield.text);

        txtRegister.interactable = false; // Disable register button to prevent multiple submissions

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/register.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string responseText = www.downloadHandler.text;
                if (responseText.Contains("0"))
                {
                    Debug.Log("User created successfully.");
                    lblError.text = "Registration successful!";
                    SceneManager.LoadScene(0);
                }
                else if (responseText.Contains("3"))
                {
                    Debug.Log("User creation failed: " + responseText);
                    lblError.text = "Username already in use";
                }
                else
                {
                    Debug.Log("User creation failed: " + responseText);
                    lblError.text = "Unknown Error " + responseText;
                }
            }
            else
            {
                Debug.Log("Network Error: " + www.error);
                lblError.text = "Network error: " + www.error;
            }
        }
        txtRegister.interactable = true;
    }

    public void GoToLogin()
    {
        SceneManager.LoadScene(0);
    }

    public void VerifyInput()
    {
        txtRegister.interactable = (Usernamefield.text.Length >= 3 && Passwordfield.text.Length >= 5);
    }
}
