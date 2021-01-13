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


    // Use this for initialization
    void Start()
    {
        email.horizontalOverflow = HorizontalWrapMode.Wrap;
        firebaseAuth = Firebase.Auth.FirebaseAuth.DefaultInstance; ;
        _database = FirebaseDatabase.GetInstance("https://runjeffrun-3949c-default-rtdb.europe-west1.firebasedatabase.app/");
        reference = _database.RootReference;
        if (SceneManager.GetActiveScene().name == "AuthTest") tutorLogIn();
        else playerLogIn();
    }

    bool reLogin()
    {
        firebaseAuth.SignInWithEmailAndPasswordAsync(PlayerPrefs.GetString("userEmail"), PlayerPrefs.GetString("userPassword")).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return false;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return false;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

            return true;
        });
        return false;
    }

    public void tutorLogIn()
    {
        firebaseAuth.SignInWithEmailAndPasswordAsync(logInEmail.text, logInPassword.text).ContinueWith(task => {
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
                        PlayerPrefs.SetString("userId", newUser.UserId);
                        PlayerPrefs.SetString("userEmail", logInEmail.text);
                        PlayerPrefs.SetString("userPassword", logInPassword.text);
                        goToTutorMenu();
                    }
                }
            });
            
        });
    }
    public void playerLogIn()
    {
        firebaseAuth.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task => {
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
                        PlayerPrefs.SetString("userId", newUser.UserId);
                        PlayerPrefs.SetString("userEmail", email.text);
                        PlayerPrefs.SetString("userPassword", password.text);
                        goToPlayerMenu();
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
                goToInvitePlayer();
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
        string json = JsonUtility.ToJson(jugador);
        reference.Child("users").Child(userId).SetRawJsonValueAsync(json).ContinueWith(task =>
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
                goToLogIn();
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
                foreach (DataSnapshot s in snapshot.Children)
                {
                    Debug.Log(s.Child("correo").GetValue(true).ToString());
                    if (s.Child("correo").GetValue(true).ToString() == logInEmail.text &&
                    s.Child("rol").GetValue(true).ToString() == "Player")
                    {
                        firebaseAuth.SendPasswordResetEmailAsync(logInEmail.text).ContinueWith((authTask) =>
                        {
                            if (authTask.IsFaulted) Debug.Log("F in the chat");
                            else if (authTask.IsCompleted)
                            {
                                Debug.Log("Reset email sent successfully!\nPlease follow the instructions sent to you via email!");
                                goToSignUp();
                                return;
                            }
                        });
                    }
                }
                Debug.Log("You are not registered as a player. If you want to play, find a tutor if you don't have one and sign up.");
                goToSignUp();
            }
        });
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
    }

    public void goToLogIn()
    {
        signUp.SetActive(false);
        logIn.SetActive(true);
        if (invitePlayer != null) invitePlayer.SetActive(false);
    }

    void goToPlayerMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    void goToTutorMenu()
    {
        SceneManager.LoadScene("menuTutor");
    }
}