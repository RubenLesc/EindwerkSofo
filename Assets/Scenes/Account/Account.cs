using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Account : MonoBehaviour
{
    public Text Username;
    public Button Logout;
    public InputField currentPasswordInput;
    public InputField newPasswordInput;
    public InputField confirmPasswordInput;
    public Text error;
    public void GotoMain()
    {
        SceneManager.LoadScene(2); //Gaan naar mainmenu Scene (2) (buildsettings in unity) file --> buildsettings)
    }
    public void Awake()
    {   
        //als scene account word geopent dan word in de label username de gebruikersnaam getoont in grote letters
        string StrUsername = DBmanager.username;

        if (StrUsername == null )
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            StrUsername = StrUsername.ToUpper();
            Username.text = "Username: " + StrUsername;
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

        UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/changepassword.php", form);

        // Debug log to see if the request is sent
        Debug.Log("Sending request");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            // Debug log to see the error if request failed
            Debug.Log("Request failed: " + www.error);
            error.text = "Error: " + www.error;
        }
        else
        {
            // Debug log to see the response text
            string responseText = www.downloadHandler.text;
            Debug.Log("Response: " + responseText);

            // Handle the server response
            switch (responseText)
            {
                case "0":
                    error.text = "Password changed successfully.";
                    ResetInputFields();
                    break;
                case "1: Connection failed: ...":
                    error.text = "Connection failed.";
                    break;
                case "2: Required fields are missing":
                    error.text = "Please fill in all required fields.";
                    break;
                case "5: Either no user with name, or more than one":
                    error.text = "User not found or duplicate users.";
                    break;
                case "6: Incorrect current password":
                    error.text = "Current password is incorrect.";
                    break;
                case "7: Update password query failed: ...":
                    error.text = "Failed to update password.";
                    break;
                case "8: New password and confirmation do not match":
                    error.text = "New passwords do not match.";
                    break;
                case "9: New password must be at least 5 characters long":
                    error.text = "New password must be at least 5 characters long.";
                    break;
                default:
                    error.text = "Unexpected error: " + responseText;
                    break;
            }
        }
    }

    private void ResetInputFields()
    {
        currentPasswordInput.text = "";
        newPasswordInput.text = "";
        confirmPasswordInput.text = "";
    }


    public void Loggout()
    {
        DBmanager.LogOut();
        SceneManager.LoadScene(0);
    }
}



