using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour, IEventListener<SoundEvent>
{
    public AudioSource bgMusicSource;
    public AudioSource sfxSource;
    public AudioSource footstepSource;

    private Dictionary<string, AudioClip> backgroundMusics = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> sfxs = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> footstepSounds = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        LoadSounds();
    }

    private void Update()
    {
        if (!bgMusicSource.isPlaying)
        {
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                PlaySound("MainMenu", 0.5f, 0, true);
            }
            else
            {
                int random = Random.Range(0, backgroundMusics.Count);
                PlaySound(backgroundMusics.Keys.ToArray()[random], 0.5f, 0, true);
            }
        }
    }

    void LoadSounds()
    {
        AudioClip[] soundClips = Resources.LoadAll<AudioClip>("Sounds/BackgroundMusic");
        foreach (var clip in soundClips)
        {
            backgroundMusics.Add(clip.name, clip);
        }

        soundClips = Resources.LoadAll<AudioClip>("Sounds/SFX");
        foreach (var clip in soundClips)
        {
            sfxs.Add(clip.name, clip);
        }

        soundClips = Resources.LoadAll<AudioClip>("Sounds/Footsteps");
        foreach (var clip in soundClips)
        {
            footstepSounds.Add(clip.name, clip);
        }
    }

    public void PlaySound(string name, float volume, float delay, bool loop)
    {
        if (backgroundMusics.ContainsKey(name))
        {
            bgMusicSource.clip = backgroundMusics[name];
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
        if (sfxs.ContainsKey(name))
        {
            sfxSource.clip = sfxs[name];
            sfxSource.volume = volume;
            sfxSource.PlayDelayed(delay);
        }
        else
        {
            Debug.LogWarning("Sound " + name + " not found!");
        }
    }

    public void PlayFootstep(float volume, float delay)
    {
        var random = Random.Range(0, footstepSounds.Count);
        footstepSource.clip = footstepSounds.Values.ToArray()[random];
        footstepSource.volume = volume;
        footstepSource.PlayDelayed(delay);
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
            case SoundType.Footstep:
                PlayFootstep(eventType.Volume, eventType.Delay);
                break;
        }
    }

    protected virtual void OnEnable()
    {
        this.StartListeningEvent<SoundEvent>();
    }

    protected virtual void OnDisable()
    {
        this.StopListeningEvent<SoundEvent>();
    }
}

