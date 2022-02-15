using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MikelW.Menus
{
    [RequireComponent(typeof(Slider))]
    public class AudioSliderController : MonoBehaviour
    {
        private Slider slider { get { return GetComponent<Slider>(); } }

        [Tooltip("This is the name of the exposed parameter in your AudioMixer")]
        [SerializeField]
        private string volumeName = "Enter Volume Parameter Name Here";

        [Tooltip("Text to set volume percentage to")]
        [SerializeField]
        private TMP_Text volumeText;

        private void Start()
        {
            ResetSliderValue();

            slider.onValueChanged.AddListener(delegate { UpdateValueOnChange(slider.value); });
            slider.onValueChanged.AddListener(delegate { ApplyChanges(); });
        }

        public void ResetSliderValue()
        {
            float volume = SettingsManager.profile.LoadAudioLevels(volumeName);

            UpdateValueOnChange(volume);
            slider.value = volume;
        }

        public void UpdateValueOnChange(float value)
        {
            if (volumeText != null)
                volumeText.text = Mathf.Round(value * 100.0f).ToString() + "%";

            SettingsManager.profile.SetAudioLevels(volumeName, value);
        }

        public void ApplyChanges()
        {
            SettingsManager.profile.SaveAudioLevels();
        }
    }
}