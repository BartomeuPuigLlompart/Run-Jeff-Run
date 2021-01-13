using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] GameObject soundButton;

    [SerializeField] Sprite soundOnImg;
    [SerializeField] Sprite soundOffImg;

    private bool soundOn = true;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.GetFloat("SoundVolume", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Press()
    {
        soundOn = !soundOn;

        soundButton.GetComponent<Image>().sprite = (soundOn) ? soundOnImg : soundOffImg;
    }
}
