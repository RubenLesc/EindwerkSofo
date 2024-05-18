using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainmenuCode : MonoBehaviour
{
    public Text Username;
    public Text Coins;
        public Text Username2;
    public Text Coins2;
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
            //schaduw
            Username.text = "Welcome: " + StrUsername;
            Username2.text = "Welcome: " + StrUsername;
        }
        //schaduw
        Coins.text =  strcoins.ToString();
        Coins2.text = strcoins.ToString();

    }
}
