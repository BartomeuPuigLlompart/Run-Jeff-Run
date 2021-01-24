using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float speed;
    public float diffInTime;
    Vector2 initialDiff;
    [SerializeField] Vector2 maxDiff;
    public float startRef;
    public static CameraMovement cameraMovement;
    float deltaTime;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        cameraMovement = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        startRef = Time.time;
        initialDiff = new Vector2(speed, PlayerController.playerController.framesRef);
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        Debug.Log(Mathf.Ceil(fps).ToString());

        if (PauseMenu.GameIsPaused == false)
        {
           Vector2 difficulty = Vector2.Lerp(initialDiff, maxDiff, (Time.time - startRef) / diffInTime);
           speed = difficulty.x;
           if (PlayerController.playerController.framesRef > maxDiff.y && (Time.time - startRef) / diffInTime < 0.5f) PlayerController.playerController.framesRef = ((int)(difficulty.y - difficulty.y * (Time.time - startRef) / diffInTime)) / 2;
           else if ((Time.time - startRef) / diffInTime < 1.0f) PlayerController.playerController.framesRef = (int)difficulty.y - (int)(difficulty.y * (1 -(Time.time - startRef) / diffInTime));
           else PlayerController.playerController.framesRef = PlayerController.playerController.finalFrames;
           if(PlayerController.playerController.framesRef < PlayerController.playerController.finalFrames) PlayerController.playerController.framesRef = PlayerController.playerController.finalFrames;
           transform.position += new Vector3(speed, 0, 0);
        }



    }
}
