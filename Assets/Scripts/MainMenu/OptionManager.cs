using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class OptionManager : Menu
{
    [Header("UI References")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Button restoreDefaultsButton;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource ambienceMusicSource;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private CanvasGroup optionsCanvasGroup;

    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private PauseManager pauseManager;

    private bool openedFromPause;

    private bool isOpenFromMainMenu = false;

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
        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(this.gameObject);
        if (instance != null)
        {
            Debug.LogError("More than one instance found. Destroying this instance.");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        if (audioMixer == null)
        {
            audioMixer = Resources.Load<AudioMixer>("MainMixer");
        }

        if (optionsCanvasGroup == null)
        {
            optionsCanvasGroup = GetComponent<CanvasGroup>();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetMasterVolume(masterVolumeSlider.value);
        SetBGMVolume(bgmVolumeSlider.value);
        SetSFXVolume(sfxVolumeSlider.value);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        LoadSettings();
        SubscribeUIEvents();
    }

    private void OnDisable()
    {
        UnsubscribeUIEvents();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void SubscribeUIEvents()
    {
        if (masterVolumeSlider != null)
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);

        if (bgmVolumeSlider != null)
            bgmVolumeSlider.onValueChanged.AddListener(SetBGMVolume);

        if (sfxVolumeSlider != null)
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);

        if (fullscreenToggle != null)
            fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
    }

    private void UnsubscribeUIEvents()
    {
        if (masterVolumeSlider != null)
            masterVolumeSlider.onValueChanged.RemoveListener(SetMasterVolume);

        if (bgmVolumeSlider != null)
            bgmVolumeSlider.onValueChanged.RemoveListener(SetBGMVolume);

        if (sfxVolumeSlider != null)
            sfxVolumeSlider.onValueChanged.RemoveListener(SetSFXVolume);

        if (fullscreenToggle != null)
            fullscreenToggle.onValueChanged.RemoveListener(SetFullscreen);
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
        Screen.fullScreenMode = isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        PlayerPrefs.SetInt(PrefFullscreen, isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void OpenFromPause()
    {
        openedFromPause = true;
        isOpenFromMainMenu = false;
        SetCanvasGroupVisibility(optionsCanvasGroup, true);
    }

        public void OpenFromMainMenu()
    {
        openedFromPause = false;
        isOpenFromMainMenu = true;
        SetCanvasGroupVisibility(optionsCanvasGroup, true);
        
        LoadSettings();
        SubscribeUIEvents();
    }

    public void RestoreDefaults()
    {
        float defaultMasterVolume = DefaultVolume;
        float defaultBGMVolume = DefaultVolume;
        float defaultSFXVolume = DefaultVolume;
        bool defaultFullscreen = true;

        SetMasterVolume(defaultMasterVolume);
        SetBGMVolume(defaultBGMVolume);
        SetSFXVolume(defaultSFXVolume);
        SetFullscreen(defaultFullscreen);

        UpdateUI(defaultMasterVolume, defaultBGMVolume, defaultSFXVolume, defaultFullscreen);
    }

    private void LoadSettings()
    {
        float masterVolume = PlayerPrefs.GetFloat(PrefMasterVolume, DefaultVolume);
        float bgmVolume = PlayerPrefs.GetFloat(PrefBGMVolume, DefaultVolume);
        float sfxVolume = PlayerPrefs.GetFloat(PrefSFXVolume, DefaultVolume);
        bool fullscreen = PlayerPrefs.GetInt(PrefFullscreen, Screen.fullScreen ? 1 : 0) == 1;


        ApplyStoredAudioSettings(masterVolume, bgmVolume, sfxVolume);
        SetFullscreen(fullscreen);

        UpdateUI(masterVolume, bgmVolume, sfxVolume, fullscreen);
    }

    private void UpdateUI(float masterVolume, float bgmVolume, float sfxVolume, bool fullscreen)
    {
        if (masterVolumeSlider != null)
            masterVolumeSlider.value = masterVolume;

        if (bgmVolumeSlider != null)
            bgmVolumeSlider.value = bgmVolume;

        if (sfxVolumeSlider != null)
            sfxVolumeSlider.value = sfxVolume;

        if (fullscreenToggle != null)
            fullscreenToggle.isOn = fullscreen;
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

    if (mainMenu == null && MainMenu.instance != null)
    {
        mainMenu = MainMenu.instance;
    }

    if (mainMenu != null)
    {
        mainMenu.ActivateMenu();
        mainMenu.EnableMenuGraphics();
    }
    else
    {
        Debug.LogWarning("MainMenu reference missing - options closed but menu may not reactivate properly.");
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
