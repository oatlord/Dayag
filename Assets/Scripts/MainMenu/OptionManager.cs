using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : Menu
{
    [Header("UI References")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Dropdown qualityDropdown;
    [SerializeField] private Button restoreDefaultsButton;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource ambienceMusicSource;

    private Resolution[] resolutions;

    private const string PrefMasterVolume = "Pref_MasterVolume";
    private const string PrefMusicVolume = "Pref_MusicVolume";
    private const string PrefFullscreen = "Pref_Fullscreen";
    private const string PrefResolutionIndex = "Pref_ResolutionIndex";
    private const string PrefQualityLevel = "Pref_QualityLevel";

    private void Awake()
    {
        resolutions = Screen.resolutions;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        InitializeResolutionDropdown();
        InitializeQualityDropdown();
        LoadSettings();
    }

    private void InitializeResolutionDropdown()
    {
        if (resolutionDropdown == null || resolutions == null || resolutions.Length == 0) return;

        resolutionDropdown.ClearOptions();
        var options = new List<string>(resolutions.Length);

        for (int i = 0; i < resolutions.Length; i++)
        {
            Resolution resolution = resolutions[i];
            options.Add($"{resolution.width} x {resolution.height} @ {resolution.refreshRate}Hz");
        }

        resolutionDropdown.AddOptions(options);
    }

    private void InitializeQualityDropdown()
    {
        if (qualityDropdown == null) return;

        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new List<string>(QualitySettings.names));
    }

    public void SetMasterVolume(float value)
    {
        float clamped = Mathf.Clamp01(value);
        AudioListener.volume = clamped;
        PlayerPrefs.SetFloat(PrefMasterVolume, clamped);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float value)
    {
        float clamped = Mathf.Clamp01(value);

        if (backgroundMusicSource != null)
            backgroundMusicSource.volume = clamped;

        if (ambienceMusicSource != null)
            ambienceMusicSource.volume = clamped;

        PlayerPrefs.SetFloat(PrefMusicVolume, clamped);
        PlayerPrefs.Save();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt(PrefFullscreen, isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetResolution(int index)
    {
        if (resolutions == null || resolutions.Length == 0) return;
        if (index < 0 || index >= resolutions.Length) return;

        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode, resolution.refreshRate);
        PlayerPrefs.SetInt(PrefResolutionIndex, index);
        PlayerPrefs.Save();
    }

    public void SetQuality(int index)
    {
        if (index < 0 || index >= QualitySettings.names.Length) return;

        QualitySettings.SetQualityLevel(index, true);
        PlayerPrefs.SetInt(PrefQualityLevel, index);
        PlayerPrefs.Save();
    }

    public void RestoreDefaults()
    {
        float defaultMasterVolume = 1f;
        float defaultMusicVolume = 1f;
        bool defaultFullscreen = true;
        int defaultQuality = QualitySettings.names.Length - 1;
        int defaultResolution = resolutions.Length - 1;

        SetMasterVolume(defaultMasterVolume);
        SetMusicVolume(defaultMusicVolume);
        SetFullscreen(defaultFullscreen);
        SetQuality(defaultQuality);
        SetResolution(defaultResolution);

        UpdateUI(defaultMasterVolume, defaultMusicVolume, defaultFullscreen, defaultResolution, defaultQuality);
    }

    private void LoadSettings()
    {
        float masterVolume = PlayerPrefs.GetFloat(PrefMasterVolume, AudioListener.volume);
        float musicVolume = PlayerPrefs.GetFloat(PrefMusicVolume, 1f);
        bool fullscreen = PlayerPrefs.GetInt(PrefFullscreen, Screen.fullScreen ? 1 : 0) == 1;
        int resolutionIndex = PlayerPrefs.GetInt(PrefResolutionIndex, resolutions.Length - 1);
        int qualityIndex = PlayerPrefs.GetInt(PrefQualityLevel, QualitySettings.GetQualityLevel());

        if (resolutionIndex < 0 || resolutionIndex >= resolutions.Length)
            resolutionIndex = resolutions.Length - 1;

        if (qualityIndex < 0 || qualityIndex >= QualitySettings.names.Length)
            qualityIndex = QualitySettings.GetQualityLevel();

        SetMasterVolume(masterVolume);
        SetMusicVolume(musicVolume);
        SetFullscreen(fullscreen);
        SetQuality(qualityIndex);
        SetResolution(resolutionIndex);

        UpdateUI(masterVolume, musicVolume, fullscreen, resolutionIndex, qualityIndex);
    }

    private void UpdateUI(float masterVolume, float musicVolume, bool fullscreen, int resolutionIndex, int qualityIndex)
    {
        if (masterVolumeSlider != null)
            masterVolumeSlider.value = masterVolume;

        if (musicVolumeSlider != null)
            musicVolumeSlider.value = musicVolume;

        if (fullscreenToggle != null)
            fullscreenToggle.isOn = fullscreen;

        if (resolutionDropdown != null)
        {
            resolutionDropdown.value = Mathf.Clamp(resolutionIndex, 0, resolutions.Length - 1);
            resolutionDropdown.RefreshShownValue();
        }

        if (qualityDropdown != null)
        {
            qualityDropdown.value = Mathf.Clamp(qualityIndex, 0, QualitySettings.names.Length - 1);
            qualityDropdown.RefreshShownValue();
        }
    }

    public void CloseOptionsPanel()
    {
        gameObject.SetActive(false);
    }
}
