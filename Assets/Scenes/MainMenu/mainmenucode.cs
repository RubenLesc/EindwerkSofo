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
    public Button adminbutton;
    public void Awake()
    {
        //When scene is opened the username and coins are displayed
        string StrUsername = DBmanager.username;
        int strcoins = DBmanager.coins;
        int admin = DBmanager.admin;

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
            //Check if they have permisson to acces the admin menu
            if (admin == 1)
            {
                adminbutton.interactable = true;
            }
            else if (admin != 1)
            {
                adminbutton.interactable = false;
            }
        }
        //schaduw
        Coins.text =  strcoins.ToString();
        Coins2.text = strcoins.ToString();

    }
}
