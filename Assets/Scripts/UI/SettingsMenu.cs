using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SettingsMenu : Menu
{
    public AudioMixer audioMixer;

    public TMP_Dropdown resDropdown;
    public Slider volSlider;
    public TMP_Dropdown qualDropdown;
    [FormerlySerializedAs("fullScreen")] public Toggle fullScrnToggle;

    private Resolution[] _resolutions;

    private void Start()
    {
        resDropdown.ClearOptions();

        // Load options into dropdown
        _resolutions = Screen.resolutions;
        resDropdown.AddOptions(new List<string>(_resolutions.Select(r => r.width + " x " + r.height)));

        // Load saved preferences
        float volume = PlayerPrefs.GetFloat("Volume", 0);
        SetVolume(volume);
        volSlider.value = volume;


        SetGraphics(PlayerPrefs.GetInt("Quality", QualitySettings.GetQualityLevel()));
        qualDropdown.value = QualitySettings.GetQualityLevel();

        SetFullScreen(PlayerPrefs.GetInt("Fullscreen", Screen.fullScreen ? 1 : 0) == 1);
        fullScrnToggle.isOn = Screen.fullScreen;

        // Set selected option to current resolution
        Resolution currRes = Screen.currentResolution;
        Resolution savedRes = new Resolution
        {
            width = PlayerPrefs.GetInt("ResolutionWidth", currRes.width),
            height = PlayerPrefs.GetInt("ResolutionHeight", currRes.height)
        };

        int currResIndex = Array.FindIndex(_resolutions, r => r.width.Equals(savedRes.width) && r.height.Equals(savedRes.height));
        if(currResIndex == -1)
            currResIndex = Array.FindIndex(_resolutions, r => r.width.Equals(currRes.width) && r.height.Equals(currRes.height));
        else Screen.SetResolution(savedRes.width, savedRes.height, Screen.fullScreen);

        resDropdown.value = currResIndex;
        resDropdown.RefreshShownValue();
    }

    public void SetResolution(int resIndex)
    {
        Resolution newRes = _resolutions[resIndex];
        Screen.SetResolution(newRes.width, newRes.height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionWidth", newRes.width);
        PlayerPrefs.SetInt("ResolutionHeight", newRes.height);

    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void SetGraphics(int setting)
    {
        QualitySettings.SetQualityLevel(setting);
        PlayerPrefs.SetInt("Quality", setting);
    }

    public void SetFullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
        PlayerPrefs.SetInt("Fullscreen", fullScreen ? 1 : 0);
    }
}