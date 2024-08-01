using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TMP_Dropdown fpsDropdown;
    public TMP_Dropdown qualityDropdown;

    private void Start() 
    {
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
        SceneManager.GetSceneByBuildIndex(0);
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
