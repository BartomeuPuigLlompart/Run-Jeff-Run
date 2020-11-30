using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pursuer : MonoBehaviour
{
    [SerializeField] float speed;
    float xSoftSpeed = 0.0025f;
    // Update is called once per frame
    void Update()
    {
        //tornar a fer pero amb aquesta nova aplicacio
        Vector2 lastPos = transform.position;
        transform.position = PlayerController.playerController.checkTrail(transform.position);
        transform.position = new Vector2(Vector2.Lerp(lastPos, transform.position, xSoftSpeed).x, Vector2.Lerp(lastPos, transform.position, speed).y) ;
  
    }
}
