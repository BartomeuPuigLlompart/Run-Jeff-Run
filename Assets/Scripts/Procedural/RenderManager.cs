using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderManager : MonoBehaviour
{
    float groundfOffset = 21.6f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Render Objects")
        {
            if (collision.gameObject.layer == 8) 
                SpawnGround(collision.transform);
            else Destroy(collision.gameObject);
        }

    }

    void SpawnGround(Transform groundObject)
    {
        groundObject.transform.position += new Vector3(groundfOffset * 2.0f, 0, 0);
    }
}
