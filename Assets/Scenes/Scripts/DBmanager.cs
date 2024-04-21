using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


//static class zodat je DBmanager van overal in je project kan vinden
public static class DBmanager
{
    public static string username;
    public static int admin;
    public static string password;
    public static string salt;
    public static int coins;
    public static int damage;
    public static int speed;
    public static int health;

    public static bool LoggedIn { get { return username != null; } }

    public static void LogOut()
    {
        username = null;
        admin = 0;
        

    }

    
}
