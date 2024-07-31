using UnityEngine;
using UnityEngine.UI; 

public class AudioManager : MonoBehaviour
{
    public Slider volumeSlider;
    private float currentVolume;

    void Start()
    {
        
        if (PlayerPrefs.HasKey("Volume"))
        {
            currentVolume = PlayerPrefs.GetFloat("Volume");
            volumeSlider.value = currentVolume;
        }
        else
        {
            currentVolume = volumeSlider.value;
        }

        
        volumeSlider.onValueChanged.AddListener(delegate { SetVolume(volumeSlider.value); });
    }

    public void SetVolume(float volume)
    {
        currentVolume = volume;
        AudioListener.volume = currentVolume;
        PlayerPrefs.SetFloat("Volume", currentVolume);
    }
}
