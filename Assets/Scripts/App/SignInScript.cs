using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using Firebase.Database;
//using Firebase.Unity.Editor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SignInScript : MonoBehaviour
{
    const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";

    FirebaseAuth firebaseAuth;
    FirebaseDatabase _database;
    DatabaseReference reference;

    [SerializeField] Text email, playerEmail;
    [SerializeField] InputField password, passwordConfirmation;


    // Use this for initialization
    void Start()
    {
        email.horizontalOverflow = HorizontalWrapMode.Wrap;
        firebaseAuth = Firebase.Auth.FirebaseAuth.DefaultInstance; ;
        _database = FirebaseDatabase.GetInstance("https://runjeffrun-3949c-default-rtdb.europe-west1.firebasedatabase.app/");
        reference = _database.RootReference;
    }


    public void SignIn()
    {
        if(password.text != passwordConfirmation.text)
        {
            Debug.LogError("You entered two different passwords");
            return;
        }

        firebaseAuth.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

            writeNewUser(newUser.UserId, email.text);
        });
    }

    private void writeNewUser(string userId, string email)
    {
        User user = new User("Tutor", email, "none", userId, "none");
        string json = JsonUtility.ToJson(user);
        reference.Child("users").Child(userId).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsFaulted) Debug.Log("F in the chat");
            else if (task.IsCompleted)
            {
                PlayerPrefs.SetString("email", email);
                PlayerPrefs.SetString("userId", userId);
            }
            
        });
    }

    public void setPlayer()
    {
        string randomPassword = "";

        for (int i = 0; i < 15; i++)
        {
            randomPassword += glyphs[Random.Range(0, glyphs.Length)];
        }

        firebaseAuth.CreateUserWithEmailAndPasswordAsync(playerEmail.text, randomPassword).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

            writeNewPlayer(newUser.UserId, playerEmail.text);
        });
    }

    private void writeNewPlayer(string userId, string email)
    {
        User user = new User("Tutor", email, "none", userId, "none");
        string json = JsonUtility.ToJson(user);
        reference.Child("users").Child(userId).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsFaulted) Debug.Log("F in the chat");
            else if (task.IsCompleted)
            {
                Debug.Log("Ye");
            }

        });
    }

}