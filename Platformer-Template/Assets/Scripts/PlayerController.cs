using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float xSpeed = 4;
    Collider2D collider;

    Animator animator;

    public LayerMask groundLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        if(rb == null)
        {
            Debug.Log("Rigidbody 2D is missing. Please add one to the same gameobject as this PlayerController");
        }
        if (rb == collider)
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

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            direction.x = xSpeed;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            direction.x = -xSpeed;
        }
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) ||  Input.GetKey(KeyCode.Space)) && isGrounded())
        {
            direction.y = 10;
        }
        else
        {
            direction.y = rb.linearVelocity.y;
        }

        if (Input.GetKey(KeyCode.UpArrow) && direction.y > 0)
        {
            rb.gravityScale = 1.5f;
        }
        else
        {
            rb.gravityScale = 4.5f;
        }

        rb.linearVelocity = direction;

        if(animator == null)
        {
            return;
        }

        

        if(direction.x != 0 && isGrounded() && animator.GetLayerIndex("run") != -1)
        {
            animator.Play("run");
        }
        else if (isGrounded() && animator.GetLayerIndex("idle") != -1)
        {
            animator.Play("idle");
        }
        else if(animator.GetLayerIndex("jump")!=-1)
        {
            animator.Play("jump");
        }
    }

    bool isGrounded()
    {
        if (groundLayerMask == 0 || collider == null)
        {
            return true;
        }

        RaycastHit2D leftHit = Physics2D.Raycast(collider.bounds.min, Vector2.down, 0.3f, groundLayerMask);
        RaycastHit2D rightHit = Physics2D.Raycast(new Vector2(collider.bounds.max.x, collider.bounds.min.y), Vector2.down, 0.3f, groundLayerMask);


        return leftHit || rightHit;

    }
}