using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JetBrains.Annotations;
using UnityEngine.Networking;
using Unity.VisualScripting;


public class RegisterPage : MonoBehaviour
{

    public InputField Usernamefield;
    public InputField Passwordfield;
    public InputField Passwordfieldconfirm;
    public Button txtRegister;
    public Text lblError;


    public void CallRegister()
    {

        string Password, Passwordconfirm;

        Password = Passwordfield.text;
        Passwordconfirm = Passwordfieldconfirm.text;

        if (Password == Passwordconfirm)
        {
            Debug.Log("Register wordt gestart");
            StartCoroutine(Register());
        }
        else
        {
            Debug.Log("Password Error");
            lblError.text = "Wachtwoord komt niet overeen met Confirm wachtwoord";
        }
    }

    IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", Usernamefield.text);
        form.AddField("password", Passwordfield.text);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/register.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                if (www.downloadHandler.text == "0")
                {
                    Debug.Log("Gebruiker aangemaakt.");
                    UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                }
                
                else
                {
                    Debug.Log("Gebruiker aanmaken mislukt" + www.downloadHandler.text);
                }
            }
            else
            {
                Debug.Log("Network Error: " + www.error);
            }
        }
    }
    public void Gotologin()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }



    public void Verifyinput()
    {
        txtRegister.interactable = (Usernamefield.text.Length >= 3 && Passwordfield.text.Length >= 5);
    }

}
