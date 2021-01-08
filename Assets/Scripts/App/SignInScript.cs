﻿using System.Collections;
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
                        Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: You don't have permission to Log In Here");
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
                PlayerPrefs.SetString("userId", userId);
            }
            
        });

        goToInvitePlayer();
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

    public void goToSignUp()
    {
        signUp.SetActive(true);
        logIn.SetActive(false);
        invitePlayer.SetActive(false);
    }

    public void goToInvitePlayer()
    {
        signUp.SetActive(false);
        logIn.SetActive(false);
        invitePlayer.SetActive(true);
    }

    public void goToLogIn()
    {
        signUp.SetActive(false);
        logIn.SetActive(true);
        invitePlayer.SetActive(false);
    }
}