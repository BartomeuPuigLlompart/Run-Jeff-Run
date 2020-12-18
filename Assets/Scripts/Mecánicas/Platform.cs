using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private Vector2 start, end;
    private bool hasSwipedDown = false;

    private PlatformEffector2D effector;
    private GameObject player;
    private Jump playerJump;

    // Start is called before the first frame update
    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();

        player = GameObject.FindGameObjectWithTag("Player");
        if (player) playerJump = player.GetComponent<Jump>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player) playerJump = player.GetComponent<Jump>();
        effector.rotationalOffset = 
            (playerJump.IsDashing()) 
            ? 180f : 0;
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
}
