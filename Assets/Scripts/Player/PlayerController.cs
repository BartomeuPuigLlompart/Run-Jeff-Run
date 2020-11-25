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
    float inmunityRef = 0.0f;
    List<Vector2> trail;
    public int framesRef;
    public  int finalFrames = 50;
    public static PlayerController playerController;

    private void Awake()
    {
        playerController = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        trail = new List<Vector2>();
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

        trail.Add(transform.position);
    }

    public void hurtplayer()
    {
        transform.position += Vector3.up;
        if (inmunityRef + 1.0f < Time.time)
        {
            inmunityRef = Time.time;
            //state--;
        }
    }

    public Vector2 checkTrail(Vector2 pos)
    {
        if (state == states.HURT) framesRef /= 2;
        while (trail.Count > framesRef)
        {
            pos = trail[0];
            trail.Remove(trail[0]);
        }
        return pos;
    }
}
