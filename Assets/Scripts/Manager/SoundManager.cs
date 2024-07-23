using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour, IEventListener<SoundEvent>
{
    public AudioSource bgMusicSource;
    public AudioSource sfxSource;

    private Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>();

    private void Start()
    {
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

    public void PlaySound(string name, float volume, float delay, bool loop)
    {
        if (sounds.ContainsKey(name))
        {
            bgMusicSource.clip = sounds[name];
            bgMusicSource.volume = volume;
            bgMusicSource.loop = loop;
            bgMusicSource.PlayDelayed(delay);
        }
        else
        {
            Debug.LogWarning("Sound " + name + " not found!");
        }
    }

    public void PlaySFX(string name, float volume, float delay)
    {
        if (sounds.ContainsKey(name))
        {
            sfxSource.clip = sounds[name];
            sfxSource.volume = volume;
            sfxSource.PlayDelayed(delay);
        }
        else
        {
            Debug.LogWarning("Sound " + name + " not found!");
        }
    }

    public void OnEvent(SoundEvent eventType)
    {
        switch (eventType.Type)
        {
            case SoundType.Background:
                PlaySound(eventType.Name, eventType.Volume, eventType.Delay, eventType.Loop);
                break;
            case SoundType.SFX:
                PlaySFX(eventType.Name, eventType.Volume, eventType.Delay);
                break;
        }
    }

}

