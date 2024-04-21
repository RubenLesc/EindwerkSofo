using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Account : MonoBehaviour
{
    public Text Username;
    public Button Logout;
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
    public void Loggout()
    {
        DBmanager.LogOut();
        SceneManager.LoadScene(0);
    }
}
