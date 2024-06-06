using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public Button lvl2;
    public Button lvl3;
    public Button lvl4;

    public Sprite lvl2NullTimeSprite;
    public Sprite lvl3NullTimeSprite;
    public Sprite lvl4NullTimeSprite;


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

            //Check if permission to admin menu
            if (admin == 1)
            {
                adminbutton.interactable = true;
            }
            else
            {
                adminbutton.interactable = false;
            }

            // did you beat levels 
            if (DBmanager.levelTime1 == null)
            {
                lvl2.image.sprite = lvl2NullTimeSprite;
                lvl2.interactable = false;
            }
            else
            {
                lvl2.image.sprite = lvl2.GetComponent<Image>().sprite;
                lvl2.interactable = true;
            }
            if (DBmanager.levelTime2 == null)
            {
                lvl3.image.sprite = lvl3NullTimeSprite;
                lvl3.interactable = false;
            }
            else
            {
                lvl3.image.sprite = lvl3.GetComponent<Image>().sprite;
                lvl3.interactable = true;
            }

            if (DBmanager.levelTime3 == null)
            {
                lvl4.image.sprite = lvl4NullTimeSprite;
                
                lvl4.interactable = false;
            }
            else
            {
                lvl4.image.sprite = lvl4.GetComponent<Image>().sprite;
                lvl4.interactable = true;
            }


        }

        //schaduw
        Coins.text = strcoins.ToString();
        Coins2.text = strcoins.ToString();
    }
}
