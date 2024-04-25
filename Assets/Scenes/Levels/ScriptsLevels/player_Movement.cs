using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player_Movement : MonoBehaviour
{   
    //declareren
    private Rigidbody2D Player;
    private BoxCollider2D BoxCollider;
    [SerializeField] private LayerMask jumpableground;
    public Animator animator;
    private SpriteRenderer sprite;
    float directionX;
    //speed and jump
    [SerializeField] float MovementSpeed = 7f;
    [SerializeField] float JumpForce = 14f;

    private enum MovementState {idle, walking, jumping }
    private MovementState state;

    // Start is called before the first frame update
    private void Start()
    {
        
       Player= GetComponent<Rigidbody2D>();
       BoxCollider = GetComponent<BoxCollider2D>();
       sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        directionX = Input.GetAxis("Horizontal");    //waarde van de horizontale as
        Player.velocity = new Vector2(MovementSpeed * directionX, Player.velocity.y); //snelheid van de 'Player' Rigidbody2D (horizontaal bewegen)
        if (Input.GetButtonDown("Jump") && IsGrounded()) //Controleert of de "Jump" -knop is ingedrukt en al de player op de grond staat
        {
            
            Player.velocity = new Vector3(Player.velocity.x, JumpForce, 0); //snelheid van de 'Player' Rigidbody2D (verticaal springen)
            

        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SceneManager.LoadScene(2);
        }
        UpdateAnimation();
    }
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(BoxCollider.bounds.center, BoxCollider.bounds.size, 0f, Vector2.down, .1f, jumpableground);
    }

    private void UpdateAnimation()
    {
        MovementState state;
        if (directionX > 0f )
        {
            state = MovementState.walking;
            sprite.flipX = false;
            
        }
        else if (directionX < 0f)
        {
            state = MovementState.walking;
            sprite.flipX = true;
           
        }
        else
        {
            state = MovementState.idle;
            
        }
        if (Player.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (Player.velocity.y < -.1f)
        {
            state = MovementState.jumping;
        }


        animator.SetInteger("MovementState",(int) state);

    }
    
    
}

