using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetector : MonoBehaviour
{
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Damage" || collision.gameObject.name.Substring(0, 6) == "Ground")
            PlayerController.playerController.hurtplayer();
    }
}
