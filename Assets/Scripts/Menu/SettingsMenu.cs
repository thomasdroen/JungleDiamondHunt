using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

public class SettingsMenu : Menu
{

    public static SettingsMenu Instance;

    public static EventHandler<SettingsEventArgs> OnChangeSettings;
    public static EventHandler<SettingsFovEventArgs> OnChangeFov;

    public Dropdown qualityDropdown;
    public Dropdown resolutionsDropdown;
    public Toggle fullscreenToggle;
    public Slider fovSlider;
    public InputField fovField;
    public Slider musicSlider;
    public Slider sfxSlider;
    public AudioMixer audioMixer;
    public PostProcessingProfile postProcessing;
    [Space] public Text debugText;
    private Resolution[] resolutions;

    private float minFOV = 65;
    private float maxFOV = 90;
    private Camera mainCamera;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;

        resolutions = Screen.resolutions;
        resolutionsDropdown.ClearOptions();

        List<string> resolutionList = new List<string>();

        int currentResolution = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            resolutionList.Add(resolutions[i].ToString());

            currentResolution =
                resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height &&
                resolutions[i].refreshRate == Screen.currentResolution.refreshRate
                    ? i
                    : currentResolution;
        }
        resolutionsDropdown.AddOptions(resolutionList);
        resolutionsDropdown.value = currentResolution;
        resolutionsDropdown.RefreshShownValue();

        LoadSettings();
    }

    public override void OpenMenu()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public override void CloseMenu()
    {
        SaveSettings();
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void ChangeResolution(int resolution)
    {
        Resolution resolutionChosen = resolutions[resolution];
        Screen.SetResolution(resolutionChosen.width, resolutionChosen.height, Screen.fullScreen, resolutionChosen.refreshRate);
    }

    public void OnChangeQualityLevel(int qualityLevel)
    {
        QualitySettings.SetQualityLevel(qualityLevel);
        if (OnChangeSettings != null)
            OnChangeSettings(this, new SettingsEventArgs { QualityLevel = qualityLevel });

        switch (qualityLevel)
        {
            case 0:
                postProcessing.ambientOcclusion.enabled = false;
                postProcessing.antialiasing.enabled = false;
                postProcessing.bloom.enabled = false;
                postProcessing.motionBlur.enabled = false;
                postProcessing.vignette.enabled = false;
                break;

            case 1:
                postProcessing.ambientOcclusion.enabled = false;
                postProcessing.antialiasing.enabled = false;
                postProcessing.bloom.enabled = false;
                postProcessing.motionBlur.enabled = false;
                postProcessing.vignette.enabled = false;
                break;

            case 2:
                postProcessing.ambientOcclusion.enabled = false;
                postProcessing.antialiasing.enabled = true;
                postProcessing.bloom.enabled = false;
                postProcessing.motionBlur.enabled = false;
                postProcessing.vignette.enabled = true;
                break;

            case 3:
                postProcessing.ambientOcclusion.enabled = true;
                postProcessing.antialiasing.enabled = true;
                postProcessing.bloom.enabled = true;
                postProcessing.motionBlur.enabled = false;
                postProcessing.vignette.enabled = true;
                break;

            case 4:
                postProcessing.ambientOcclusion.enabled = true;
                postProcessing.antialiasing.enabled = true;
                postProcessing.bloom.enabled = true;
                postProcessing.motionBlur.enabled = true;
                postProcessing.vignette.enabled = true;
                break;

            case 5:
                postProcessing.ambientOcclusion.enabled = true;
                postProcessing.antialiasing.enabled = true;
                postProcessing.bloom.enabled = true;
                postProcessing.motionBlur.enabled = true;
                postProcessing.vignette.enabled = true;
                break;

            default:
                postProcessing.ambientOcclusion.enabled = true;
                postProcessing.antialiasing.enabled = true;
                postProcessing.bloom.enabled = true;
                postProcessing.motionBlur.enabled = true;
                postProcessing.vignette.enabled = true;
                break;
        }
    }

    public void ToggleFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }

    public void SetFOV(float fov)
    {
        fov = Mathf.Clamp(fov, minFOV, maxFOV);
        fovSlider.value = fov;
        fovField.text = "" + fov;
        mainCamera.fieldOfView = fov;
        OnChaOnChangeFieldOfView(fov);
        if (RigidbodyFirstPersonController.player != null)
            RigidbodyFirstPersonController.player.fovBySpeed.Setup();
    }

    public void SetFOV(string fovString)
    {
        float fov;
        if (!float.TryParse(fovString, out fov))
        {
            fov = fovSlider.value;
        }

        fov = Mathf.Clamp(fov, minFOV, maxFOV);
        fovSlider.value = fov;
        fovField.text = "" + fov;
        mainCamera.fieldOfView = fov;
        OnChaOnChangeFieldOfView(fov);
        RigidbodyFirstPersonController.player.fovBySpeed.Setup();
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", GetCorrectVolume(volume) * 80);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", GetCorrectVolume(volume) * 80);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("Settings.quality", QualitySettings.GetQualityLevel());
        PlayerPrefs.SetString("Settings.res", resolutionsDropdown.options[resolutionsDropdown.value].text);
        int fullscreen = Screen.fullScreen ? 1 : 0;
        PlayerPrefs.SetInt("Settings.fullscreen", fullscreen);
        PlayerPrefs.SetFloat("Settings.fov", mainCamera.fieldOfView);
        PlayerPrefs.SetFloat("Settings.musicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("Settings.sfxVolume", sfxSlider.value);
        PlayerPrefs.Save();

        //debugText.text = PlayerPrefs.GetString("Settings.res") + "vs current res: " + Screen.currentResolution;
    }

    public void LoadSettings()
    {
        int quality;
        Resolution resolution;
        bool fullscreen;
        float fov;
        float musicVolume;
        float sfxVolume;
        if (PlayerPrefs.HasKey("Settings.quality"))
            quality = PlayerPrefs.GetInt("Settings.quality");
        else
        {
            quality = 2;
            PlayerPrefs.SetInt("Settings.quality", quality);
        }
        if (PlayerPrefs.HasKey("Settings.res"))
        {
            string res = PlayerPrefs.GetString("Settings.res");
            int width = int.Parse(res.Split('x')[0]);
            int height = int.Parse(res.Split('x')[1].Split('@')[0]);
            int refreshRate = int.Parse(res.Split('@')[1].Replace("Hz", ""));

            resolution = new Resolution { width = width, height = height, refreshRate = refreshRate };
            //Debug.Log(res + " vs " + resolution);
            //Screen.SetResolution(width, height, Screen.fullScreen, refreshRate);
        }
        else
        {
            resolution = Screen.currentResolution;
        }

        if (PlayerPrefs.HasKey("Settings.fullscreen"))
        {
            //Debug.Log(PlayerPrefs.GetInt("Settings.fullscreen"));
            fullscreen = PlayerPrefs.GetInt("Settings.fullscreen") > 0;
        }
        else
        {
            fullscreen = Screen.fullScreen;
        }
        if (PlayerPrefs.HasKey("Settings.fov"))
            fov = PlayerPrefs.GetFloat("Settings.fov");
        else
        {
            fov = mainCamera.fieldOfView;
            PlayerPrefs.SetFloat("Settings.fov", fov);
        }
        if (PlayerPrefs.HasKey("Settings.musicVolume"))
            musicVolume = PlayerPrefs.GetFloat("Settings.musicVolume");
        else
        {
            if (audioMixer.GetFloat("musicVolume", out musicVolume))
                PlayerPrefs.SetFloat("Settings.musicVolume", musicVolume);
            else
            {
                Debug.LogError("Could not find musicVolume param");
            }
        }
        if (PlayerPrefs.HasKey("Settings.sfxVolume"))
            sfxVolume = PlayerPrefs.GetFloat("Settings.sfxVolume");
        else
        {
            if (audioMixer.GetFloat("sfxVolume", out sfxVolume))
                PlayerPrefs.SetFloat("Settings.sfxVolume", sfxVolume);
            else
            {
                Debug.LogError("Could not find sfxVolume param");
            }
        }

        qualityDropdown.value = quality;
        qualityDropdown.RefreshShownValue();

        int resIndex = Array.FindIndex(resolutions, res => res.width == resolution.width
                                                           && res.height == resolution.height
                                                           && res.refreshRate == resolution.refreshRate);
        Debug.Log(resIndex);
        //debugText.text = "index: " + resIndex + " value: " + resolution;
        resolutionsDropdown.value = resIndex;
        resolutionsDropdown.RefreshShownValue();

        fullscreenToggle.isOn = fullscreen;

        fovField.text = "" + fov;
        fovSlider.value = fov;

        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;

        QualitySettings.SetQualityLevel(quality);
        Screen.SetResolution(resolution.width, resolution.height, fullscreen, resolution.refreshRate);
        mainCamera.fieldOfView = fov;
        audioMixer.SetFloat("musicVolume", GetCorrectVolume(musicVolume) * 80);
        audioMixer.SetFloat("sfxVolume", GetCorrectVolume(sfxVolume) * 80);

        OnChaOnChangeFieldOfView(fov);
    }

    private float GetCorrectVolume(float start)
    {
        float result = -Mathf.Abs(Mathf.Pow(start, 3));
        //Debug.Log(result);
        return result;
    }

    public void OnChaOnChangeFieldOfView(float fov)
    {
        if (OnChangeFov != null)
        {
            OnChangeFov(this, new SettingsFovEventArgs{Fov = fov});
        }
    }
}

public class SettingsFovEventArgs : EventArgs
{
    public float Fov { get; set; }
}
