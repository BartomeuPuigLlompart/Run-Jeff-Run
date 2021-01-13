using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
 

    public Text text;
    enum states { DEAD, HURT, NORMAL}
    [SerializeField] states state;
    [SerializeField]Transform gameMidRef;
    [SerializeField] float speed;
    [SerializeField] float savedSpeed;
    const float deadRef = 0.0015f;
    const float hurtRef = 0.0075f;
    const float normalRef = 0.015f;
    float inmunityRef = 0.0f;
    List<Vector2> trail;
    public int framesRef;
    public  int finalFrames = 50;
    public static PlayerController playerController;
    private int coins;

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
       
        if (PauseMenu.GameIsPaused == true)
        {
            savedSpeed= speed;
            speed = 0;
        }
      
    }

    public void hurtplayer()
    {
        transform.position += Vector3.up;
        if (inmunityRef + 3.0f < Time.time)
        {
            inmunityRef = Time.time;
            state--;
            if (state == 0) Invoke("backToMenu", 3.0f);
        }
    }

    void backToMenu()
    {

        SceneManager.LoadScene(1);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9) //Layer Coin
        {

            coins = int.Parse(text.text);
            coins++;
            text.text = coins.ToString();
            Destroy(collision.gameObject);
            FindObjectOfType<AudioManager>().Play("CoinCollect");

            //Update the PlayerPref of total Coins
            PlayerPrefs.SetInt("CurrCoins", PlayerPrefs.GetInt("CurrCoins", 0) + 1); //CurrCoins++
        }
    }
}
