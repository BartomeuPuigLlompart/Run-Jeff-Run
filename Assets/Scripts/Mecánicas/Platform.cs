using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private Vector2 start, end;
    private bool hasSwipedDown = false;

    private PlatformEffector2D effector;

    // Start is called before the first frame update
    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        effector.rotationalOffset = 
            (Input.GetKey(KeyCode.DownArrow) || hasSwipedDown) 
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
