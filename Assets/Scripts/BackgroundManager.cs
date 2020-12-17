using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    const float imageDistance = 70.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.main.ScreenToWorldPoint(Vector3.zero).x > (transform.position.x + imageDistance)) transform.position += new Vector3(imageDistance * 2.0f, 0, 0);
    }
}
