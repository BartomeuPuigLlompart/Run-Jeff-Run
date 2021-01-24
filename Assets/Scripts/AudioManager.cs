using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    // Start is called before the first frame update

    bool soundOn, oldSoundOn;

    void Awake()
    {
        soundOn = (PlayerPrefs.GetFloat("SoundVolume", 0.5f) > 0.0f);

        if (PlayerPrefs.GetFloat("SoundVolume", 0.5f) == 0.0f) Camera.main.gameObject.GetComponent<AudioSource>().mute = true;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = PlayerPrefs.GetFloat("SoundVolume", 0.5f); //s.volume;

        }
    }

    public void Play (string name)
    {
      Sound s= Array.Find(sounds, sound=> sound.name == name);
        s.source.Play();
    }
 
}
