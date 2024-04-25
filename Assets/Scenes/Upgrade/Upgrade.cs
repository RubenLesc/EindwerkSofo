using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{   
    //declareren
    int currentcoins = 0;
    int Damagecost = 1;
    int Healthcost = 2;
    int Speedcost = 3;
    int newcoins = 0;
    public Button btnUpgradeDamage;
    public Button btnUpgradeHealth;
    public Button btnUpgradeSpeed;
    public Text txtUsername;
    public Text txtCoins;
    public Text txtCostDamage;
    public Text txtCostHealth;
    public Text txtCostSpeed;
    string StrUsername = DBmanager.username;
    int Health = DBmanager.health;
    int Damage = DBmanager.damage;
    int Speed = DBmanager.speed;
    
    
    public void GotoMain()
    {
        SceneManager.LoadScene(2); //Gaan naar mainmenu Scene (2) (buildsettings in unity) file --> buildsettings)
    }
    private void Awake()
    {   //als de scene geladen word dan word er gekeken of je bent ingelogd of niet als je bent ingelogd dan gaat de code als het moet anders wordt je naar de loginpagina gestuurt
        if (StrUsername == null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {   //tekst overal inladen
            //bovenaan
            currentcoins = DBmanager.coins;
            StrUsername = StrUsername.ToUpper();
            txtUsername.text = "Username\n" + StrUsername;
            txtCoins.text = "Coins\n" + currentcoins;
            //upgrades
            txtCostDamage.text = "Upgrade\nCost: " + Damagecost;
            txtCostHealth.text = "Upgrade\nCost: " + Healthcost;
            txtCostSpeed.text = "Upgrade\nCost: " + Speedcost;

        }
    }
    public void UpgradeSword()
    {
        
        // Decrease coins by "Price"
        currentcoins = DBmanager.coins;
        newcoins = currentcoins - Damagecost;
        

        // Check if the player has enough coins for the upgrade
        if (newcoins >= 0)
        {
            // Update the UI with the new number of coins
            txtCoins.text = "Coins\n" + newcoins;

            //Het level van je wapen verhogen met 1
            DBmanager.damage = Damage + 1;

            // Send the updated coins and other player data to the database
            StartCoroutine(SaveCurrentcoins(newcoins, Damage, Health, Speed));

            

        }
        else
        {
            Debug.Log("Not enough coins for the upgrade.");
        }
    }
    public void UpgradeHealth()
    {
        
        // Decrease coins by "Price"
        currentcoins = DBmanager.coins;
        newcoins = currentcoins - Healthcost;

        // Check if the player has enough coins for the upgrade
        if (newcoins >= 0)
        {
            // Update the UI with the new number of coins
            txtCoins.text = "Coins\n" + newcoins;

            // Send the updated coins and other player data to the database
            StartCoroutine(SaveCurrentcoins(newcoins, Damage, Health, Speed));
        }
        else
        {
            Debug.Log("Not enough coins for the upgrade.");
        }
    }
    public void UpgradeSpeed()
    {
        
        // Decrease coins by "Price"
        currentcoins = DBmanager.coins;
        newcoins = currentcoins - Speedcost;

        // Check if the player has enough coins for the upgrade
        if (newcoins >= 0)
        {
            // Update the UI with the new number of coins
            txtCoins.text = "Coins\n" + newcoins;

            // Send the updated coins and other player data to the database
            StartCoroutine(SaveCurrentcoins(newcoins, Damage, Health, Speed));
        }
        else
        {
            Debug.Log("Not enough coins for the upgrade.");
        }
    }

    IEnumerator SaveCurrentcoins(int currentcoins, int damage, int health, int speed)
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
                    txtCoins.text = "Coins\n" + currentcoins;
                }
                else
                {   //foutencontrole console
                    Debug.Log(www.downloadHandler.text);
                }
            }
            else
            {   //foutencontrole console
                Debug.Log("Kan upgrades.php niet vinden" + www.downloadHandler.text);
            }
        }
    }

}
