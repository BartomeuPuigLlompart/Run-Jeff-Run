using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    string level="Level 1-1";
    public Text textLevel;
    public void ButtonMoveScene()
    {
        SceneManager.LoadScene(level);
    }

    public void PlayAlbertLevel()
    {
        SceneManager.LoadScene("Level 12");
    }

    public void ExitButton() 
    {
        Application.Quit();
    }
    public void MinusLevel() 
    {
        SceneManager.LoadScene("Level 17");
    }
    public void PlusLevel()
    {
        switch (level)
        {
            case "Level 1-1":
                level = "Level 1-2";
                textLevel.text = "Level 1-2";
                break;
            case "Level 1-2":
                level = "Level 1-3";
                textLevel.text = "Level 1-3";
                break;
            case "Level 1-3":
                level = "Level 1-4";
                textLevel.text = "Level 1-3";
                break;
            case "Level 1-4":
                level = "Level 1-1";
                textLevel.text = "Level 1-1";
                break;
        }
    }
}
