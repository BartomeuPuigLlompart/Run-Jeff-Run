using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private PlatformEffector2D effector;
    [SerializeField] private float waitTime;

    // Start is called before the first frame update
    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.DownArrow))
        {
            waitTime = .1f;
        }

        if(Input.GetKey(KeyCode.DownArrow))
        {
            if(waitTime <= 0)
            {
                effector.rotationalOffset = 180f;
                waitTime = .1f;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }

        if(Input.GetButton("Jump"))
        {
            effector.rotationalOffset = 0;
        }
    }
}
