using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameClock : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI yearText;
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI seasonText;

    private void OnEnable()
    {
        EventHandler.AdvanceGameMinuteEvent += UpdateTime;
    }

    private void OnDisable()
    {
        EventHandler.AdvanceGameMinuteEvent -= UpdateTime;
    }

    private void UpdateTime(int gameYear,Season gameSeason,int gameDay,string gameDayOfWeek,int gameHour,int gameMinute,int gameSecond)
    {
        string gameYearStr = "Year "+gameYear.ToString();
        string gameSeasonStr = gameSeason.ToString();
        string gameDateStr = gameDayOfWeek + ". " + gameDay;
        string gameTimeStr = "";
        int tmpMinute=gameMinute - (gameMinute % 10);
        if (gameHour <= 12)
        {
            gameTimeStr = gameHour.ToString() + " : " + tmpMinute.ToString() + " am";
        }
        else
        {
            int tmpHour = gameHour - 12;
            gameTimeStr = tmpHour.ToString() + " : " + tmpMinute.ToString() + " pm";
        }
        yearText.SetText(gameYearStr);
        dateText.SetText(gameDateStr);
        timeText.SetText(gameTimeStr);
        seasonText.SetText(gameSeasonStr);
        
    }
}
