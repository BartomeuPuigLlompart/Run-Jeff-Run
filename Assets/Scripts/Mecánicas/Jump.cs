using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    private Vector2 start, end;
    [SerializeField] private float swipeDistance;
    private bool hasSwipedDown = false;
    private bool hasSwiped = false;

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

    [SerializeField] private AnimationCurve jumpFactor;
    [SerializeField] private AnimationCurve dashDownFactor;

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
        CheckSwipe();

        if (Input.GetButtonDown("Jump") || (hasSwiped && !hasSwipedDown))
        {
            DoJump();
        }

        if (Input.GetKey(KeyCode.DownArrow) || (hasSwiped && hasSwipedDown))
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
        Debug.Log("Jumping");
        if(!IsGrounded())
        {
            return;
        }
        else
        {
            rb.velocity = Vector2.up * jumpVelocity;
        }

        hasSwiped = false;
    }

    void DoDownDash()
    {
        Debug.Log("Dashing");
        if (dashTime <= 0)
        {
            dashing = false;
            dashTime = startDashTime;
        }
        else
        {
            dashTime -= Time.deltaTime;
            rb.velocity = Vector2.down * dashDownVelocity;
        }

        hasSwiped = false;
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
        //Getting the start point of the swipe
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            start = Input.GetTouch(0).position;

        //Getting the end point of the swipe
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            end = Input.GetTouch(0).position;
            swipeDistance = Vector2.Distance(start, end) / Screen.height; //Get the distance to calculate the time/power
            hasSwipedDown = end.y < start.y; //Check if it has to jump or dash
            hasSwiped = true; //Activate the swipe/jump function

            if (hasSwipedDown && !grounded)
            {
                //Max 1s, Min 0,25s
                dashTime = startDashTime = dashDownFactor.Evaluate(swipeDistance);
            }
            else if(!hasSwipedDown && grounded)
            {
                //Max 17, Min 8 
                jumpVelocity = jumpFactor.Evaluate(swipeDistance);
            }
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
