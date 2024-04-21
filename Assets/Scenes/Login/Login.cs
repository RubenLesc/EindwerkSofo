using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Login : MonoBehaviour
{
    public InputField Usernamefield;
    public InputField Passwordfield;
    public Button txtRegister;

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
                if (www.downloadHandler.text.Contains("01"))
                {
                    DBmanager.username = Usernamefield.text;
                    string[] php = www.downloadHandler.text.Split('\t');
                    string statusCode = php[0];
                    DBmanager.admin = int.Parse(php[1]);
                    DBmanager.coins = int.Parse(php[2]);
                    DBmanager.damage = int.Parse(php[3]);
                    DBmanager.speed = int.Parse(php[4]);
                    DBmanager.health = int.Parse(php[5]);

                    SceneManager.LoadScene(2);//Gaan naar mainmenu Scene (2) (buildsettings in unity) file --> buildsettings)

                }
                else
                {
                    Debug.Log("Gebruiker login mislukt. ERROR #" + www.downloadHandler.text);
                }
            }
            else
            {
                Debug.Log("Kan login.php niet vinden." + www.downloadHandler.text);
            }


        }
    }
    public void Verifyinput()
    {
        txtRegister.interactable = (Usernamefield.text.Length >= 3 && Passwordfield.text.Length >= 5);
    }
}




