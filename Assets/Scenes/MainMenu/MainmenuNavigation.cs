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
        DBmanager.level = 1;
        SceneManager.LoadScene(6);//Gaan naar Level1  (6) (buildsettings in unity) file --> buildsettings)
    }
    public void GoToLevel2()
    {
        DBmanager.level = 2;
        SceneManager.LoadScene(9);//Gaan naar Level2  (9) (buildsettings in unity) file --> buildsettings)
    }
    public void GoToLevel3()
    {
        DBmanager.level = 3;
        SceneManager.LoadScene(10);//Gaan naar Level3  (10) (buildsettings in unity) file --> buildsettings)
    }
    public void GoToLevel4()
    {
        DBmanager.level = 4;
        SceneManager.LoadScene(11);//Gaan naar Level4  (11) (buildsettings in unity) file --> buildsettings)
    }
    public void GoToHelp()
    {
        SceneManager.LoadScene(7);//Gaan naar Controls Pagina  (7) (buildsettings in unity) file --> buildsettings)
    }
    public void GoToAdmin()
    {
        SceneManager.LoadScene(8);//Gaan naar Admin Pagina  (7) (buildsettings in unity) file --> buildsettings)
    }
}
