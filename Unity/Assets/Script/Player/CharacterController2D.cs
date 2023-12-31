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

    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = gravityScale;
    }

    // private void Update()
    // {
    //     if (DialogueManager.GetInstance().dialogueIsPlaying)
    //     {
    //         return;
    //     }

    //     HandleMovement();
    //     UpdateAnimator();
    // }

    private void FixedUpdate()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }
        HandleMovement();
        UpdateAnimator();
    }

    private void HandleMovement()
    {
        // Vector2 moveDirection = InputManager.GetInstance().GetMoveDirection();
        // Vector2 velocity = new Vector2(moveDirection.x * runSpeed, moveDirection.y * runSpeed);
        // rb.velocity = velocity;
        Vector2 moveDirection = InputManager.GetInstance().GetMoveDirection();
        rb.velocity = new Vector2(moveDirection.x * runSpeed,moveDirection.y * runSpeed);
    }

    private void UpdateAnimator()
    {
        Vector2 moveDirection = InputManager.GetInstance().GetMoveDirection();

        // Set Speed parameter based on horizontal or vertical movement
        // float speed = Mathf.Max(Mathf.Abs(moveDirection.x), Mathf.Abs(moveDirection.y));
        // animator.SetFloat("Speed", speed);

        // Set facing direction parameters
        if (moveDirection.x > 0)
        {
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
    }
}
