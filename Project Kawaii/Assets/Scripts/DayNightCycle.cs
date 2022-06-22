using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class DayNightCycle : MonoBehaviour
{
    //Seconds in real time for 10 minutes in game time
    public float tenMinuteRealTime = 10;
    public bool countTime = true;

    [SerializeField]
    private int startTime = 8;
    [SerializeField]
    private int endTime = 24;
    [SerializeField]
    private int noonTime = 12;
    [SerializeField]
    private int duskTime = 20;

    [SerializeField]
    private Color sunUINightColor;

    [SerializeField]
    private TMP_Text timeText;
    [SerializeField]
    private TMP_Text dayText;
    [SerializeField]
    private Image sunImage;

    private int currentHour;
    private int currentMinute;
    private float timePercent;
    private float timeCount;

    public UnityEvent onMinuteChanged;
    public UnityEvent onHourChanged;
    public UnityEvent onNoon;
    public UnityEvent onDusk;
    public UnityEvent onDayStart;
    public UnityEvent onDayEnd;

    private void Start()
    {
        currentHour = startTime;
        onMinuteChanged.AddListener(UpdateSunUIColor);
        onMinuteChanged.AddListener(UpdateTimeText);
        onDayStart.Invoke();
    }

    private void Update()
    {
        CalculateTime();
    }

    private void CalculateTime()
    {
        timePercent = (currentHour - startTime) / (endTime - startTime) + currentMinute / 600;

        if (countTime)
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

    private void UpdateSunUIColor()
    {
        sunImage.color = Color.Lerp(Color.white, sunUINightColor, timePercent);
    }

    private void UpdateTimeText()
    {
        if(currentHour < 13)
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

    private void UpdateDayText()
    {

    }
}