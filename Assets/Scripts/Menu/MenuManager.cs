using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase;
using Firebase.Database;
public class MenuManager : MonoBehaviour
{
    FirebaseAuth firebaseAuth;
    FirebaseDatabase _database;
    DatabaseReference reference;

    [SerializeField] private GameObject firstUI;

    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject levels;
    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject stats;

    [SerializeField] GameObject areYouSureObject;

    [SerializeField] Text[] coinsTexts;
    [SerializeField] Text currentPlayingTime, availablePlayingTime, tasksDone, averageRunPlayingTime, maxCoinsInSingleRun, maxRunPlayingTime;

    [SerializeField] InputField setAvailableTimeField;

    int playerCoins;

    User myUser;
    float menuTime;
    

    // Start is called before the first frame update
    void Start()
    {
        firebaseAuth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        _database = FirebaseDatabase.GetInstance("https://runjeffrun-3949c-default-rtdb.europe-west1.firebasedatabase.app/");
        reference = _database.RootReference;

        //Debug Only
        //PlayerPrefs.SetInt("CurrCoins", 650);
        playerCoins = PlayerPrefs.GetInt("CurrCoins", 0);

        //Set the values
        PlayerPrefs.SetInt("Char1", 1);
        PlayerPrefs.SetInt("House1", 1);
        PlayerPrefs.SetInt("Enemy1", 1);

        //Inactive every panel and then active the first selected one
        menu.SetActive(false);
        levels.SetActive(false);
        shop.SetActive(false);
        stats.SetActive(false);

        firstUI.SetActive(true);

        getPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (myUser != null)
        {
            if (playerCoins != myUser.player.coins)
            {
                myUser.player.coins = playerCoins;
                updatePlayer();
            }
        }
        for(int i = 0; i < coinsTexts.Length; i++)
        {
            if(coinsTexts[i]) //Check if it exists
            {
                int textValue;
                int.TryParse(coinsTexts[i].text, out textValue);
                int playerPref = PlayerPrefs.GetInt("CurrCoins", 0);

                if (textValue != playerPref)
                {
                    coinsTexts[i].text = playerPref.ToString();
                }
            }
        }
        menuTime += Time.unscaledDeltaTime;
    }

    public void updatePlayer()
    {
        if (PlayerPrefs.HasKey("pauseTime")) PlayerPrefs.SetFloat("pauseTime", menuTime + PlayerPrefs.GetFloat("pauseTime"));
        else PlayerPrefs.SetFloat("pauseTime", menuTime);
        menuTime = 0;
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

                myUser.player.numOfRuns = int.Parse(snapshot.Child("player").Child("numOfRuns").GetValue(true).ToString());
                myUser.player.numOfDays = int.Parse(snapshot.Child("player").Child("numOfDays").GetValue(true).ToString());
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
        while (SceneManager.GetActiveScene().name == "Menu")
        {
            myUser.player.currentPlayingTime++;
            counter++;
            if (counter == refresh) { updatePlayer(); counter = 0; }
            yield return new WaitForSeconds(1);
        }
    }

    void watchStats()
    {
        currentPlayingTime.text = "Current Playing Time: " + (myUser.player.currentPlayingTime / 60).ToString() + " minutes";
        availablePlayingTime.text = "Available Playing Time: " + myUser.player.availablePlayingTime + " minutes";
        tasksDone.text = myUser.player.tasksDone ? "TASKS DONE" : "TASKS NOT DONE";
        tasksDone.color = myUser.player.tasksDone ? new Color(0, 1, 0) : new Color(1, 0, 0);
        averageRunPlayingTime.text = "Average Run Playing Time: " + myUser.player.averageRunPlayingTime + " seconds";
        maxRunPlayingTime.text = "Max Run Playing Time: " + myUser.player.maxRunPlayingTime + " seconds";
        maxCoinsInSingleRun.text = "Max Coins In Single Run: " + myUser.player.maxCoinsInSingleRun + " coins";
    }

    public void setAvailableTime()
    {
        myUser.player.availablePlayingTime = int.Parse(setAvailableTimeField.text);
        updatePlayer();
    }

    public void GoToMenu()
    {
        if(!areYouSureObject.active)
        {
            menu.SetActive(true);
            levels.SetActive(false);
            shop.SetActive(false);
            stats.SetActive(false);
        }
    }

    public void GoToLevels()
    {
        levels.SetActive(true);
        menu.SetActive(false);
        shop.SetActive(false);
        stats.SetActive(false);
    }

    public void GoToShop()
    {
        shop.SetActive(true);
        menu.SetActive(false);
        levels.SetActive(false);
        stats.SetActive(false);
    }

    public void GoToStats()
    {
        shop.SetActive(false);
        menu.SetActive(false);
        levels.SetActive(false);
        stats.SetActive(true);
        watchStats();
    }

    public void GoToLevel1() //For Presentation only
    {
        updatePlayer();
        SceneManager.LoadScene(2); //Menu scene
    }
}
