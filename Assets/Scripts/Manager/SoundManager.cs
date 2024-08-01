using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour, IEventListener<SoundEvent>
{
    public AudioSource bgMusicSource;
    public AudioSource sfxSource;
    public AudioSource footstepSource;

    public Slider bgMusicSlider;
    public Slider sfxSlider;

    private Dictionary<string, AudioClip> backgroundMusics = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> sfxs = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> footstepSounds = new Dictionary<string, AudioClip>();
    private float _currentBgMusicVolume;
    private float _currentSFXVolume;

    private void Awake()
    {
        LoadSounds();
    }

    private void Start() 
    {
        if (PlayerPrefs.HasKey("BgMusicVolume"))
        {
            bgMusicSlider.value = PlayerPrefs.GetFloat("BgMusicVolume");
            bgMusicSource.volume = bgMusicSlider.value;
            _currentBgMusicVolume = bgMusicSlider.value;
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
            sfxSource.volume = sfxSlider.value;
            if(footstepSource != null)
                footstepSource.volume = sfxSlider.value;

            _currentSFXVolume = sfxSlider.value;
        }

        bgMusicSlider.onValueChanged.AddListener(delegate { OnBgMusicVolumeChange(); });
        sfxSlider.onValueChanged.AddListener(delegate { OnSFXVolumeChange(); });
    }

    private void Update()
    {
        if (!bgMusicSource.isPlaying)
        {
            int random = Random.Range(0, backgroundMusics.Count);
            PlaySound(backgroundMusics.Keys.ToArray()[random], 0, true);
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

    public void PlaySound(string name, float delay, bool loop)
    {
        if (backgroundMusics.ContainsKey(name))
        {
            bgMusicSource.clip = backgroundMusics[name];
            bgMusicSource.volume = _currentBgMusicVolume;
            bgMusicSource.loop = loop;
            bgMusicSource.PlayDelayed(delay);
        }
        else
        {
            Debug.LogWarning("Sound " + name + " not found!");
        }
    }

    public void PlaySFX(string name, float delay)
    {
        if (sfxs.ContainsKey(name))
        {
            sfxSource.clip = sfxs[name];
            sfxSource.volume = _currentSFXVolume;
            sfxSource.PlayDelayed(delay);
        }
        else
        {
            Debug.LogWarning("Sound " + name + " not found!");
        }
    }

    public void PlayFootstep(float delay)
    {
        var random = Random.Range(0, footstepSounds.Count);
        footstepSource.clip = footstepSounds.Values.ToArray()[random];
        footstepSource.volume = _currentSFXVolume / 2;
        footstepSource.PlayDelayed(delay);
    }

    public void OnBgMusicVolumeChange()
    {
        bgMusicSource.volume = bgMusicSlider.value;
        PlayerPrefs.SetFloat("BgMusicVolume", bgMusicSlider.value);
        _currentBgMusicVolume = bgMusicSlider.value;
    }

    public void OnSFXVolumeChange()
    {
        sfxSource.volume = sfxSlider.value;
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        _currentSFXVolume = sfxSlider.value;
    }

    public void OnEvent(SoundEvent eventType)
    {
        switch (eventType.Type)
        {
            case SoundType.Background:
                PlaySound(eventType.Name, eventType.Delay, eventType.Loop);
                break;
            case SoundType.SFX:
                PlaySFX(eventType.Name, eventType.Delay);
                break;
            case SoundType.Footstep:
                PlayFootstep(eventType.Delay);
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

