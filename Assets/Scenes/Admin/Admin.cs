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
    public string url2 = "http://localhost/sqlconnect/adminupdate.php";
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
        if (DBmanager.username == null)
        {
            SceneManager.LoadScene(0); // Go to login if no username
        }
        else
        {
            StartCoroutine(GetUsers()); // Fetch users from server
            coinsInputField.onValueChanged.AddListener(delegate { ValidateInput(); }); // Validate coins input
        }
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
                        users.Add(new User(username, adminn, coins)); // Add user to list
                    }
                }
            }

            List<string> usernames = new List<string>();
            foreach (var user in users)
            {
                usernames.Add(user.username);
            }

            usernameDropdown.ClearOptions();
            usernameDropdown.AddOptions(usernames); // Populate username dropdown
            deleteDropdown.ClearOptions();
            deleteDropdown.AddOptions(new List<string> { "Keep account", "Delete account" }); // Options for delete dropdown

            usernameDropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(usernameDropdown); });
            deleteDropdown.onValueChanged.AddListener(delegate { HandleDeleteDropdownChange(); });

            DropdownValueChanged(usernameDropdown); // Set initial values
        }
    }

    void DropdownValueChanged(Dropdown change)
    {
        int selectedIndex = change.value;
        User selectedUser = users[selectedIndex];
        adminToggle.isOn = selectedUser.adminn == 1; // Set admin toggle
        coinsInputField.text = selectedUser.coins.ToString(); // Set coins input
    }

    void HandleDeleteDropdownChange()
    {
        if (deleteDropdown.value == 1) // If "Delete account" selected
        {
            string selectedUsername = usernameDropdown.options[usernameDropdown.value].text;
            StartCoroutine(UpdateUsers(selectedUsername, "Delete account")); // Start delete user process
        }
    }

    public void HandleButtonClick()
    {
        if (newPasswordInputField.text == confirmPasswordInputField.text) // Check password match
        {
            string selectedOption = deleteDropdown.options[deleteDropdown.value].text;
            if (selectedOption == "Delete account")
            {
                string selectedUsername = usernameDropdown.options[usernameDropdown.value].text;
                StartCoroutine(UpdateUsers(selectedUsername, "Delete account")); // Delete user
            }
            else if (selectedOption == "Keep account")
            {
                string selectedUsername = usernameDropdown.options[usernameDropdown.value].text;
                int newAdminStatus = adminToggle.isOn ? 1 : 0;
                int newCoins = int.Parse(coinsInputField.text);
                string newPassword = newPasswordInputField.text;

                if (!string.IsNullOrEmpty(newPassword) && newPassword.Length < 5) // Validate new password
                {
                    feedbackText.text = "New password must be at least 5 characters long.";
                    return;
                }

                StartCoroutine(UpdateUsers(selectedUsername, "Keep account", newPassword, newAdminStatus, newCoins)); // Update user
            }
        }
        else
        {
            feedbackText.text = "Passwords are not the same"; // Password mismatch feedback
        }
    }

    IEnumerator UpdateUsers(string username, string selectedOption, string newPassword = "", int newAdminStatus = -1, int newCoins = -1)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("selectedOption", selectedOption);

        if (!string.IsNullOrEmpty(newPassword) && newPassword.Length >= 5)
        {
            form.AddField("newPassword", newPassword); // Add new password
        }

        if (newAdminStatus != -1)
        {
            form.AddField("adminStatus", newAdminStatus); // Add new admin status
        }
        if (newCoins != -1)
        {
            form.AddField("coins", newCoins); // Add new coins value
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
        text = Regex.Replace(text, "[^0-9]", ""); // Remove non-digits

        if (text.Length > 6) // Limit to 6 characters
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
        SceneManager.LoadScene(2); // Go to main menu
    }
}
