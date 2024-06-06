using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    int Damagecost = 50;
    int Healthcost = 100;
    int Speedcost = 75;
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

    private void Awake()
    {
        if (StrUsername == null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            // Initialize texts
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        txtUsername.text = "Username\n" + StrUsername.ToUpper();
        txtUsername2.text = "Username\n" + StrUsername.ToUpper();
        txtCoins.text = DBmanager.coins.ToString();
        txtCoins2.text = DBmanager.coins.ToString();

        UpdateUpgradeTexts();
    }

    private void UpdateUpgradeTexts()
    {
        UpdateDamageUpgradeText();
        UpdateHealthUpgradeText();
        UpdateSpeedUpgradeText();
    }

    private void UpdateDamageUpgradeText()
    {
        if (DBmanager.damage >= 10)
        {
            txtLevelDamage.text = "Level: " + DBmanager.damage + "\nMax Level";
            txtCostDamage.text = "Max Level";
            btnUpgradeDamage.interactable = false;
        }
        else if (DBmanager.damage == 9)
        {
            txtLevelDamage.text = "Level: " + DBmanager.damage + "\nNext Upgrade: Max Level";
            txtCostDamage.text = "Upgrade\nCost: " + Damagecost;
        }
        else
        {
            txtLevelDamage.text = "Level: " + DBmanager.damage + "\nNext Upgrade: " + (DBmanager.damage + 1) + " damage";
            txtCostDamage.text = "Upgrade\nCost: " + Damagecost;
        }
    }

    private void UpdateHealthUpgradeText()
    {
        if (DBmanager.health >= 19)
        {
            txtLevelHealth.text = "Level: " + DBmanager.health + "\nMax Level";
            txtCostHealth.text = "Max Level";
            btnUpgradeHealth.interactable = false;
        }
        else if (DBmanager.health == 18)
        {
            txtLevelHealth.text = "Level: " + DBmanager.health + "\nNext Upgrade: Max Level";
            txtCostHealth.text = "Upgrade\nCost: " + Healthcost;
        }
        else
        {
            float healthcalc = ((DBmanager.health * 0.5f) + 1f);
            txtLevelHealth.text = "Level: " + DBmanager.health + "\nNext Upgrade: " + healthcalc + " hearts";
            txtCostHealth.text = "Upgrade\nCost: " + Healthcost;
        }
    }

    private void UpdateSpeedUpgradeText()
    {
        if (DBmanager.speed >= 30)
        {
            txtLevelSpeed.text = "Level: " + DBmanager.speed + "\nMax Level";
            txtCostSpeed.text = "Max Level";
            btnUpgradeSpeed.interactable = false;
        }
        else if (DBmanager.speed == 29)
        {
            txtLevelSpeed.text = "Level: " + DBmanager.speed + "\nNext Upgrade: Max Level";
            txtCostSpeed.text = "Upgrade\nCost: " + Speedcost;
        }
        else
        {
            txtLevelSpeed.text = "Level: " + DBmanager.speed + "\nNext Upgrade: " + (DBmanager.speed * 0.2f + 7) + " speed";
            txtCostSpeed.text = "Upgrade\nCost: " + Speedcost;
        }
    }

    public void UpgradeSword()
    {
        if (DBmanager.coins >= Damagecost)
        {
            CheckDamage = true;
            int newDamage = DBmanager.damage + 1;
            int newCoins = DBmanager.coins - Damagecost;

            // Update UI 
            UpdateCoinsLabel(newCoins);

            
            StartCoroutine(SaveCurrentcoins(newCoins, newDamage, DBmanager.health, DBmanager.speed));
        }
        else
        {
            Debug.Log("Not enough coins for the upgrade.");
        }
    }

    public void UpgradeHealth()
    {
        if (DBmanager.coins >= Healthcost)
        {
            CheckHealth = true;
            int newHealth = DBmanager.health + 1;
            int newCoins = DBmanager.coins - Healthcost;

            // Update UI immediately
            UpdateCoinsLabel(newCoins);

            // Start server update coroutine
            StartCoroutine(SaveCurrentcoins(newCoins, DBmanager.damage, newHealth, DBmanager.speed));
        }
        else
        {
            Debug.Log("Not enough coins for the upgrade.");
        }
    }

    public void UpgradeSpeed()
    {
        if (DBmanager.coins >= Speedcost)
        {
            CheckSpeed = true;
            int newSpeed = DBmanager.speed + 1;
            int newCoins = DBmanager.coins - Speedcost;

            // Update UI immediately
            UpdateCoinsLabel(newCoins);

            // Start server update coroutine
            StartCoroutine(SaveCurrentcoins(newCoins, DBmanager.damage, DBmanager.health, newSpeed));
        }
        else
        {
            Debug.Log("Not enough coins for the upgrade.");
        }
    }

    private void UpdateCoinsLabel(int newCoins)
    {
        txtCoins.text = newCoins.ToString();
        txtCoins2.text = newCoins.ToString();
    }

    IEnumerator SaveCurrentcoins(int newCoins, int newDamage, int newHealth, int newSpeed)
    {
        WWWForm form = new WWWForm();
        form.AddField("coins", newCoins);
        form.AddField("username", DBmanager.username);
        form.AddField("damage", newDamage);
        form.AddField("health", newHealth);
        form.AddField("speed", newSpeed);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/upgrade.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                if (www.downloadHandler.text.Contains("3"))
                {
                    Debug.Log(www.downloadHandler.text);

                    // Update local variables
                    DBmanager.coins = newCoins;
                    if (CheckDamage) DBmanager.damage = newDamage;
                    if (CheckHealth) DBmanager.health = newHealth;
                    if (CheckSpeed) DBmanager.speed = newSpeed;

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

    public void GotoMain()
    {
        SceneManager.LoadScene(2); // Go to main menu Scene (2) (build settings in Unity)
    }
}
