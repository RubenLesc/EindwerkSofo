using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class Admin : MonoBehaviour
{
    public string url = "http://localhost/sqlconnect/adminloading.php";
    public string url2 = "http://localhost/sqlconnect/adminupdate.php"; // URL for update/delete PHP script
    public Dropdown usernameDropdown;
    public Toggle adminToggle;
    public Dropdown deleteDropdown;
    public InputField coinsInputField;
    public InputField newPasswordInputField; 
    public InputField confirmPasswordInputField; 
    public Text feedbackText; 

    private List<User> users = new List<User>();

    void Start()
    {
        StartCoroutine(GetUsers());
        coinsInputField.onValueChanged.AddListener(delegate { ValidateInput(); });
    }

    IEnumerator GetUsers()
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + www.error);
        }
        else
        {
            // Parse response
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
                    int coins = int.Parse(fields[2]); // Parse coins
                    if (username.ToLower() != "admin") // Exclude "admin" user
                    {
                        users.Add(new User(username, adminn, coins));
                    }
                }
            }

            // Populate dropdowns
            List<string> usernames = new List<string>();
            foreach (var user in users)
            {
                usernames.Add(user.username);
            }

            usernameDropdown.ClearOptions();
            usernameDropdown.AddOptions(usernames);
            deleteDropdown.ClearOptions(); // Clear options for delete dropdown
            deleteDropdown.AddOptions(new List<string> { "Keep account", "Delete account" }); // Add options to delete dropdown

            // Add listeners for dropdown value changes
            usernameDropdown.onValueChanged.AddListener(delegate {
                DropdownValueChanged(usernameDropdown);
            });

            deleteDropdown.onValueChanged.AddListener(delegate {
                HandleDeleteDropdownChange();
            });

            // Set initial state for admin toggle and coins input field
            DropdownValueChanged(usernameDropdown);
        }
    }

    void DropdownValueChanged(Dropdown change)
    {
        int selectedIndex = change.value;
        User selectedUser = users[selectedIndex];
        adminToggle.isOn = selectedUser.adminn == 1;
        coinsInputField.text = selectedUser.coins.ToString();
    }

    // Method to handle delete dropdown value change
    void HandleDeleteDropdownChange()
    {
        if (deleteDropdown.value == 1) // If "Delete account" option is selected
        {
            string selectedUsername = usernameDropdown.options[usernameDropdown.value].text;
            StartCoroutine(UpdateUsers(selectedUsername, "Delete account"));
        }
    }

    // Method to handle button click event
    public void HandleButtonClick()
    {   
        if (newPasswordInputField.text == confirmPasswordInputField.text)
        {
            string selectedOption = deleteDropdown.options[deleteDropdown.value].text;
            if (selectedOption == "Delete account")
            {
                string selectedUsername = usernameDropdown.options[usernameDropdown.value].text;
                StartCoroutine(UpdateUsers(selectedUsername, "Delete account"));
            }
            else if (selectedOption == "Keep account")
            {
                string selectedUsername = usernameDropdown.options[usernameDropdown.value].text;
                int newAdminStatus = adminToggle.isOn ? 1 : 0;
                int newCoins = int.Parse(coinsInputField.text);
                string newPassword = newPasswordInputField.text; // Get new password from input field

                // Validate the new password if it is provided
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
            feedbackText.text = "Passwords are not the same";
        }

        
    }

    IEnumerator UpdateUsers(string username, string selectedOption, string newPassword = "", int newAdminStatus = -1, int newCoins = -1)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("selectedOption", selectedOption);

        // Add the new password field only if it's provided and valid
        if (!string.IsNullOrEmpty(newPassword) && newPassword.Length >= 5)
        {
            form.AddField("newPassword", newPassword);
        }

        if (newAdminStatus != -1)
        {
            form.AddField("adminStatus", newAdminStatus);
        }
        if (newCoins != -1)
        {
            form.AddField("coins", newCoins);
        }

        using (UnityWebRequest www = UnityWebRequest.Post(url2, form))
        {
            yield return www.SendWebRequest();

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
        // Remove any non-digit characters from the input
        text = Regex.Replace(text, "[^0-9]", "");
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
        //admin until you log out (everyone but admin account)
        SceneManager.LoadScene(2); // Go to main menu scene (2) (build settings in Unity) File -> Build Settings)
    }
}
