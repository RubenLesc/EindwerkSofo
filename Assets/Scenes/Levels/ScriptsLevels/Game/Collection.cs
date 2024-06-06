using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection : MonoBehaviour
{
    public GameManager GameManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {   

        //collect coins
        if (collision.name == "Player")
        {
            Debug.Log("Collected");
            GameManager.CollectCoins();
            Destroy(gameObject);
        }
    }
    

}
