using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainmenuCode : MonoBehaviour
{
    public Text Username;
    public Text Coins;
    public void Awake()
    {
        //als scene account word geopent dan word in de label username de gebruikersnaam getoont in grote letters
        string StrUsername = DBmanager.username;
        int strcoins = DBmanager.coins;

        if (StrUsername == null)
        {
            SceneManager.LoadScene(0);
        }   
        else
        {
            StrUsername = StrUsername.ToUpper();
            Username.text = "Welcome " + StrUsername;
        }
        Coins.text = "Usercoins: " + strcoins;
    }
}
