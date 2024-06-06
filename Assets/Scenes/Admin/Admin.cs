
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class Admin : MonoBehaviour
{
    //urls naar bestanden in sqlconnect
    public string url = "http://localhost/sqlconnect/adminloading.php";
    public string url2 = "http://localhost/sqlconnect/adminupdate.php";

    // UI elementen
    public Dropdown usernameDropdown;
    public Toggle adminToggle;
    public Dropdown deleteDropdown;
    public InputField coinsInputField;
    public InputField newPasswordInputField;
    public InputField confirmPasswordInputField;
    public Text feedbackText;

    // lijst voor gebruikers
    private List<User> users = new List<User>();

    // Start methode
    void Start()
    {
        //kijkt als je bent ingelogd
        if (DBmanager.username == null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            StartCoroutine(GetUsers()); //starten van methode voor de gegevens voor intevullen
            coinsInputField.onValueChanged.AddListener(delegate { ValidateInput(); });
        }
    }

    
    IEnumerator GetUsers()
    {
        // krijg gebruikers
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        // antwoord
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + www.error);
        }
        else
        {
            // vull de dropdown menus in
            string data = www.downloadHandler.text;
            string[] entries = data.Split(';');

            users.Clear();
            foreach (string entry in entries)
            {
                string[] fields = entry.Split(',');
                if (fields.Length == 3)
                {
                    string username = fields[0];
                    int adminn = int.Parse(fields[1]);
                    int coins = int.Parse(fields[2]);
                    if (username.ToLower() != "admin")
                    {
                        users.Add(new User(username, adminn, coins));
                    }
                }
            }

            // vull dropdown in met gegevens
            List<string> usernames = new List<string>();
            foreach (var user in users)
            {
                usernames.Add(user.username);
            }

            usernameDropdown.ClearOptions();
            usernameDropdown.AddOptions(usernames);
            deleteDropdown.ClearOptions();
            deleteDropdown.AddOptions(new List<string> { "Keep account", "Delete account" });

            // Set initial dropdown values
            usernameDropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(usernameDropdown); });
            deleteDropdown.onValueChanged.AddListener(delegate { HandleDeleteDropdownChange(); });

            DropdownValueChanged(usernameDropdown);
        }
    }

    // Methode als verandering van dropdown
    void DropdownValueChanged(Dropdown change)
    {
        int selectedIndex = change.value;
        User selectedUser = users[selectedIndex];
        adminToggle.isOn = selectedUser.adminn == 1; //admin toggle
        coinsInputField.text = selectedUser.coins.ToString(); //coins input
    }

    // Methode voor delete dropdonw
    void HandleDeleteDropdownChange()
    {
        if (deleteDropdown.value == 1)
        {
            string selectedUsername = usernameDropdown.options[usernameDropdown.value].text;
            StartCoroutine(UpdateUsers(selectedUsername, "Delete account"));
        }
    }

    
    public void HandleButtonClick()
    {
        if (newPasswordInputField.text == confirmPasswordInputField.text) // Check wachtwoord match
        {
            string selectedOption = deleteDropdown.options[deleteDropdown.value].text;
            if (selectedOption == "Delete account")
            {
                string selectedUsername = usernameDropdown.options[usernameDropdown.value].text;
                StartCoroutine(UpdateUsers(selectedUsername, "Delete account")); // verwijder
            }
            else if (selectedOption == "Keep account")
            {
                // Update gebruiker info
                string selectedUsername = usernameDropdown.options[usernameDropdown.value].text;
                int newAdminStatus = adminToggle.isOn ? 1 : 0;
                int newCoins = int.Parse(coinsInputField.text);
                string newPassword = newPasswordInputField.text;

                //check new wachtwoord
                if (!string.IsNullOrEmpty(newPassword) && newPassword.Length < 5)
                {
                    feedbackText.text = "New password must be at least 5 characters long.";
                    return;
                }

                
                StartCoroutine(UpdateUsers(selectedUsername, "Keep account", newPassword, newAdminStatus, newCoins));
            }
        }
        else
        {
            feedbackText.text = "Passwords are not the same"; //is niet gelijk passwoord
        }
    }


    IEnumerator UpdateUsers(string username, string selectedOption, string newPassword = "", int newAdminStatus = -1, int newCoins = -1)
    {
        //form voor post
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("selectedOption", selectedOption);

        //paswoord toevoegen als ingevuld
        if (!string.IsNullOrEmpty(newPassword) && newPassword.Length >= 5)
        {
            form.AddField("newPassword", newPassword);
        }

        //admin
        if (newAdminStatus != -1)
        {
            form.AddField("adminStatus", newAdminStatus);
        }
        //coins
        if (newCoins != -1)
        {
            form.AddField("coins", newCoins);
        }

        //update
        using (UnityWebRequest www = UnityWebRequest.Post(url2, form))
        {
            yield return www.SendWebRequest();

            // Handle server response
            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Update successful!");
                feedbackText.text = "Update successful!";
            }
            else
            {
                Debug.LogError("Error updating user: " + www.error);
                feedbackText.text = "Error updating user: " + www.error;
            }
        }
    }


    void ValidateInput()
    {
        string text = coinsInputField.text;
        text = Regex.Replace(text, "[^0-9]", ""); // verwijder geen cijfers

        if (text.Length > 6)
        {
            text = text.Substring(0, 6);
        }
        coinsInputField.text = text;
    }

    public class User
    {
        public string username;
        public int adminn;
        public int coins;


        public User(string username, int adminn, int coins)
        {
            this.username = username;
            this.adminn = adminn;
            this.coins = coins;
        }
    }


    public void GotoMain()
    {
        SceneManager.LoadScene(2); // Ga naar main menu
    }
}
