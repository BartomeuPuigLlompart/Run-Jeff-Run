using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    private Vector2 start, end;
    private bool hasSwipedDown = false;

    [Range(1, 50)]
    [SerializeField] private float jumpVelocity = 5f;
    [SerializeField] private float gravityExtra = 9.8f;

    [Range(1, 50)]
    [SerializeField] private float dashDownVelocity = 5f;
    private float dashTime;
    [SerializeField] private float startDashTime;
    private bool dashing;

    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private bool grounded;
    private bool oldGrounded;

    Rigidbody2D rb;
    [SerializeField] private GameObject currPlatform;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        dashTime = startDashTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") || HasTapped())
        {
            DoJump();
        }

        if (Input.GetKey(KeyCode.DownArrow) || hasSwipedDown)
        {
            dashing = true;
        }

        if(dashing)
        {
            DoDownDash();
        }

        if (!IsGrounded())
        {
            Vector2 vel = rb.velocity;

            vel.y -= gravityExtra * Time.deltaTime;
            rb.velocity = vel;
        }

        grounded = IsGrounded();
        oldGrounded = grounded;
    }

    void DoJump()
    {
        if(!IsGrounded())
        {
            return;
        }
        else
        {
            rb.velocity = Vector2.up * jumpVelocity;
        }
    }

    void DoDownDash()
    {
        if(dashTime <= 0)
        {
            dashing = false;
            dashTime = startDashTime;
        }
        else
        {
            dashTime -= Time.deltaTime;
            rb.velocity = Vector2.down * dashDownVelocity;
        }
        
    }

    bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1.0f;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }

    public bool IsDashing()
    {
        return dashing;
    }

    bool HasTapped()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    }

    void CheckSwipe()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            start = Input.GetTouch(0).position;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            end = Input.GetTouch(0).position;

            hasSwipedDown = end.y > start.y;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "FloatingPlatform" && IsGrounded() && oldGrounded) currPlatform = collision.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "FloatingPlatform") currPlatform = null;
    }
}
