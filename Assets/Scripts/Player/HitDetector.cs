using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetector : MonoBehaviour
{
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag != "Player" && collision.tag != "Camera Scope" &&
            collision.gameObject.layer != 9)
            PlayerController.playerController.hurtplayer();
    }
}
