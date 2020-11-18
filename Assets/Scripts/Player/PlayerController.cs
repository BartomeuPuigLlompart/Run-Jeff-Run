using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum states { DEAD, HURT, NORMAL}
    [SerializeField] states state;
    [SerializeField]Transform gameMidRef;
    [SerializeField] float speed;
    const float deadRef = 0.015f;
    const float hurtRef = 0.02f;
    const float normalRef = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case states.NORMAL:
                speed = speed + (normalRef - speed) * 0.01f;
                break;
            case states.HURT:
                speed = hurtRef;
                break;
            case states.DEAD:
                speed = deadRef;
                break;
        }
        transform.position = Vector2.Lerp(transform.position, new Vector2(gameMidRef.position.x, transform.position.y), speed);
    }

    //void hurtplayer()
    //{
    //    switch (state)
    //    {
    //        case states.normal:
    //            speed = normalref;
    //            break;
    //        case states.hurt:
    //            speed = hurtref;
    //            break;
    //        case states.dead:
    //            speed = deadref;
    //            break;
    //    }
    //}
}
