using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderManager : MonoBehaviour
{
    const float groundfOffset = 21.6f;
    const float groundY = -4.64f;
    const float topT = 1.36f;

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
        //Random Y
        float randomY = groundY + Random.Range(0, 7);
        groundObject.transform.position = new Vector3(groundObject.transform.position.x, randomY, groundObject.transform.position.z);
        while(randomY != groundY)
        {
            GameObject underground = Instantiate(groundObject.gameObject, new Vector3(groundObject.position.x, randomY, groundObject.position.z) + Vector3.down, groundObject.rotation);
            underground.layer = 0;
            randomY = (float)System.Math.Round(underground.transform.position.y, 2);
        }
    }
}
