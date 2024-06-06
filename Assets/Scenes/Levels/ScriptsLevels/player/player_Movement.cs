using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player_Movement : MonoBehaviour
{   
    private Rigidbody2D Player;
    private BoxCollider2D BoxCollider;
    [SerializeField] private LayerMask jumpableground;
    public Animator animator;
    private SpriteRenderer sprite;
    float directionX;
    float MovementSpeed = (DBmanager.speed * 0.2f + 7);
    [SerializeField] float JumpForce = 14f;

    private enum MovementState { idle, walking, jumping }
    private MovementState state;

    private bool canMove = true;
    private bool facingRight = true;

    public BoxCollider2D PlayerCollider { get; private set; }

    private PlayerAttack playerAttack;

    private void Start()
    {   
        //references
        PlayerCollider = GetComponent<BoxCollider2D>();
        Player = GetComponent<Rigidbody2D>();
        BoxCollider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    private void Update()
    {
        if (!canMove)
            return;

        //kijkt welke toets je hebt ingedrukt
        directionX = Input.GetAxis("Horizontal");
        Player.velocity = new Vector2(MovementSpeed * directionX, Player.velocity.y);

        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && IsGrounded())
        {
            Player.velocity = new Vector3(Player.velocity.x, JumpForce, 0);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(2);
        }
        UpdateAnimation();
    }

    private bool IsGrounded()
    {   
        //ben je op de grond
        return Physics2D.BoxCast(BoxCollider.bounds.center, BoxCollider.bounds.size, 0f, Vector2.down, .1f, jumpableground);
    }

    private void UpdateAnimation()
    {   
        //animation
        MovementState state;
        if (directionX > 0f)
        {
            state = MovementState.walking;
            if (!facingRight)
            {
                Flip();
            }
        }
        else if (directionX < 0f)
        {
            state = MovementState.walking;
            if (facingRight)
            {
                Flip();
            }
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
        animator.SetInteger("MovementState", (int)state);
    }

    private void Flip()
    {   
        //verander de richting naar waar de speler kijkt
        facingRight = !facingRight;
        playerAttack.Flip();
        sprite.flipX = !sprite.flipX;
    }

    public void StopMovement()
    {   
        //steft --> velocity 0
        canMove = false;
        Player.velocity = Vector2.zero;
    }

    public void ResumeMovement()
    {
        canMove = true;
    }
}
