using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TMP_Dropdown fpsDropdown;
    public TMP_Dropdown qualityDropdown;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    Resolution[] resolutions;

    private void Start() 
    {
        FindResolutions();
        ApplySettings();

        fpsDropdown.onValueChanged.AddListener(delegate { SetFps(fpsDropdown.value); });
        qualityDropdown.onValueChanged.AddListener(delegate { SetQuality(qualityDropdown.value); });
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ApplySettings()
    {
        if (PlayerPrefs.HasKey("FPS"))
        {
            SetFps(PlayerPrefs.GetInt("FPS"));
            fpsDropdown.value = PlayerPrefs.GetInt("FPS");
        }
        else
        {
            SetFps(0);
            fpsDropdown.value = 0;
        }
        
        if (PlayerPrefs.HasKey("Quality"))
        {
            SetQuality(PlayerPrefs.GetInt("Quality"));
            qualityDropdown.value = PlayerPrefs.GetInt("Quality");
        }
        else
        {
            SetQuality(2);
            qualityDropdown.value = 2;
        }

        if (PlayerPrefs.HasKey("isFullScreen"))
        {
            bool isFullscreen = PlayerPrefs.GetInt("isFullScreen") == 1;
            Screen.fullScreen = isFullscreen;
            fullscreenToggle.isOn = isFullscreen;
        }
        else
        {
            Screen.fullScreen = true;
            fullscreenToggle.isOn = true;
        }

        if (PlayerPrefs.HasKey("Resolution"))
        {
            resolutionDropdown.value = PlayerPrefs.GetInt("Resolution");
            Resolution resolution = resolutions[PlayerPrefs.GetInt("Resolution")];
            
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }
        else
        {
            var resolution = resolutions.FirstOrDefault(x => x.width == 1920 && x.height == 1080);
            Screen.SetResolution(resolution.width, resolution.height, true);
        }
    }

    private void FindResolutions()
	{
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        var fullscreen = isFullscreen ? 1 : 0;
        PlayerPrefs.SetInt("isFullScreen", fullscreen);
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        resolutionDropdown.value = resolutionIndex;
        PlayerPrefs.SetInt("Resolution", resolutionIndex);
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFps(int fps)
    {
        switch (fps)
        {
            case 0:
                PlayerPrefs.SetInt("FPS", 0);
                Application.targetFrameRate = 60;
                break;
            case 1:
                PlayerPrefs.SetInt("FPS", 1);
                Application.targetFrameRate = 120;
                break;
            case 2:
                PlayerPrefs.SetInt("FPS", 2);
                Application.targetFrameRate = 240;
                break;
        }
    }

    public void SetQuality(int quality)
    {
        PlayerPrefs.SetInt("Quality", quality);
        UpdateURPSettings(quality);
    }

    private void UpdateURPSettings(int quality)
    {
        UniversalRenderPipelineAsset urpAsset = QualitySettings.renderPipeline as UniversalRenderPipelineAsset;
        if (urpAsset != null)
        {
            switch (quality)
            {
                case 0:
                    urpAsset.shadowDistance = 0f;
                    urpAsset.msaaSampleCount = 2;
                    break;
                case 1:
                    urpAsset.shadowDistance = 10f;
                    urpAsset.msaaSampleCount = 4;
                    break;
                case 2:
                    urpAsset.shadowDistance = 200f;
                    urpAsset.msaaSampleCount = 8;
                    break;
            }
        }
    }
}
