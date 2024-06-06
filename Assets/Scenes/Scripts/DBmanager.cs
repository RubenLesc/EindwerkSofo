using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


//static class zodat je DBmanager van overal in je project kan vinden
public static class DBmanager
{   
    //variablen
    public static int playerId;
    public static string username;
    public static int admin;
    public static string password;
    public static string salt;
    public static int coins;
    public static int damage;
    public static int speed;
    public static int health;
    public static float elapsedTime;
    public static int level;
    public static string levelTime1;
    public static string levelTime2;
    public static string levelTime3;
    public static string levelTime4;

    public static bool LoggedIn { get { return username != null; } }

    //logout
    public static void LogOut()
    {
        username = null;
        admin = 0;
        

    }

    
}
