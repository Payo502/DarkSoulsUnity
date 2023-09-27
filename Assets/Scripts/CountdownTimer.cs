using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    public static float currentTime = 0;
    public float startingTime= 15;
    [SerializeField] TextMeshProUGUI countdownText;

    public static event Action OnTimerEnd;

    private void Start()
    {
        currentTime = startingTime;
    }

    private void Update()
    {
        currentTime -= Time.deltaTime;
        countdownText.text = "Time Left: " + currentTime.ToString("0");

        if (currentTime <= 10)
        {
            countdownText.color = Color.red;
        }

        if (currentTime <= 0)
        { 
            currentTime = 0;
            //Game over
            OnTimerEnd?.Invoke();
        }
    }
}
