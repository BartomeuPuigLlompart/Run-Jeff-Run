using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    int countDownStartValue = 60;
    public Text timerUI;
    // Start is called before the first frame update
    void Start()
    {
        countDownTimer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void countDownTimer()
    {
        if (countDownStartValue > 0)
        {
          
            timerUI.text =  "Timer: "+countDownStartValue;
            countDownStartValue--;
            Invoke("countDownTimer", 1.0f);
        }
        else
        {
            SceneManager.LoadScene("Level 2");
        }
    }
}
