using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Timers;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{   

    //gameobjects to see where the enemy switches direction (patrol points)
    [SerializeField] private Transform LeftEdge;
    [SerializeField] private Transform RightEdge;
    //enemy
    [SerializeField] private Transform Enemy;

    //movement speed
    [SerializeField] private float Speed;
    //sees which direction enemy is facing
    private Vector3 initScale;
    //looking for when the enemy is moving left or right
    private bool movingLeft;

    //idletime
    [SerializeField] private float IdleDuration;
    private float idleTimer;

    //animator (mushroom)
    [SerializeField] private Animator anim;

    private void Awake()
    {
        initScale = Enemy.localScale;
        
    }

    private void OnDisable()
    {   //set moving false when enemy patrol get disabled (on disable --> get run when an object get disabled or destroyed)
        anim.SetBool("Moving", false);
    }


    private void Update()
    {
        if (movingLeft)
        {   //checks if enemy passes the leftedge gameobject to see when it has to switch direction
            if (Enemy.position.x >= LeftEdge.position.x)
            {
                Movedirection(-1);
            }
            else
            {
                //change direction
                ChangeDirection();
                
            }
        }
        else
        {   //checks if enemy passes the rightedge gameobject to see when it has to switch direction
            if (Enemy.position.x <= RightEdge.position.x)
            {
                Movedirection(1);
            }
            else
            {
                //change direction
                ChangeDirection();
            }
        }
    }
    private void ChangeDirection()
    {
        anim.SetBool("Moving", false);

        idleTimer += Time.deltaTime;
        if(idleTimer > IdleDuration)
            movingLeft = !movingLeft;
        

    }
    private void Movedirection(int _direction)
    {
        idleTimer = 0;
        anim.SetBool("Moving", true);
        //make enemy look in direction
        Enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction, initScale.y, initScale.z);
        //make enemy move in direction
        Enemy.position = new Vector3(Enemy.position.x + Time.deltaTime * _direction * Speed, Enemy.position.y, Enemy.position.z);
    }
}
