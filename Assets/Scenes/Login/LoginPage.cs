using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginPage : MonoBehaviour
{
    
    public void GotoRegister()
    {
        SceneManager.LoadScene(1); // naar register pagina gaan
    }
    private void Start()
    {
        if (DBmanager.LoggedIn)
        {
            
            SceneManager.LoadScene(2);
        }
    }

    
    

}
