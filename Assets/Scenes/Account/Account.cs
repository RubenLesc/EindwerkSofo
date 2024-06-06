using System.Collections; // Importeer de namespace voor het gebruik van IEnumerator en andere collectiegerelateerde klassen
using UnityEngine; // Importeer de Unity Engine API
using UnityEngine.Networking; // Importeer de Unity Networking API
using UnityEngine.UI; // Importeer de Unity UI API
using UnityEngine.SceneManagement; // Importeer de Unity Scene Management API

public class Account : MonoBehaviour
{
    public Text Username;
    public Text Level1;
    public Button Logout;
    public InputField currentPasswordInput; 
    public InputField newPasswordInput;
    public InputField confirmPasswordInput; 
    public Text error; 

    public void GotoMain()
    {
        SceneManager.LoadScene(2); // Laad de hoofdscene
    }

    
    public void Awake()
    {
        // Controleer of er een gebruikersnaam is opgeslagen, zo niet, laad dan de inlogscene, anders laad de hoofdscene
        string StrUsername = DBmanager.username;
        if (StrUsername == null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            // Zet de gebruikersnaam in de UI Text en haal de scores op
            StrUsername = StrUsername.ToUpper();
            Username.text = "Gebruikersnaam: " + StrUsername;
            StartCoroutine(PersonalLeaderboard());
        }
    }

    // Methode voor het wijzigen van het wachtwoord
    public void ChangePassword()
    {
        string username = DBmanager.username;
        string currentPassword = currentPasswordInput.text;
        string newPassword = newPasswordInput.text;
        string confirmPassword = confirmPasswordInput.text;

        // Start de coroutine voor het wijzigen van het wachtwoord
        StartCoroutine(ChangePasswordCoroutine(username, currentPassword, newPassword, confirmPassword));
    }

    // Coroutine voor het daadwerkelijke wijzigen van het wachtwoord
    private IEnumerator ChangePasswordCoroutine(string username, string currentPassword, string newPassword, string confirmPassword)
    {
        // Maak een formulier aan voor het verzenden van gegevens
        WWWForm form = new WWWForm();
        form.AddField("name", username);
        form.AddField("current_password", currentPassword);
        form.AddField("new_password", newPassword);
        form.AddField("confirm_password", confirmPassword);

        //communicatie met de php bestanden
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/changepassword.php", form))
        {
            Debug.Log("Verzoek verzenden");
            yield return www.SendWebRequest(); // Wacht op de voltooiing van het verzoek

            // Controleer of het verzoek succesvol was
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Verzoek mislukt: " + www.error);
                error.text = "Fout: " + www.error; // Toon eventuele foutmeldingen
            }
            else
            {
                string responseText = www.downloadHandler.text;
                Debug.Log("Reactie: " + responseText);
            }
        }
    }

    //ophalen van persoonlijke scores
    IEnumerator PersonalLeaderboard()
    {
        // Maak een formulier aan voor het verzenden van gegevens
        WWWForm form = new WWWForm();
        form.AddField("username", DBmanager.username);
        form.AddField("id", DBmanager.playerId);

        //communicatie met de php bestanden
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/personalrecords.php", form))
        {
            yield return www.SendWebRequest(); // Wacht op de voltooiing van het verzoek

            // Controleer of het verzoek succesvol was
            if (www.result == UnityWebRequest.Result.ConnectionError ||
                www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
                yield break;
            }

            // Verwerk de ontvangen gegevens
            string response = www.downloadHandler.text;
            string[] times = response.Split('|');

            if (times.Length >= 4)
            {
                string level1Time = times[0].Trim();
                string level2Time = times[1].Trim();
                string level3Time = times[2].Trim();
                string level4Time = times[3].Trim();

                Debug.Log(level1Time);
                Debug.Log(level2Time);
                Debug.Log(level3Time);
                Debug.Log(level4Time);

                // Toon de scores in de UI Text
                Level1.text = level1Time + "\n\n" + level2Time + "\n\n" + level3Time + "\n\n" + level4Time;
            }
            else
            {
                Debug.LogError("Ongeldig responsformaat");
            }
        }
    }

    // Methode voor uitloggen
    public void Loggout()
    {
        DBmanager.LogOut(); // Log de gebruiker uit
        SceneManager.LoadScene(0); // Laad de inlogscene
    }
}
