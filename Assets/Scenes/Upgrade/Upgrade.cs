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
    [SerializeField] public int CostPerUpgrade = 20;
    bool CheckDamage = false;
    bool CheckHealth = false;
    bool CheckSpeed = false;
    public Button btnUpgradeDamage;
    public Button btnUpgradeHealth;
    public Button btnUpgradeSpeed;
    public Text txtUsername;
    public Text txtCoins;
    public Text txtCostDamage;
    public Text txtCostHealth;
    public Text txtCostSpeed;
    public Text txtLevelDamage;
    public Text txtLevelHealth;
    public Text txtLevelSpeed;
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

            //tekst upgrade updaten
            txtCoins.text = "Coins\n" + currentcoins;
            txtLevelDamage.text = "Level: " + DBmanager.damage + "\nNext Upgrade: " + Damagecost;
            Damagecost = DBmanager.damage * CostPerUpgrade + CostPerUpgrade;
            txtCostDamage.text = "Upgrade\nCost: " + Damagecost;
            //tekst health updaten
            txtCoins.text = "Coins\n" + currentcoins;
            txtLevelHealth.text = "Level: " + DBmanager.health + "\nNext Upgrade: " + Healthcost;
            Healthcost = DBmanager.health * CostPerUpgrade + CostPerUpgrade;
            txtCostHealth.text = "Upgrade\nCost: " + Healthcost;
            //tekst Speed updaten
            txtCoins.text = "Coins\n" + currentcoins;
            txtLevelSpeed.text = "Level: " + DBmanager.speed + "\nNext Upgrade: " + Speedcost;
            Speedcost = DBmanager.speed * CostPerUpgrade + CostPerUpgrade;
            txtCostSpeed.text = "Upgrade\nCost: " + Speedcost;

        }
    }
    public void UpgradeSword()
    {
        
        // Decrease coins by "Price"
        currentcoins = DBmanager.coins;
        Damage = DBmanager.damage + 1;
        Damagecost = DBmanager.damage * CostPerUpgrade + CostPerUpgrade;
        newcoins = currentcoins - Damagecost;
        

        // Check if the player has enough coins for the upgrade
        if (newcoins >= 0)
        {

            //kijken welke upgrade je hebt gekocht
            CheckDamage = true;

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
        Health = DBmanager.health + 1;
        Healthcost = DBmanager.health * CostPerUpgrade + CostPerUpgrade;
        newcoins = currentcoins - Healthcost;

        // Check if the player has enough coins for the upgrade
        if (newcoins >= 0)
        {
            //kijken welke upgrade je hebt gekocht
            CheckHealth = true;
            
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
        Speed = DBmanager.speed + 1;
        Speedcost = DBmanager.speed * CostPerUpgrade + CostPerUpgrade;
        newcoins = currentcoins - Speedcost;

        // Check if the player has enough coins for the upgrade
        if (newcoins >= 0)
        {
            //kijken welke upgrade je hebt gekocht
            CheckSpeed = true;

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
                    //Het level van je stats verhogen met 1
                    if (CheckDamage == true)
                    {
                        DBmanager.damage = Damage;
                    }
                    else if (CheckHealth == true)
                    {
                        DBmanager.health = Health;
                    }
                    else if (CheckSpeed == true)
                    {
                        DBmanager.speed = Speed;
                    }
                    else
                    {
                        Debug.Log("Er is iets misgegaan, je hebt blijkbaar niets gekocht");
                    }


                    DBmanager.health = Health;
                    DBmanager.speed = Speed;
                    //tekst bovenaan veranderen door de nieuwe waarde
                    txtCoins.text = "Coins\n" + currentcoins;

                    //tekst upgrade updaten
                    txtCoins.text = "Coins\n" + newcoins;
                    txtLevelDamage.text = "Level: " + DBmanager.damage + "\nNext Upgrade: " + Damagecost;
                    txtCostDamage.text = "Upgrade\nCost: " + Damagecost;
                    //tekst health updaten
                    txtCoins.text = "Coins\n" + newcoins;
                    txtLevelHealth.text = "Level: " + DBmanager.health + "\nNext Upgrade: " + Healthcost;
                    txtCostHealth.text = "Upgrade\nCost: " + Healthcost;
                    //tekst Speed updaten
                    txtCoins.text = "Coins\n" + newcoins;
                    txtLevelSpeed.text = "Level: " + DBmanager.speed + "\nNext Upgrade: " + Speedcost;
                    txtCostSpeed.text = "Upgrade\nCost: " + Speedcost;

                    //Alle 3 op false zetten zodat je zeker niet 2 upgrades tergelijk doet terwijl je maar voor 1 betaalt
                    CheckDamage = false;
                    CheckHealth = false;
                    CheckSpeed = false;
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
