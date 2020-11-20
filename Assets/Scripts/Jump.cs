using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [Range(1, 20)]
    [SerializeField] private float jumpVelocity = 5f;
    [SerializeField] private float gravityExtra = 9.8f;

    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private bool isGrounded;
    //[SerializeField] private float fallMultiplier = 2.5f;
    //[SerializeField] private float lowJumpMultiplier = 2f;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            DoJump();
        }

        if(!IsGrounded())
        {
            Vector2 vel = rb.velocity;

            vel.y -= gravityExtra * Time.deltaTime;
            rb.velocity = vel;
        }

        isGrounded = IsGrounded();
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

    //private void FixedUpdate()
    //{
    //    if(rb.velocity.y < 0)
    //    {
    //        rb.gravityScale = fallMultiplier;
    //    }
    //    else if(rb.velocity.y > 0 && !Input.GetButton("Jump"))
    //    {
    //        rb.gravityScale = lowJumpMultiplier;
    //    }
    //    else
    //    {
    //        rb.gravityScale = 1f;
    //    }
    //}

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
}
