using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource[] sfxSources;
    [Header("Settings Panel")]
    public GameObject settingsPanel;

    [Header("Audio Toggles")]
    public Toggle musicToggle;
    public Toggle sfxToggle;

    [Header("Language")]
    public TMP_Dropdown languageDropdown;

    [Header("Other")]
    public Button exitButton;

    private const string MusicPrefKey = "MusicEnabled";
    private const string SfxPrefKey = "SfxEnabled";

    void Start()
    {
        // Load saved preferences
        musicToggle.isOn = PlayerPrefs.GetInt(MusicPrefKey, 1) == 1;
        sfxToggle.isOn = PlayerPrefs.GetInt(SfxPrefKey, 1) == 1;

        // Apply audio settings
        ApplyAudioSettings();

        // Add listeners
        musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
        sfxToggle.onValueChanged.AddListener(OnSfxToggleChanged);
        languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
        exitButton.onClick.AddListener(OnExitButtonClicked);

        // Hide panel initially
        settingsPanel.SetActive(false);
    }

    public void ToggleSettingsPanel()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    private void OnMusicToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt(MusicPrefKey, isOn ? 1 : 0);
        ApplyAudioSettings();
    }

    private void OnSfxToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt(SfxPrefKey, isOn ? 1 : 0);
        ApplyAudioSettings();
    }

    private void ApplyAudioSettings()
    {
        if (musicSource != null)
            musicSource.mute = !musicToggle.isOn;

        // Add similar logic if you want to control SFX separately
        foreach (var sfx in sfxSources)
            sfx.mute = !sfxToggle.isOn;
    }

    private void OnLanguageChanged(int index)
    {
        // TODO: Integrate with localization system
        Debug.Log($"Language changed to index: {index}");
    }

    private void OnExitButtonClicked()
    {
        settingsPanel.SetActive(false);
    }
}