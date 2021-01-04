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
    FirebaseAuth firebaseAuth;
    FirebaseDatabase _database;
    DatabaseReference reference;

    // Use this for initialization
    void Start()
    {
        firebaseAuth = Firebase.Auth.FirebaseAuth.DefaultInstance; ;
        //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://runjeffrun-3949c-default-rtdb.europe-west1.firebasedatabase.app/");
        //DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        _database = FirebaseDatabase.GetInstance("https://runjeffrun-3949c-default-rtdb.europe-west1.firebasedatabase.app/");
        reference = _database.RootReference;
        
        Invoke("SignIn", 2);
    }


    public void SignIn()
    {
        //firebaseAuth.CreateUserWithEmailAndPasswordAsync("tpuigllompart@gmail.com", "123456").ContinueWith(task =>
        //{
        //    if (task.IsCanceled)
        //    {
        //        Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
        //        return;
        //    }
        //    if (task.IsFaulted)
        //    {
        //        Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
        //        return;
        //    }

        //    // Firebase user has been created.
        //    Firebase.Auth.FirebaseUser newUser = task.Result;
        //    Debug.LogFormat("Firebase user created successfully: {0} ({1})",
        //        newUser.DisplayName, newUser.UserId);

        //    writeNewUser(newUser.UserId, "tpuigllompart@gmail.com");
        //});
        writeNewUser("122", "tpuigllompart@gmail.com");
    }

    private void writeNewUser(string userId, string email)
    {
        User user = new User("Tutor", email, "none", userId, "none");
        string json = JsonUtility.ToJson(user);
        _database.GetReference("scores/").SetValueAsync(1).ContinueWith(task =>
        {
            if (task.IsFaulted) Debug.Log("F in the chat");
            else if (task.IsCompleted)
            {
                Debug.Log("Ye");
            }
            
        });
        reference.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }

}