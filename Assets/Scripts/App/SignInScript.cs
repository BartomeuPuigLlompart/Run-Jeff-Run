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

    [SerializeField] Text email, playerEmail, logInEmail;
    [SerializeField] InputField password, passwordConfirmation, logInPassword;

    [SerializeField] GameObject signUp, invitePlayer, logIn;

    public static User tutor, jugador;

    string userID;

    bool _goToTutorMenu = false;

    bool _goToInvitePlayer = false;

    bool _goToLogIn = false;

    bool _goToPlayerMenu = false;

    // Use this for initialization
    void Start()
    {
        email.horizontalOverflow = HorizontalWrapMode.Wrap;
        firebaseAuth = Firebase.Auth.FirebaseAuth.DefaultInstance; ;
        _database = FirebaseDatabase.GetInstance("https://runjeffrun-3949c-default-rtdb.europe-west1.firebasedatabase.app/");
        reference = _database.RootReference;
        if (SceneManager.GetActiveScene().name == "AuthTest") tutorLogIn(true);
        else playerLogIn(true);
    }

    private void Update()
    {
        if (_goToTutorMenu) goToTutorMenu();
        else if (_goToInvitePlayer) goToInvitePlayer();
        else if (_goToLogIn) goToLogIn();
        else if (_goToPlayerMenu) goToPlayerMenu();
    }

    public void tutorLogIn(bool firstLog = false)
    {
        firebaseAuth.SignInWithEmailAndPasswordAsync(!firstLog ? logInEmail.text : PlayerPrefs.GetString("userEmail"), !firstLog ? logInPassword.text : 
            PlayerPrefs.GetString("userPassword")).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

            reference.Child("users").Child(newUser.UserId).Child("rol").GetValueAsync().ContinueWith(_task => {
                if (_task.IsFaulted)
                {
                    // Handle the error...
                }
                else if (_task.IsCompleted)
                {
                    DataSnapshot snapshot = _task.Result;
                    if (snapshot.GetValue(true).ToString() == "Player")
                    {
                        Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: You don't have permission to Log In Here");
                        firebaseAuth.SignOut();
                    }
                    else
                    {
                        _goToTutorMenu = true; 
                    }
                }
            });
            
        });
    }
    public void playerLogIn(bool firstLog = false)
    {
        firebaseAuth.SignInWithEmailAndPasswordAsync(!firstLog ? email.text : PlayerPrefs.GetString("userEmail"), !firstLog ? password.text : PlayerPrefs.GetString("userPassword")).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

            reference.Child("users").Child(newUser.UserId).Child("rol").GetValueAsync().ContinueWith(_task => {
                if (_task.IsFaulted)
                {
                    // Handle the error...
                }
                else if (_task.IsCompleted)
                {
                    DataSnapshot snapshot = _task.Result;
                    if (snapshot.GetValue(true).ToString() == "Tutor")
                    {
                        Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: You don't have permission to Log In Here");
                        firebaseAuth.SignOut();
                    }
                    else
                    {
                        userID = newUser.UserId;
                        _goToPlayerMenu = true;
                    }
                }
            });

        });
    }


    public void SignUp()
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
            userID = newUser.UserId;
            writeNewUser(newUser.UserId, email.text);
        });
    }

    private void writeNewUser(string userId, string email)
    {
        tutor = new User("Tutor", email, userId, "none");
        string json = JsonUtility.ToJson(tutor);
        reference.Child("users").Child(userId).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsFaulted) Debug.Log("F in the chat");
            else if (task.IsCompleted)
            {
                _goToInvitePlayer = true;
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
        jugador = new User("Player", email, userId, tutor.id);
        jugador.player = new User.Player();
        jugador.player.createNewDefaultPlayer();
        string json = JsonUtility.ToJson(jugador);
        reference.Child("users").Child(userId).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsFaulted) Debug.Log("F in the chat");
            else if (task.IsCompleted)
            {
                Debug.Log("Ye");
            }

        });
        json = JsonUtility.ToJson(jugador.player);
        reference.Child("users").Child(userId).Child("player").SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsFaulted) Debug.Log("F in the chat");
            else if (task.IsCompleted)
            {
                Debug.Log("Ye");
            }

        });
        tutor.idOpuesto = userId;
        json = JsonUtility.ToJson(tutor);
        reference.Child("users").Child(tutor.id).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsFaulted) Debug.Log("F in the chat");
            else if (task.IsCompleted)
            {
                Debug.Log("Ye");
                _goToLogIn = true;
            }

        });
    }

    public void resetPassword()
    {
        reference.Child("users").GetValueAsync().ContinueWith(_task =>
        {
            if (_task.IsFaulted)
            {
                // Handle the error...
                Debug.Log("F in the chat");
            }
            else if (_task.IsCompleted)
            {
                DataSnapshot snapshot = _task.Result;
                bool found = false;
                foreach (DataSnapshot s in snapshot.Children)
                {
                    Debug.Log(s.Child("correo").GetValue(true).ToString());
                    if (s.Child("correo").GetValue(true).ToString() == logInEmail.text &&
                    s.Child("rol").GetValue(true).ToString() == "Player")
                    {
                        found = true;
                        firebaseAuth.SendPasswordResetEmailAsync(logInEmail.text).ContinueWith((authTask) =>
                        {
                            if (authTask.IsFaulted) Debug.Log("F in the chat");
                            else if (authTask.IsCompleted)
                            {
                                Debug.Log("Reset email sent successfully!\nPlease follow the instructions sent to you via email!");

                                return;
                            }
                        });
                    }
                }
                
                if(!found) Debug.Log("You are not registered as a player. If you want to play, find a tutor if you don't have one and sign up.");
            }
        });
        goToSignUp();
    }

    public void goToSignUp()
    {
        signUp.SetActive(true);
        logIn.SetActive(false);
        if(invitePlayer != null)invitePlayer.SetActive(false);
    }

    public void goToInvitePlayer()
    {
        signUp.SetActive(false);
        logIn.SetActive(false);
        if (invitePlayer != null) invitePlayer.SetActive(true);
        _goToInvitePlayer = false;
    }

    public void goToLogIn()
    {
        signUp.SetActive(false);
        logIn.SetActive(true);
        if (invitePlayer != null) invitePlayer.SetActive(false);
        _goToLogIn = false;
    }

    void goToPlayerMenu()
    {
        PlayerPrefs.SetString("userId", userID);
        PlayerPrefs.SetString("userEmail", email.text);
        PlayerPrefs.SetString("userPassword", password.text);
        SceneManager.LoadScene("Menu");
    }

    void goToTutorMenu()
    {
        PlayerPrefs.SetString("userId", userID);
        PlayerPrefs.SetString("userEmail", logInEmail.text);
        PlayerPrefs.SetString("userPassword", logInPassword.text);
        SceneManager.LoadScene("menuTutor");
    }
}