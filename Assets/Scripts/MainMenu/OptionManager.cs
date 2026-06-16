using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class OptionManager : Menu
{
    [Header("UI References")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Button restoreDefaultsButton;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource ambienceMusicSource;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private CanvasGroup optionsCanvasGroup;

    private Resolution[] resolutions;
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private PauseManager pauseManager;

    private bool openedFromPause;

    private const string PrefMasterVolume = "MasterVol";
    private const string PrefBGMVolume = "BGMVol";
    private const string PrefSFXVolume = "SFXVol";
    private const string PrefFullscreen = "Fullscreen";
    private const string PrefResolutionIndex = "ResolutionIndex";

    private const float DefaultVolume = 1f;
    private const float MinDecibels = -80f;
    private const float MaxDecibels = 0f;

    public static OptionManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one instance found. Destroying this instance.");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        
        resolutions = Screen.resolutions;

        if (audioMixer == null)
        {
            audioMixer = Resources.Load<AudioMixer>("MainMixer");
        }

        if (optionsCanvasGroup == null)
        {
            optionsCanvasGroup = GetComponent<CanvasGroup>();
        }
    }

    void Start()
    {
        SetMasterVolume(masterVolumeSlider.value);
        SetBGMVolume(bgmVolumeSlider.value);
        SetSFXVolume(sfxVolumeSlider.value);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        InitializeResolutionDropdown();
        LoadSettings();
        SubscribeUIEvents();
    }

    private void OnDisable()
    {
        UnsubscribeUIEvents();
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

    private void SubscribeUIEvents()
    {
        if (masterVolumeSlider != null)
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);

        if (bgmVolumeSlider != null)
            bgmVolumeSlider.onValueChanged.AddListener(SetBGMVolume);

        if (sfxVolumeSlider != null)
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    private void UnsubscribeUIEvents()
    {
        if (masterVolumeSlider != null)
            masterVolumeSlider.onValueChanged.RemoveListener(SetMasterVolume);

        if (bgmVolumeSlider != null)
            bgmVolumeSlider.onValueChanged.RemoveListener(SetBGMVolume);

        if (sfxVolumeSlider != null)
            sfxVolumeSlider.onValueChanged.RemoveListener(SetSFXVolume);
    }

    public void SetMasterVolume(float value)
    {
        float clamped = Mathf.Clamp01(value);

        if (audioMixer != null)
            audioMixer.SetFloat(PrefMasterVolume, Mathf.Lerp(MinDecibels, MaxDecibels, clamped));
        else
            AudioListener.volume = clamped;

        PlayerPrefs.SetFloat(PrefMasterVolume, clamped);
        PlayerPrefs.Save();
    }

    public void SetBGMVolume(float value)
    {
        float clamped = Mathf.Clamp01(value);

        if (audioMixer != null)
            audioMixer.SetFloat(PrefBGMVolume, Mathf.Lerp(MinDecibels, MaxDecibels, clamped));
        else if (backgroundMusicSource != null)
            backgroundMusicSource.volume = clamped;

        PlayerPrefs.SetFloat(PrefBGMVolume, clamped);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float value)
    {
        float clamped = Mathf.Clamp01(value);

        if (audioMixer != null)
            audioMixer.SetFloat(PrefSFXVolume, Mathf.Lerp(MinDecibels, MaxDecibels, clamped));
        else if (ambienceMusicSource != null)
            ambienceMusicSource.volume = clamped;

        PlayerPrefs.SetFloat(PrefSFXVolume, clamped);
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

    public void OpenFromPause()
    {
        openedFromPause = true;
        SetCanvasGroupVisibility(optionsCanvasGroup, true);
    }

    public void RestoreDefaults()
    {
        float defaultMasterVolume = DefaultVolume;
        float defaultBGMVolume = DefaultVolume;
        float defaultSFXVolume = DefaultVolume;
        bool defaultFullscreen = true;
        int defaultResolution = resolutions.Length - 1;

        SetMasterVolume(defaultMasterVolume);
        SetBGMVolume(defaultBGMVolume);
        SetSFXVolume(defaultSFXVolume);
        SetFullscreen(defaultFullscreen);
        SetResolution(defaultResolution);

        UpdateUI(defaultMasterVolume, defaultBGMVolume, defaultSFXVolume, defaultFullscreen, defaultResolution);
    }

    private void LoadSettings()
    {
        float masterVolume = PlayerPrefs.GetFloat(PrefMasterVolume, DefaultVolume);
        float bgmVolume = PlayerPrefs.GetFloat(PrefBGMVolume, DefaultVolume);
        float sfxVolume = PlayerPrefs.GetFloat(PrefSFXVolume, DefaultVolume);
        bool fullscreen = PlayerPrefs.GetInt(PrefFullscreen, Screen.fullScreen ? 1 : 0) == 1;
        int resolutionIndex = PlayerPrefs.GetInt(PrefResolutionIndex, resolutions.Length - 1);

        if (resolutionIndex < 0 || resolutionIndex >= resolutions.Length)
            resolutionIndex = resolutions.Length - 1;

        ApplyStoredAudioSettings(masterVolume, bgmVolume, sfxVolume);
        SetFullscreen(fullscreen);
        SetResolution(resolutionIndex);

        UpdateUI(masterVolume, bgmVolume, sfxVolume, fullscreen, resolutionIndex);
    }

    private void UpdateUI(float masterVolume, float bgmVolume, float sfxVolume, bool fullscreen, int resolutionIndex)
    {
        if (masterVolumeSlider != null)
            masterVolumeSlider.value = masterVolume;

        if (bgmVolumeSlider != null)
            bgmVolumeSlider.value = bgmVolume;

        if (sfxVolumeSlider != null)
            sfxVolumeSlider.value = sfxVolume;

        if (fullscreenToggle != null)
            fullscreenToggle.isOn = fullscreen;

        if (resolutionDropdown != null)
        {
            resolutionDropdown.value = Mathf.Clamp(resolutionIndex, 0, resolutions.Length - 1);
            resolutionDropdown.RefreshShownValue();
        }
    }

    private void ApplyStoredAudioSettings(float masterVolume, float bgmVolume, float sfxVolume)
    {
        SetMasterVolume(masterVolume);
        SetBGMVolume(bgmVolume);
        SetSFXVolume(sfxVolume);
    }

    public void CloseOptionsPanel()
    {
        SetCanvasGroupVisibility(optionsCanvasGroup, false);

        if (openedFromPause)
        {
            openedFromPause = false;
            if (pauseManager != null)
                pauseManager.ShowPausePanel();

            return;
        }

        if (mainMenu != null)
        {
            mainMenu.ActivateMenu();
            mainMenu.EnableMenuGraphics();
        }
    }

    public bool IsOpenedFromPause() => openedFromPause;

    private void SetCanvasGroupVisibility(CanvasGroup group, bool visible)
    {
        if (group == null) return;
        group.alpha = visible ? 1f : 0f;
        group.interactable = visible;
        group.blocksRaycasts = visible;
    }
}
