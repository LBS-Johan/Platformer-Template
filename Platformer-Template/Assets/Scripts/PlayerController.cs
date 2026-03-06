using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    [SerializeField] float xSpeed = 4;
    [SerializeField] float jumpPower = 10;

    Collider2D playerCollider;

    Animator animator;

    public LayerMask groundLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        if(rb == null)
        {
            Debug.Log("Rigidbody 2D is missing. Please add one to the same gameobject as this PlayerController");
        }
        if (sr == null)
        {
            Debug.Log("SpriteRenderer is missing. Please add one to the same gameobject as this PlayerController");
        }
        if (playerCollider == null)
        {
            Debug.Log("Collider 2D is missing. Please add one to the same gameobject as this PlayerController");
        }

        if(animator == null)
        {
            Debug.Log("Animator is missing. Please add one to the same gameobject as this PlayerController to enable animations");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rb == null )
        {
            return;
        }

        Vector2 direction = Vector2.zero;

        if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
        {
            direction.x = xSpeed;
        }
        if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
        {
            direction.x = -xSpeed;
        }
        if ((Keyboard.current.upArrowKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame) && isGrounded())
        {
            direction.y = jumpPower;
        }
        else
        {
            direction.y = rb.linearVelocity.y;
        }

        if ((Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed || Keyboard.current.spaceKey.isPressed) && direction.y > 0)
        {
            rb.gravityScale = 1.5f;
        }
        else
        {
            rb.gravityScale = 4.5f;
        }

        rb.linearVelocity = direction;

        if(sr != null)
        {
            if(direction.x > 0)
            {
                sr.flipX = false;
            }
            if(direction.x < 0)
            {
                sr.flipX = true;
            }
        }

        if(animator == null)
        {
            return;
        }

        

        if(direction.x != 0 && isGrounded())
        {
            animator.Play("run");
        }
        else if (isGrounded())
        {
            animator.Play("idle");
        }
        else 
        {
            animator.Play("jump");
        }
    }

    bool isGrounded()
    {
        if (groundLayerMask == 0 || playerCollider == null)
        {
            return true;
        }

        RaycastHit2D leftHit = Physics2D.Raycast(playerCollider.bounds.min, Vector2.down, 0.3f, groundLayerMask);
        RaycastHit2D rightHit = Physics2D.Raycast(new Vector2(playerCollider.bounds.max.x, playerCollider.bounds.min.y), Vector2.down, 0.3f, groundLayerMask);


        return leftHit || rightHit;

    }
}