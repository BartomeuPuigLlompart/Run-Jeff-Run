using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase;
using Firebase.Database;

public class PlayerController : MonoBehaviour
{
    FirebaseAuth firebaseAuth;
    FirebaseDatabase _database;
    DatabaseReference reference;

    public static User myUser;

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
        firebaseAuth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        _database = FirebaseDatabase.GetInstance("https://runjeffrun-3949c-default-rtdb.europe-west1.firebasedatabase.app/");
        reference = _database.RootReference;
        getPlayer();
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

    public void backToMenu()
    {
        updatePlayer();
        setRunStats();
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

    void getPlayer()
    {
        reference.Child("users").Child(PlayerPrefs.GetString("userId")).GetValueAsync().ContinueWith(_task =>
        {
            if (_task.IsFaulted)
            {
                // Handle the error...
                Debug.Log("F in the chat");
            }
            else if (_task.IsCompleted)
            {
                DataSnapshot snapshot = _task.Result;
                myUser = new User(snapshot.Child("rol").GetValue(true).ToString(),
                    snapshot.Child("correo").GetValue(true).ToString(),
                    snapshot.Child("id").GetValue(true).ToString(),
                    snapshot.Child("idOpuesto").GetValue(true).ToString());
                myUser.player = new User.Player();
                myUser.player.coins = int.Parse(snapshot.Child("player").Child("coins").GetValue(true).ToString());
                myUser.player.map1Unlocked = bool.Parse(snapshot.Child("player").Child("map1Unlocked").GetValue(true).ToString());
                myUser.player.map2Unlocked = bool.Parse(snapshot.Child("player").Child("map2Unlocked").GetValue(true).ToString());
                myUser.player.map3Unlocked = bool.Parse(snapshot.Child("player").Child("map3Unlocked").GetValue(true).ToString());
                myUser.player.map4Unlocked = bool.Parse(snapshot.Child("player").Child("map4Unlocked").GetValue(true).ToString());
                myUser.player.currentPlayingTime = int.Parse(snapshot.Child("player").Child("currentPlayingTime").GetValue(true).ToString());
                myUser.player.online = bool.Parse(snapshot.Child("player").Child("online").GetValue(true).ToString());
                myUser.player.tasksDone = bool.Parse(snapshot.Child("player").Child("tasksDone").GetValue(true).ToString());

                myUser.player.availablePlayingTime = int.Parse(snapshot.Child("player").Child("availablePlayingTime").GetValue(true).ToString());
                myUser.player.averageDailyPlayingTime = int.Parse(snapshot.Child("player").Child("averageDailyPlayingTime").GetValue(true).ToString());
                myUser.player.averageRunPlayingTime = int.Parse(snapshot.Child("player").Child("averageRunPlayingTime").GetValue(true).ToString());
                myUser.player.averagePauseOrMenuPlayingTime = int.Parse(snapshot.Child("player").Child("averagePauseOrMenuPlayingTime").GetValue(true).ToString());
                myUser.player.maxRunPlayingTime = int.Parse(snapshot.Child("player").Child("maxRunPlayingTime").GetValue(true).ToString());
                myUser.player.maxCoinsInSingleRun = int.Parse(snapshot.Child("player").Child("maxCoinsInSingleRun").GetValue(true).ToString());
            }
        });
        StartCoroutine("updateBD");
    }

    IEnumerator updateBD()
    {
        yield return new WaitForSeconds(1);
        int refresh = 10;
        int counter = 0;
        while (SceneManager.GetActiveScene().name == "SampleScene")
        {
            myUser.player.currentPlayingTime++;
            counter++;
            if (counter == refresh) { updatePlayer(); counter = 0; }
            yield return new WaitForSeconds(1);
        }
    }

    void setRunStats()
    {
        ;//Max coins, max time...
    }

    public void updatePlayer()
    {
        string json = JsonUtility.ToJson(myUser);
        reference.Child("users").Child(myUser.id).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsFaulted) Debug.Log("F in the chat");
            else if (task.IsCompleted)
            {
                Debug.Log("Ye");
            }

        });
        json = JsonUtility.ToJson(myUser.player);
        reference.Child("users").Child(myUser.id).Child("player").SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsFaulted) Debug.Log("F in the chat");
            else if (task.IsCompleted)
            {
                Debug.Log("Ye");
            }

        });
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
