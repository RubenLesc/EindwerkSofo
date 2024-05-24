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
    [SerializeField] float MovementSpeed = 7f;
    [SerializeField] float JumpForce = 14f;

    private enum MovementState { idle, walking, jumping }
    private MovementState state;

    private bool canMove = true;

    public BoxCollider2D PlayerCollider { get; private set; }

    private void Start()
    {
        PlayerCollider = GetComponent<BoxCollider2D>();
        Player = GetComponent<Rigidbody2D>();
        BoxCollider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!canMove)
            return;

        directionX = Input.GetAxis("Horizontal");
        Player.velocity = new Vector2(MovementSpeed * directionX, Player.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Player.velocity = new Vector3(Player.velocity.x, JumpForce, 0);
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
        if (directionX > 0f)
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
        animator.SetInteger("MovementState", (int)state);
    }

    public void StopMovement()
    {
        canMove = false;
    }

    public void ResumeMovement()
    {
        canMove = true;
    }
}
