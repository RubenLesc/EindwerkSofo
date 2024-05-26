using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Help : MonoBehaviour
{
    public void GotoMain()
    {
        SceneManager.LoadScene(2); // Go to main menu scene (2) (build settings in Unity) File -> Build Settings)
    }
}
