using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource bgMusicSource;
    public AudioSource sfxSource;

    private Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadSounds();
    }

    void LoadSounds()
    {
        AudioClip[] soundClips = Resources.LoadAll<AudioClip>("Sounds");
        foreach (var clip in soundClips)
        {
            sounds.Add(clip.name, clip);
        }
    }

    public void PlayMusic(string name)
    {
        if (sounds.ContainsKey(name))
        {
            bgMusicSource.clip = sounds[name];
            bgMusicSource.Play();
        }
        else
        {
            Debug.LogWarning("Sound " + name + " not found!");
        }
    }

    public void PlaySFX(string name)
    {
        if (sounds.ContainsKey(name))
        {
            sfxSource.PlayOneShot(sounds[name]);
        }
        else
        {
            Debug.LogWarning("Sound " + name + " not found!");
        }
    }
}

