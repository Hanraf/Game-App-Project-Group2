using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    [Header("Movement Params")]
    public float runSpeed = 6.0f;
    public float jumpSpeed = 0f;
    public float gravityScale = 0f;
    public Animator animator;

    private BoxCollider2D coll;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.gravityScale = gravityScale;
    }

    private void FixedUpdate()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            // Stop character movement when dialogue is playing
            rb.velocity = Vector2.zero;
            return;
        }

        HandleMovement();
        UpdateAnimator();
    }

    private void HandleMovement()
    {
        Vector2 moveDirection = InputManager.GetInstance().GetMoveDirection();
        rb.velocity = new Vector2(moveDirection.x * runSpeed, moveDirection.y * runSpeed);
    }

    private void UpdateAnimator()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 moveDirection = InputManager.GetInstance().GetMoveDirection();

        float speed = Mathf.Max(Mathf.Abs(moveDirection.x), Mathf.Abs(moveDirection.y));

        if (moveDirection.x > 0)
        {
            // Set facing direction parameters
            animator.SetBool("IsFacingRight", true);
            animator.SetBool("IsFacingLeft", false);
        }
        else if (moveDirection.x < 0)
        {
            animator.SetBool("IsFacingRight", false);
            animator.SetBool("IsFacingLeft", true);
        }
        else
        {
            animator.SetBool("IsFacingRight", false);
            animator.SetBool("IsFacingLeft", false);
        }

        if (moveDirection.y > 0)
        {
            animator.SetBool("IsFacingUp", true);
            animator.SetBool("IsFacingDown", false);
        }
        else if (moveDirection.y < 0)
        {
            animator.SetBool("IsFacingUp", false);
            animator.SetBool("IsFacingDown", true);
        }
        else
        {
            animator.SetBool("IsFacingUp", false);
            animator.SetBool("IsFacingDown", false);
        }

        // Check if the character is not moving, then stop the animation
        // if (speed == 0)
        // {
        //     animator.SetBool("IsMoving", false);
        // }
        // else
        // {
        //     animator.SetBool("IsMoving", true);
        // }

        // Set sorting order to ensure it's always 2
        spriteRenderer.sortingOrder = 2;
    }
}
