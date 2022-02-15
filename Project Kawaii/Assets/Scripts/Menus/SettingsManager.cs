using UnityEngine;
using MikelW.Menus;
using TMPro;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private TMP_Dropdown quality;
    [SerializeField]
    private TMP_Dropdown reso;
    [SerializeField]
    private Toggle fsToggle;
    [SerializeField]
    private Profiles settingsProfile;

    public static Profiles profile { get; private set; }

    private void Awake()
    {
        if (settingsProfile)
            profile = settingsProfile;
        else
            Debug.LogError("A Profile needs to be added to MainMenuManager script, object: " + gameObject.name);
    }

    private void Start()
    {
        SettingsInitialization();
    }

    private void SettingsInitialization()
    {
        //Audio
        profile.LoadAudioLevels();

        //FullScreen
        MenuFunctions.PopulateFullscreenToggle(fsToggle);
        fsToggle.isOn = profile.LoadFullScreen();
        fsToggle.onValueChanged.AddListener(delegate { profile.SaveFullScreen(fsToggle.isOn); });
        fsToggle.onValueChanged.AddListener(delegate { MenuFunctions.SetFullScreen(fsToggle.isOn); });

        //Quality
        MenuFunctions.PopulateQualityPresetDropdown(quality);
        quality.value = profile.LoadQuality();
        quality.onValueChanged.AddListener(delegate { profile.SaveQuality(quality.value); });
        quality.onValueChanged.AddListener(delegate { MenuFunctions.SetQuality(quality.value); });

        //Resolution
        MenuFunctions.PopulateResolutionDropdown(reso);
        reso.value = profile.LoadResolution();
        reso.onValueChanged.AddListener(delegate { profile.SaveResolution(reso.value); });
        reso.onValueChanged.AddListener(delegate { MenuFunctions.SetResolution(reso.value); });
    }
}