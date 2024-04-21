using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainmenuNavigation : MonoBehaviour
{
    public void GoToAccount()
    {
        SceneManager.LoadScene(3);//Gaan naar account Scene (3) (buildsettings in unity) file --> buildsettings)
        
    }
    public void GoToLeaderboard()
    {
        SceneManager.LoadScene(4);//Gaan naar Leaderboard Scene (4) (buildsettings in unity) file --> buildsettings)
    }
    public void GoToUpgrades()
    {
        SceneManager.LoadScene(5);//Gaan naar Upgrade Scene (5) (buildsettings in unity) file --> buildsettings)
    }
    public void GoToLevel1()
    {
        SceneManager.LoadScene(6);//Gaan naar Level1  (6) (buildsettings in unity) file --> buildsettings)
    }
}
