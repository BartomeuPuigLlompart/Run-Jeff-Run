using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] GameObject soundButton;

    [SerializeField] Sprite soundOnImg;
    [SerializeField] Sprite soundOffImg;

    [SerializeField] AudioSource tapAudioSource; //Of the tap sound
    [SerializeField] AudioSource musicAudioSource; //Of the music

    private bool soundOn = true;

    // Start is called before the first frame update
    void Start()
    {
        soundOn = (PlayerPrefs.GetFloat("SoundVolume", 0.5f) > 0.0f);

        soundButton.GetComponent<Image>().sprite = (soundOn) ? soundOnImg : soundOffImg;

        if (tapAudioSource)
        {
            tapAudioSource.volume = PlayerPrefs.GetFloat("SoundVolume", 0.5f);
        }

        if (musicAudioSource)
        {
            musicAudioSource.volume = PlayerPrefs.GetFloat("SoundVolume", 0.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Press()
    {
        soundOn = !soundOn;

        soundButton.GetComponent<Image>().sprite = (soundOn) ? soundOnImg : soundOffImg;

        PlayerPrefs.SetFloat("SoundVolume", (soundOn) ? 0.5f : 0f);

        if(tapAudioSource)
        {
            tapAudioSource.volume = PlayerPrefs.GetFloat("SoundVolume", 0.5f);
        }

        if (musicAudioSource)
        {
            musicAudioSource.volume = PlayerPrefs.GetFloat("SoundVolume", 0.5f) * 0.75f;
        }
    }

    public void PlayTapSound()
    {
        tapAudioSource.PlayOneShot(tapAudioSource.clip);
    }
}
