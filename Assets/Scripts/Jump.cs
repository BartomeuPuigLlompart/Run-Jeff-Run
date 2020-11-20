using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [Range(1, 20)]
    [SerializeField] private float jumpVelocity = 5f;
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
            GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpVelocity;
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
}
