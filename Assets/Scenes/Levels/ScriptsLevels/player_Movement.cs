using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player_Movement : MonoBehaviour
{
    private Rigidbody2D Player;
    private BoxCollider2D BoxCollider;
    [SerializeField] private LayerMask jumpableground;

    // Start is called before the first frame update
    private void Start()
    {
        
       Player= GetComponent<Rigidbody2D>();
       BoxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        float directionX = Input.GetAxis("Horizontal");    //waarde van de horizontale as

        Player.velocity = new Vector2(7f * directionX, Player.velocity.y); //snelheid van de 'Player' Rigidbody2D (horizontaal bewegen)


        if (Input.GetButtonDown("Jump") && IsGrounded()) //Controleert of de "Jump" -knop is ingedrukt en al de player op de grond staat
        {
            
            Player.velocity = new Vector3(Player.velocity.x, 14, 0); //snelheid van de 'Player' Rigidbody2D (verticaal springen)
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SceneManager.LoadScene(2);
        }
    }
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(BoxCollider.bounds.center, BoxCollider.bounds.size, 0f, Vector2.down, .1f, jumpableground);
    }
    
    
}
