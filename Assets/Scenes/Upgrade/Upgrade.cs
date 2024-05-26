using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    int currentcoins = 0;
    int Damagecost = 50;
    int Healthcost = 100;
    int Speedcost = 75;
    int newcoins = 0;
    bool CheckDamage = false;
    bool CheckHealth = false;
    bool CheckSpeed = false;
    public Button btnUpgradeDamage;
    public Button btnUpgradeHealth;
    public Button btnUpgradeSpeed;
    public Text txtUsername;
    public Text txtUsername2;
    public Text txtCoins;
    public Text txtCoins2;
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
    {   //exit button
        SceneManager.LoadScene(2); // Go to main menu Scene (2) (build settings in Unity)
    }

    private void Awake()
    {
        if (StrUsername == null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            // Initialize texts
            currentcoins = DBmanager.coins;
            StrUsername = StrUsername.ToUpper();
            txtUsername.text = "Username\n" + StrUsername;
            txtUsername2.text = "Username\n" + StrUsername;
            txtCoins.text = currentcoins.ToString();
            txtCoins2.text = currentcoins.ToString();

            // Update texts for upgrades
            UpdateUpgradeTexts();
        }
    }

    private void UpdateUpgradeTexts()
    {
        UpdateDamageUpgradeText();
        UpdateHealthUpgradeText();
        UpdateSpeedUpgradeText();
        // Update coin texts
        txtCoins.text = currentcoins.ToString();
        txtCoins2.text = currentcoins.ToString();
        Debug.Log(DBmanager.damage);
    }

    private void UpdateDamageUpgradeText()
    {
        if (DBmanager.damage >= 10) //10 is the maximum damage level
        {
            txtLevelDamage.text = "Level: " + DBmanager.damage + "\nMax Level";
            txtCostDamage.text = "Max Level";
            btnUpgradeDamage.interactable = false;
        }
        else if (DBmanager.damage == 9) // Second to last level
        {
            txtLevelDamage.text = "Level: " + DBmanager.damage + "\nNext Upgrade: Max Level";
            txtCostDamage.text = "Upgrade\nCost: " + Damagecost;
        }
        else
        {
            txtLevelDamage.text = "Level: " + DBmanager.damage + "\nNext Upgrade: " + (DBmanager.damage + 1 )+ " damage";
            txtCostDamage.text = "Upgrade\nCost: " + Damagecost;
        }
    }

    private void UpdateHealthUpgradeText()
    {
        if (DBmanager.health >= 19) //18 is the maximum health level
        {
            txtLevelHealth.text = "Level: " + DBmanager.health + "\nMax Level";
            txtCostHealth.text = "Max Level";
            btnUpgradeHealth.interactable = false;
        }
        else if (DBmanager.health == 18) // Second to last level
        {
            txtLevelHealth.text = "Level: " + DBmanager.health + "\nNext Upgrade: Max Level";
            txtCostHealth.text = "Upgrade\nCost: " + Healthcost;
        }
        else
        {
            float healthcalc = ((DBmanager.health * 0.5f) + 1f); // Calculate the next upgrade value based on current health level
            txtLevelHealth.text = "Level: " + DBmanager.health + "\nNext Upgrade: " + healthcalc + " hearts";
            txtCostHealth.text = "Upgrade\nCost: " + Healthcost;
        }
    }

    private void UpdateSpeedUpgradeText()
    {
        if (DBmanager.speed >= 30) //30 is the maximum speed level
        {
            txtLevelSpeed.text = "Level: " + DBmanager.speed + "\nMax Level";
            txtCostSpeed.text = "Max Level";
            btnUpgradeSpeed.interactable = false;
        }
        else if (DBmanager.speed == 29) // Second to last level
        {
            txtLevelSpeed.text = "Level: " + DBmanager.speed + "\nNext Upgrade: Max Level";
            txtCostSpeed.text = "Upgrade\nCost: " + Speedcost;
        }
        else
        {
            txtLevelSpeed.text = "Level: " + DBmanager.speed + "\nNext Upgrade: " + (Speed * 0.2f + 7) + " speed";
            txtCostSpeed.text = "Upgrade\nCost: " + Speedcost;
        }
    }

    public void UpgradeSword()
    {
        // Decrease coins by "Price"
        currentcoins = DBmanager.coins;
        Damage = DBmanager.damage + 1;
        newcoins = currentcoins - Damagecost;

        // Check if the player has enough coins for the upgrade
        if (newcoins >= 0)
        {
            CheckDamage = true;
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
       
        newcoins = currentcoins - Healthcost; // Recalculate the cost

        // Check if the player has enough coins for the upgrade
        if (newcoins >= 0)
        {
            CheckHealth = true;
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
        newcoins = currentcoins - Speedcost; // Now use the updated cost

        // Check if the player has enough coins for the upgrade
        if (newcoins >= 0)
        {
            CheckSpeed = true;
            StartCoroutine(SaveCurrentcoins(newcoins, Damage, Health, Speed));
        }
        else
        {
            Debug.Log("Not enough coins for the upgrade.");
        }
    }

    IEnumerator SaveCurrentcoins(int currentcoins, int damage, int health, int speed)
    {
        WWWForm form = new WWWForm();
        form.AddField("coins", currentcoins);
        form.AddField("username", DBmanager.username);
        form.AddField("damage", damage);
        form.AddField("health", health);
        form.AddField("speed", speed);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/upgrade.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                if (www.downloadHandler.text.Contains("3"))
                {
                    Debug.Log(www.downloadHandler.text);

                    // Update local variables
                    DBmanager.coins = currentcoins;
                    if (CheckDamage) DBmanager.damage = Damage;
                    if (CheckHealth) DBmanager.health = Health;
                    if (CheckSpeed) DBmanager.speed = Speed;

                    // Update UI
                    UpdateUpgradeTexts();

                    // Reset checks
                    CheckDamage = false;
                    CheckHealth = false;
                    CheckSpeed = false;
                }
                else
                {
                    Debug.Log(www.downloadHandler.text);
                }
            }
            else
            {
                Debug.Log("Failed to connect to the server. Error: " + www.error);
            }
        }
    }
}
