using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

namespace MikelW.Menus
{
    /// <summary>
    /// Class To Contain All Menu Related Functions
    /// </summary>
    public static class MenuFunctions
    {
        #region Loading Scenes
        /// <summary>
        /// Bool To See If A Scene Is Being Loaded
        /// </summary>
        public static bool isLoading;

        private static AsyncOperation loadingOperation;

        /// <summary>
        /// Loads A Scene Asynchronously
        /// </summary>
        /// <param name="sceneIndex"> Scene Index To Load </param>
        public static void LoadScene(int sceneIndex)
        {
            loadingOperation = SceneManager.LoadSceneAsync(sceneIndex);
            isLoading = true;
            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        /// <summary>
        /// Loads A Scene Asynchronously
        /// </summary>
        /// <param name="sceneName"> Scene Name To Load </param>
        public static void LoadScene(string sceneName)
        {
            loadingOperation = SceneManager.LoadSceneAsync(sceneName);
            isLoading = true;
            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        /// <summary>
        /// Returns The Loading Progress As A Float Between 0 And 1
        /// </summary>
        public static float GetLoadProgress()
        {
            return Mathf.Clamp01(loadingOperation.progress / 0.9f);
        }

        /// <summary>
        /// Returns Loading Percentage As A String
        /// </summary>
        public static string GetLoadPercentage()
        {
            return Mathf.Clamp01(loadingOperation.progress / 0.9f) * 100f + "%";
        }

        /// <summary>
        /// Set When A Loaded Scene Should Be Set Active Or Not.
        /// True = Loaded Scene Instantly Appears When Loading Is Complete.
        /// Scenes Do Not Appear While This Is Set To False.
        /// </summary>
        /// <param name="canActivate"> Bool To Set If New Scene Should Appear Or Not </param>
        public static void SetSceneActivation(bool canActivate)
        {
            if (loadingOperation != null)
                loadingOperation.allowSceneActivation = canActivate;
            else
                Debug.LogWarning("There is no scene being loaded, MenuFunctions.SetSceneActivation Must Be Called After A Load Has Started.");
        }

        private static void OnSceneChanged(Scene current, Scene next)
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
            isLoading = false;
        }
        #endregion Loading Scenes

        #region Settings
        #region Screen Settings
        /// <summary>
        /// The Index Of The Resolution Currently Active
        /// </summary>
        public static int currResolutionIndex;

        private static Resolution[] screenSizes;

        /// <summary>
        /// Toggles Fullscreen In The Project
        /// </summary>
        /// <param name="isFullscreen"> Should Project Be Full Screen? </param>
        public static void SetFullScreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
        }

        /// <summary>
        /// Adjusts Resolution Based On Given Index
        /// </summary>
        /// <param name="resolutionIndex"> Index Of Resolutions To Set The Project To </param>
        public static void SetResolution(int resolutionIndex)
        {
            if (screenSizes == null)
                GetResolutions();

            Resolution currResolution = screenSizes[resolutionIndex];

            Screen.SetResolution(currResolution.width, currResolution.height, Screen.fullScreen);
        }

        /// <summary>
        /// Gets All Available Resolutions On The Current System
        /// </summary>
        /// <returns> A List Of Resolutions As Strings Formatted As Width x Height e.g. 1920 x 1080 </returns>
        public static List<string> GetResolutions()
        {
            screenSizes = Screen.resolutions;
            List<string> options = new List<string>();

            currResolutionIndex = 0;
            for (int i = 0; i < screenSizes.Length; i++)
            {
                string option = screenSizes[i].width + " x " + screenSizes[i].height;
                options.Add(option);

                if (screenSizes[i].width == Screen.currentResolution.width && screenSizes[i].height == Screen.currentResolution.height)
                    currResolutionIndex = i;
            }

            return options;
        }

        /// <summary>
        /// Initializes Toggle Value To Match Fullscreen
        /// </summary>
        /// <param name="toggle"> The UI Toggle </param>
        public static void PopulateFullscreenToggle(Toggle toggle)
        {
            toggle.isOn = Screen.fullScreen;
        }

        /// <summary>
        /// Populates A Dropdown UI Element With Available Resolutions On The System
        /// </summary>
        /// <param name="dropdown"> The UI Element To Populate </param>
        public static void PopulateResolutionDropdown(Dropdown dropdown)
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(GetResolutions());
            dropdown.value = currResolutionIndex;
            dropdown.RefreshShownValue();
        }

        /// <summary>
        /// Populates A Dropdown UI Element With Available Resolutions On The System
        /// </summary>
        /// <param name="dropdown"> The UI Element To Populate </param>
        public static void PopulateResolutionDropdown(TMP_Dropdown dropdown)
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(GetResolutions());
            dropdown.value = currResolutionIndex;
            dropdown.RefreshShownValue();
        }
        #endregion Screen Settings

        //Add More In Depth Options? (AA, Texture Quality, ETC)
        #region Quality Settings
        /// <summary>
        /// Adjusts Quality Preset Based On Given Index
        /// </summary>
        /// <param name="index"> Index Of Presets To Set Project Quality To </param>
        public static void SetQuality(int index)
        {
            QualitySettings.SetQualityLevel(index);
        }

        /// <summary>
        /// Gets All Available Quality Presets Of The Current Project
        /// </summary>
        /// <returns> A List Of Quality Presets As Strings e.g. Low, Medium, High </returns>
        public static List<string> GetQualityPresets()
        {
            List<string> qPresets = new List<string>();

            foreach (string name in QualitySettings.names)
            {
                qPresets.Add(name);
            }

            return qPresets;
        }

        /// <summary>
        /// Populates A Dropdown UI Element With Available Presets Of The Project
        /// </summary>
        /// <param name="dropdown"> The UI Element To Populate </param>
        public static void PopulateQualityPresetDropdown(Dropdown dropdown)
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(GetQualityPresets());
            dropdown.value = QualitySettings.GetQualityLevel();
            dropdown.RefreshShownValue();
        }

        /// <summary>
        /// Populates A Dropdown UI Element With Available Presets Of The Project
        /// </summary>
        /// <param name="dropdown"> The UI Element To Populate </param>
        public static void PopulateQualityPresetDropdown(TMP_Dropdown dropdown)
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(GetQualityPresets());
            dropdown.value = QualitySettings.GetQualityLevel();
            dropdown.RefreshShownValue();
        }
        #endregion Quality Settings
        #endregion Settings

        /// <summary>
        /// Closes The Project
        /// </summary>
        public static void ExitProject()
        {
            Application.Quit();
        }
    }
}