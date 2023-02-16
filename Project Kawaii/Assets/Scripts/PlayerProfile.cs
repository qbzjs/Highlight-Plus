using UnityEngine;

namespace MikelW.Profiles
{
    /// <summary>
    /// Profile to contain players stats such as money or current day
    /// </summary>
    [CreateAssetMenu(menuName = "MikelW/Create Player Profile")]
    public class PlayerProfile : ScriptableObject
    {
        [Header("Base Save Settings")]
        [SerializeField]
        private bool saveInPlayerPrefs = true;
        [SerializeField]
        private string prefPrefix = "Player_";

        [Header("Player Settings")]
        [SerializeField]
        private int currentDay = 0;

        public void SaveDayInt(int savedDayInt)
        {
            if (saveInPlayerPrefs)
                PlayerPrefs.SetInt(prefPrefix + "Day", savedDayInt);

            currentDay = savedDayInt;
        }


        public int LoadDayInt()
        {
            if (saveInPlayerPrefs && PlayerPrefs.HasKey(prefPrefix + "Day"))
            {
                int loadedIndex = PlayerPrefs.GetInt(prefPrefix + "Day");
                return loadedIndex;
            }
            else
            {
                return currentDay;
            }
        }
    }
}