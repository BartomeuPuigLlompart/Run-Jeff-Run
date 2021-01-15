using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using Firebase.Database;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorManager : MonoBehaviour
{
    FirebaseAuth firebaseAuth;
    FirebaseDatabase _database;
    DatabaseReference reference;

    string idOpuesto;
    User.Player player;
    string username;

    bool tutorRecived = false;
    bool playerRecived = false;

    [SerializeField] Text playerName, state, currentPlayingTime, availablePlayingTime, averageDailyPlayingTime, tasksDone;

    private void Awake()
    {
        firebaseAuth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        _database = FirebaseDatabase.GetInstance("https://runjeffrun-3949c-default-rtdb.europe-west1.firebasedatabase.app/");
        reference = _database.RootReference;
    }

    // Start is called before the first frame update
    void Start()
    {
        

        getTutorBD();
    }

    // Update is called once per frame
    void Update()
    {
        if(tutorRecived)
        {
            tutorRecived = false;
            getPlayer();
        }
        else if(playerRecived)
        {
            playerRecived = false;
            watchStats();
        }
    }

    void getTutorBD()
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
                idOpuesto = snapshot.Child("idOpuesto").GetValue(true).ToString();
                tutorRecived = true;
            }
        });
    }

    public void getPlayer()
    {
        //if(tutor == null || tutor.idOpuesto == null) { Invoke("getPlayer", 0.25f); return; }
        reference.Child("users").Child(idOpuesto).GetValueAsync().ContinueWith(_task =>
        {
            if (_task.IsFaulted)
            {
                // Handle the error...
                Debug.Log("F in the chat");
            }
            else if (_task.IsCompleted)
            {
                DataSnapshot snapshot = _task.Result;
                username = snapshot.Child("correo").GetValue(true).ToString(); //Por ahora
                player = new User.Player();
                player.coins = int.Parse(snapshot.Child("player").Child("coins").GetValue(true).ToString());
                player.map1Unlocked = bool.Parse(snapshot.Child("player").Child("map1Unlocked").GetValue(true).ToString());
                player.map2Unlocked = bool.Parse(snapshot.Child("player").Child("map2Unlocked").GetValue(true).ToString());
                player.map3Unlocked = bool.Parse(snapshot.Child("player").Child("map3Unlocked").GetValue(true).ToString());
                player.map4Unlocked = bool.Parse(snapshot.Child("player").Child("map4Unlocked").GetValue(true).ToString());
                player.currentPlayingTime = int.Parse(snapshot.Child("player").Child("currentPlayingTime").GetValue(true).ToString());
                player.online = bool.Parse(snapshot.Child("player").Child("online").GetValue(true).ToString());
                player.tasksDone = bool.Parse(snapshot.Child("player").Child("tasksDone").GetValue(true).ToString());

                player.numOfRuns = int.Parse(snapshot.Child("player").Child("numOfRuns").GetValue(true).ToString());
                player.numOfDays = int.Parse(snapshot.Child("player").Child("numOfDays").GetValue(true).ToString());
                player.availablePlayingTime = int.Parse(snapshot.Child("player").Child("availablePlayingTime").GetValue(true).ToString());
                player.averageDailyPlayingTime = int.Parse(snapshot.Child("player").Child("averageDailyPlayingTime").GetValue(true).ToString());
                player.averageRunPlayingTime = int.Parse(snapshot.Child("player").Child("averageRunPlayingTime").GetValue(true).ToString());
                player.averagePauseOrMenuPlayingTime = int.Parse(snapshot.Child("player").Child("averagePauseOrMenuPlayingTime").GetValue(true).ToString());
                player.maxRunPlayingTime = int.Parse(snapshot.Child("player").Child("maxRunPlayingTime").GetValue(true).ToString());
                player.maxCoinsInSingleRun = int.Parse(snapshot.Child("player").Child("maxCoinsInSingleRun").GetValue(true).ToString());

                playerRecived = true;
            }
        });
    }

    void watchStats()
    {
        playerName.text = "Player: " + username;
        state.text = "State: " + (player.online ? "Online" : "Offline");
        state.color = player.online ? new Color(0, 1, 0) : new Color(1, 0, 0);
        currentPlayingTime.text = "Current Playing Time: " + (player.currentPlayingTime / 60).ToString() + " minutes";
        availablePlayingTime.text = "Available Playing Time: " + player.availablePlayingTime + " minutes";
        averageDailyPlayingTime.text = "Average Daily Playing Time: " + player.averageDailyPlayingTime + " minutes";
        if(player.tasksDone)
        {
            tasksDone.text = "TASKS DONE";
            tasksDone.transform.parent.GetComponent<Button>().enabled = false;
        }
        else
        {
            tasksDone.text = "SET TASKS DONE";
            tasksDone.transform.parent.GetComponent<Button>().enabled = true;
        }
    }

    public void setTasksDone()
    {
        player.tasksDone = true;
        updatePlayer();
        getPlayer();
    }

    public void logOut()
    {
        firebaseAuth.SignOut();
        PlayerPrefs.DeleteKey("userId");
        PlayerPrefs.DeleteKey("userEmail");
        PlayerPrefs.DeleteKey("userPassword");
        SceneManager.LoadScene("AuthTest");
    }

    public void updatePlayer()
    {
        string json = JsonUtility.ToJson(player);
        reference.Child("users").Child(idOpuesto).Child("player").SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsFaulted) Debug.Log("F in the chat");
            else if (task.IsCompleted)
            {
                Debug.Log("Ye");
            }

        });
    }
}
