using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public int CoinsCollected = 0;
    public int DamageLevel = 1;
    public Text Score;
    


    public void Awake()
    {
        //als de scene geladen word dan word er gekeken of je bent ingelogd of niet als je bent ingelogd dan gaat de code als het moet anders wordt je naar de loginpagina gestuurt
        if (DBmanager.username == null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            CoinsCollected = DBmanager.coins;
            DamageLevel = DBmanager.damage;

        }
    }

    private void Update()
    {
        Score.text = "Coins: " + CoinsCollected;
    }

    public void CollectCoins()
    {
        CoinsCollected++;
    }
    public void CollectScore()
    {
        GetComponent<Text>().text = Score.text;
    }
    
}
