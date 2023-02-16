using UnityEngine;
using UnityEngine.Audio;
using MikelW.Menus;

namespace MikelW.Profiles
{
    #region Volume Class
    /// <summary>
    /// Base Volume Settings
    /// </summary>
    [System.Serializable]
    public class Volume
    {
        [Tooltip("Name of exposed Volume parameter in AudioMixer")]
        public string name;
        [Tooltip("Saved Volume")]
        public float volume = 1f;
    }
    #endregion Volume Class

    #region Profile ScriptableObject
    /// <summary>
    /// Profile to contain players settings such as volume or fullscreen
    /// </summary>
    [CreateAssetMenu(menuName = "MikelW/Create Settings Profile")]
    public class SettingsProfile : ScriptableObject
    {
        #region Variables
        [Header("Base Save Settings")]
        [SerializeField]
        private bool saveInPlayerPrefs = true;
        [SerializeField]
        private string prefPrefix = "Settings_";

        [Header("Screen Settings")]
        [SerializeField]
        private bool fullScreen = true;
        [SerializeField]
        private int qualityIndex;
        [SerializeField]
        private int resolutionIndex;

        [Header("Audio Settings")]
        [SerializeField]
        private AudioMixer audioMixer;
        [SerializeField]
        private Volume[] volumeControl;
        #endregion Variables

        #region Audio
        public float LoadAudioLevels(string name)
        {
            float volume = 1f;

            if (!audioMixer)
            {
                Debug.LogError("There is no AudioMixer defined in the profiles file");
                return volume;
            }

            for (int i = 0; i < volumeControl.Length; i++)
            {
                if (volumeControl[i].name == name)
                {
                    if (saveInPlayerPrefs && PlayerPrefs.HasKey(prefPrefix + volumeControl[i].name))
                        volumeControl[i].volume = PlayerPrefs.GetFloat(prefPrefix + volumeControl[i].name);

                    audioMixer.SetFloat(volumeControl[i].name, Mathf.Log(volumeControl[i].volume) * 20f);
                    volume = volumeControl[i].volume;
                    break;
                }
            }
            return volume;
        }

        public void LoadAudioLevels()
        {
            if (!audioMixer)
            {
                Debug.LogError("There is no AudioMixer defined in the profiles file");
                return;
            }

            for (int i = 0; i < volumeControl.Length; i++)
            {
                if (saveInPlayerPrefs && PlayerPrefs.HasKey(prefPrefix + volumeControl[i].name))
                    volumeControl[i].volume = PlayerPrefs.GetFloat(prefPrefix + volumeControl[i].name);

                //set the mixer to match the volume
                audioMixer.SetFloat(volumeControl[i].name, Mathf.Log(volumeControl[i].volume) * 20f);
            }
        }

        public void SetAudioLevels(string name, float volume)
        {
            if (!audioMixer)
            {
                Debug.LogError("There is no AudioMixer defined in the profiles file");
                return;
            }

            for (int i = 0; i < volumeControl.Length; i++)
            {
                if (volumeControl[i].name == name)
                {
                    audioMixer.SetFloat(volumeControl[i].name, Mathf.Log(volume) * 20f);
                    volumeControl[i].volume = volume;
                    break;
                }
            }
        }

        public void SaveAudioLevels()
        {
            if (!audioMixer)
            {
                Debug.LogError("There is no AudioMixer defined in the profiles file");
                return;
            }

            float volume = 0f;
            for (int i = 0; i < volumeControl.Length; i++)
            {
                volume = volumeControl[i].volume;
                if (saveInPlayerPrefs)
                {
                    PlayerPrefs.SetFloat(prefPrefix + volumeControl[i].name, volume);
                }
                audioMixer.SetFloat(volumeControl[i].name, Mathf.Log(volume) * 20f);
                volumeControl[i].volume = volume;
            }
        }
        #endregion Audio

        #region FullScreen
        public void SaveFullScreen(bool saveIsFullscreen)
        {
            if (saveInPlayerPrefs)
                PlayerPrefs.SetInt(prefPrefix + "FullScreen", saveIsFullscreen ? 1 : 0);

            fullScreen = saveIsFullscreen;
        }

        public bool LoadFullScreen()
        {
            if (saveInPlayerPrefs && PlayerPrefs.HasKey(prefPrefix + "FullScreen"))
            {
                bool loadedBool = PlayerPrefs.GetInt(prefPrefix + "FullScreen") == 1 ? true : false;
                MenuFunctions.SetFullScreen(loadedBool);
                return loadedBool;
            }
            else
            {
                MenuFunctions.SetFullScreen(fullScreen);
                return fullScreen;
            }
        }
        #endregion FullScreen

        #region Resolution
        public void SaveResolution(int savedResolutionIndex)
        {
            if (saveInPlayerPrefs)
                PlayerPrefs.SetInt(prefPrefix + "Resolution", savedResolutionIndex);

            resolutionIndex = savedResolutionIndex;
        }

        public int LoadResolution()
        {
            if (saveInPlayerPrefs && PlayerPrefs.HasKey(prefPrefix + "Resolution"))
            {
                int loadedIndex = PlayerPrefs.GetInt(prefPrefix + "Resolution");
                MenuFunctions.SetResolution(loadedIndex);
                return loadedIndex;
            }
            else
            {
                MenuFunctions.SetResolution(resolutionIndex);
                return resolutionIndex;
            }
        }
        #endregion Resolution

        #region Quality
        public void SaveQuality(int savedQualityIndex)
        {
            if (saveInPlayerPrefs)
                PlayerPrefs.SetInt(prefPrefix + "Quality", savedQualityIndex);

            qualityIndex = savedQualityIndex;
        }

        public int LoadQuality()
        {
            if (saveInPlayerPrefs && PlayerPrefs.HasKey(prefPrefix + "Quality"))
            {
                int loadedIndex = PlayerPrefs.GetInt(prefPrefix + "Quality");
                MenuFunctions.SetQuality(loadedIndex);
                return loadedIndex;
            }
            else
            {
                MenuFunctions.SetQuality(qualityIndex);
                return qualityIndex;
            }
        }
        #endregion Quality
    }
    #endregion Profile ScriptableObject
}