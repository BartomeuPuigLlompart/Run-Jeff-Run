﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderManager : MonoBehaviour
{
    const float groundfOffset = 21.6f;
    const float groundY = -4.64f;
    const float maxNonFreeSpace = 0.1f;
    const int maxGroundRows = 7;
    const int maxRows = 10;
    const int maxPlatformColumns = 4;
    [SerializeField] GameObject platformPrefab;

    

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Render Objects")
        {
            if (collision.gameObject.layer == 8 && collision.name.Substring(0, 6) == "Ground") 
                SpawnGround(collision.transform);
            else Destroy(collision.gameObject);
        }

    }

    void SpawnGround(Transform groundObject)
    {
        groundObject.transform.position += new Vector3(groundfOffset * 2.0f, 0, 0);
        //Random Y
        float randomY = groundY + ((Time.time - CameraMovement.cameraMovement.startRef) < ( CameraMovement.cameraMovement.diffInTime / 2.0f) ? 
            Random.Range(0, (int)((Time.time - CameraMovement.cameraMovement.startRef) / 60.0f) + 3) : Random.Range(0, maxGroundRows));
        groundObject.transform.position = new Vector3(groundObject.transform.position.x, randomY, groundObject.transform.position.z);
        Instantiate(platformPrefab, new Vector3(groundObject.position.x, groundObject.transform.position.y -100, groundObject.position.z), groundObject.rotation, groundObject);
        //Free Y Space
        float maxTiles = ((Time.time - CameraMovement.cameraMovement.startRef) < (CameraMovement.cameraMovement.diffInTime / 2.0f) ? maxNonFreeSpace / 2.0f : maxNonFreeSpace);
        GameObject[][] platforms = new GameObject[maxPlatformColumns][];
        int counter = 0;
        for (int i = 0; i < maxPlatformColumns; i++)
        {
            Debug.Log((int)(randomY - groundY));
            Debug.Log((int)(maxRows - (randomY - groundY + 1)));
            platforms[i] = new GameObject[(int)(maxRows - (randomY - groundY + 1))];
            for (int j = 0; j < (int)(maxRows - (randomY - groundY + 1)); j++)
            {
                if (Random.Range(0, 4) == 0 && counter < (int)(maxRows - (randomY - groundY + 1)) * maxPlatformColumns * (maxNonFreeSpace / 
                    ((Time.time - CameraMovement.cameraMovement.startRef) < (CameraMovement.cameraMovement.diffInTime / 2.0f) ? 2 : 1)))
                {
                    platforms[i][j] = Instantiate(groundObject.GetChild(0).gameObject, new Vector3(groundObject.position.x, groundObject.transform.position.y + j + 1, groundObject.position.z), groundObject.GetChild(0).rotation, groundObject);
                    platforms[i][j].transform.localPosition = new Vector2(-1.5f + i, platforms[i][j].transform.localPosition.y);
                    platforms[i][j].tag = "Render Objects";
                    platforms[i][j].transform.parent = null;
                    j += i != 0 ? 2 : 3;
                    counter++;
                }
            }
        }

        Destroy(groundObject.GetChild(0).gameObject);

        while(randomY != groundY)
        {
            GameObject underground = Instantiate(groundObject.gameObject, new Vector3(groundObject.position.x, randomY, groundObject.position.z) + Vector3.down, groundObject.rotation);
            if (underground.transform.GetChild(0) != null) Destroy(underground.transform.GetChild(0).gameObject);
            underground.layer = 0;
            randomY = (float)System.Math.Round(underground.transform.position.y, 2);
        }
    }
}
