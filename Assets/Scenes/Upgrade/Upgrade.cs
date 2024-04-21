using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    int currentcoins = 0;
    public Button UpgradeDamage;
    string StrUsername = DBmanager.username;
    int Health = DBmanager.health;
    int Damage = DBmanager.damage;
    int Speed = DBmanager.speed;
    int newcoins = 0;
    public Text Username;
    public Text Coins;

    public void GotoMain()
    {
        SceneManager.LoadScene(2); //Gaan naar mainmenu Scene (2) (buildsettings in unity) file --> buildsettings)
    }
    public void UpgradeSword()
    {
        // Decrease coins by "Price"
        currentcoins = DBmanager.coins;
        newcoins = currentcoins - 1;

        // Check if the player has enough coins for the upgrade
        if (newcoins >= 0)
        {
            // Update the UI with the new number of coins
            Coins.text = "Coins\n" + newcoins;

            // Send the updated coins and other player data to the database
            StartCoroutine(SaveCurretcoins(newcoins, Damage, Health, Speed));
        }
        else
        {
            Debug.Log("Not enough coins for the upgrade.");
        }
    }
    
    private void Awake()
    {   //als de scene geladen word dan word er gekeken of je bent ingelogd of niet als je bent ingelogd dan gaat de code als het moet anders wordt je naar de loginpagina gestuurt
        if (StrUsername == null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {   //tekst bovenaan inladen
            currentcoins = DBmanager.coins;
            StrUsername = StrUsername.ToUpper();
            Username.text = "Username\n" + StrUsername;
            Coins.text = "Coins\n" + currentcoins;
        }
    }
    IEnumerator SaveCurretcoins(int currentcoins, int damage, int health, int speed)
    {
        // form maken zodat je data kan sturen naar het script
        WWWForm form = new WWWForm();
        form.AddField("coins", currentcoins);
        form.AddField("username", DBmanager.username);
        form.AddField("damage", damage);
        form.AddField("health", health);
        form.AddField("speed", speed);

        // Send the request to the PHP script
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/upgrade.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                if (www.downloadHandler.text.Contains("3"))
                {
                    Debug.Log(www.downloadHandler.text);

                    //update local variable van coins naar de nieuwe waarde
                    DBmanager.coins = currentcoins;

                    //tekst bovenaan veranderen door de nieuwe waarde
                    Coins.text = "Coins\n" + currentcoins;
                }
                else
                {
                    Debug.Log(www.downloadHandler.text);
                }
            }
            else
            {
                Debug.Log("Kan upgrades.php niet vinden" + www.downloadHandler.text);
            }
        }
    }

}
