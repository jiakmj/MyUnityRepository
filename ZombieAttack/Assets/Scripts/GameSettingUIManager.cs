using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameSettingUIManager : MonoBehaviour
{
    public GameObject SettingsObj;

    public Text resolutionText;
    public Text graphicsQualityText;
    public Text fullScreenText;

    private int resolutionIndex = 0;
    private int qualityIndex = 0;
    private bool isFullScreen = true;

    private string[] resolutions = { "1280 x 720", "1920 x 1080", "2560 x 1440", "3870 x 2160" };
    private string[] qualityOptions = { "Low", "Normal", "High" };


    void Start()
    {
        
    }

    public void LoadScene(string sceneName)
    {        
        SceneManager.LoadScene("SmallMap");

        Debug.Log("æ¿ ¿Ãµø : " + sceneName);
    }

    public void ExitScene()
    {
        Application.Quit();
    }


    public void OnResolutionLeftClick()
    {
        SoundManager.Instance.PlaySFX("OnSettings", transform.position);
        resolutionIndex = Mathf.Max(0, resolutionIndex - 1);
        UpdateResolutionText();
    }

    public void OnResolutionRightClick()
    {
        SoundManager.Instance.PlaySFX("OnSettings", transform.position);
        resolutionIndex = Mathf.Min(resolutionIndex - 1, resolutionIndex + 1);
        UpdateResolutionText();
    }

    public void OnGraphicsLeftClick()
    {
        SoundManager.Instance.PlaySFX("OnSettings", transform.position);
        qualityIndex = Mathf.Max(0, qualityIndex - 1);
        UpdateGraphicsQualityText();
    }

    public void OnGraphicsRightClick()
    {
        SoundManager.Instance.PlaySFX("OnSettings", transform.position);
        qualityIndex = Mathf.Min(qualityIndex - 1, qualityIndex + 1);
        UpdateGraphicsQualityText();
    }
    
    public void OnFullScreenToggleClick()
    {
        SoundManager.Instance.PlaySFX("OnSettings", transform.position);
        isFullScreen = !isFullScreen;
        UpdateFullScreenText();
    }

    private void UpdateResolutionText()
    {
        resolutionText.text = resolutions[resolutionIndex];
    }

    private void UpdateGraphicsQualityText()
    {
        graphicsQualityText.text = qualityOptions[qualityIndex];
    }

    private void UpdateFullScreenText()
    {
        fullScreenText.text = isFullScreen ? "On" : "Off";
    }

    public void OnApplySettingsClick()
    {
        SoundManager.Instance.PlaySFX("OnSettings", transform.position);
        ApplySettings();
        SaveSettings();
    }   

    private void ApplySettings()
    {
        SoundManager.Instance.PlaySFX("OnSettings", transform.position);
        string[] res = resolutions[resolutionIndex].Split('x');
        int width = int.Parse(res[0]);
        int height = int.Parse(res[1]);
        Screen.SetResolution(width, height, isFullScreen);
        QualitySettings.SetQualityLevel(qualityIndex);
    }


    private void SaveSettings()
    {
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        PlayerPrefs.SetInt("GraphicsQualityIndex", qualityIndex);
        PlayerPrefs.SetInt("FullScreen", isFullScreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        resolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 1);
        qualityIndex = PlayerPrefs.GetInt("GraphicsQualityIndex", 1);
        
    }

    public void OnSettings()
    {
        SoundManager.Instance.PlaySFX("OnSettings", transform.position);
        SettingsObj.SetActive(true);
    }

    public void OnBackSettings()
    {
        SoundManager.Instance.PlaySFX("OnSettings", transform.position);
        SettingsObj.SetActive(false);
    }
}
