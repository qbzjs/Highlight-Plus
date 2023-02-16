using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using MikelW.Profiles;

namespace MikelW.World
{
    public class DayNightCycle : MonoBehaviour
    {
        #region Variables
        //Seconds in real time for 10 minutes in game time
        public float tenMinuteRealTime = 10;
        public bool countTime = true;

        public PlayerProfile playerProfile;

        [SerializeField]
        private float fadeOutDelay = 2;
        [SerializeField]
        private int startTime = 8;
        [SerializeField]
        private int endTime = 24;
        [SerializeField]
        private int noonTime = 12;
        [SerializeField]
        private int duskTime = 20;

        [SerializeField]
        private TMP_Text timeText;
        [SerializeField]
        private TMP_Text dayText;
        [SerializeField]
        private TMP_Text endDayText;
        [SerializeField]
        private Image darknessUIImage;


        [Header("PRIVATE SETTINGS FOR DEBUGGING ONLY")]
        [SerializeField]
        private int currentDay = 1;
        [SerializeField]
        private int currentHour;
        [SerializeField]
        private int currentMinute;
        [SerializeField]
        private float timePercent;
        [SerializeField]
        private float timeCount;

        public UnityEvent onMinuteChanged;
        public UnityEvent onHourChanged;
        public UnityEvent onNoon;
        public UnityEvent onDusk;
        public UnityEvent onDayStart;
        public UnityEvent onDayEnd;
        public UnityEvent onDayEndFade;

        #endregion Variables

        #region Unity Methods
        private void Awake()
        {
            onMinuteChanged.AddListener(UpdateTimeUI);
            onDayEnd.AddListener(EndDay);
        }

        private void Start()
        {
            StartDay();
        }

        private void Update()
        {
            if (countTime)
                CalculateTime();
        }
        #endregion Unity Methods

        #region Private Methods
        private void CalculateTime()
        {
            timePercent = ((currentHour - startTime) + ((float)currentMinute / 60)) / (endTime - startTime);
            //timePercent = ((float)(currentHour - startTime) / (endTime - startTime)) + ((float)currentMinute / ((endTime - startTime) * 60));
            timeCount += Time.deltaTime;

            if (timeCount >= tenMinuteRealTime)
            {
                timeCount = 0;
                currentMinute += 10;
                if (currentMinute >= 60)
                {
                    currentHour += 1;
                    currentMinute = 0;
                    if (currentHour == noonTime)
                    {
                        onNoon.Invoke();
                    }
                    else if (currentHour == duskTime)
                    {
                        onDusk.Invoke();
                    }
                    else if (currentHour == endTime)
                    {
                        onDayEnd.Invoke();
                    }
                    else
                    {
                        onHourChanged.Invoke();
                    }
                }
                onMinuteChanged.Invoke();
            }
        }

        private void UpdateTimeUI()
        {
            Color temp = Color.black;
            temp.a = Mathf.Lerp(0, 1, timePercent);
            darknessUIImage.color = temp;

            if (currentHour < 13)
                timeText.text = currentHour + ":" + currentMinute;
            else
                timeText.text = (currentHour - 12) + ":" + currentMinute;

            if (currentMinute == 0)
                timeText.text += "0";

            if (currentHour < 12)
                timeText.text += " AM";
            else
                timeText.text += " PM";
        }
        #endregion Private Methods

        #region Public Methods
        public void StartDay()
        {
            //Initialize game day, load game here
            if (playerProfile)
            {
                currentDay = playerProfile.LoadDayInt();
            }

            currentDay += 1;
            dayText.text = "Day " + currentDay;
            currentHour = startTime;
            currentMinute = 0;
            timePercent = 0;
            timeCount = 0;
            UpdateTimeUI();
            countTime = true;

            onDayStart.Invoke();
        }

        public void EndDay()
        {
            //Force player to bed, progress day, save game here
            countTime = false;
            playerProfile.SaveDayInt(currentDay);
            endDayText.text = "Day " + currentDay + " Completed!";
            StartCoroutine(FadeOutDelay(fadeOutDelay));
        }

        IEnumerator FadeOutDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            onDayEndFade.Invoke();
        }
        #endregion Public Methods
    }
}