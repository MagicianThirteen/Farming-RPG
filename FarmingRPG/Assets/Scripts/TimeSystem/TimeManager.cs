using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager :SingletonMonobehaviour<TimeManager>
{
    private int gameYear = 1;
    private Season gameSeason = Season.Spring;
    private int gameDay = 1;
    private int gameHour = 6;
    private int gameMinute = 30;
    private int gameSecond = 0;
    private string gameDayOfWeek = "Mon";
    private bool gameClockPaused = false;
    private float gameTick = 0f;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        EventHandler.CallAdvanceGameMinuteEvent(gameYear,gameSeason,gameDay,gameDayOfWeek,gameHour,gameMinute,gameSecond);
    }

    private void Update()
    {
        if (!gameClockPaused)
        {
            GameTick();
        }
    }

    private void GameTick()
    {
        gameTick += Time.deltaTime;
        if (gameTick > Settings.secondsPerGameSecond)
        {
            gameTick = 0;
            UpdateGameSecond();
            Debug.Log($"Game Year:{gameYear}+Game Season:{gameSeason.ToString()}" +
                      $"Game Day:{gameDay}+Game Hour:{gameHour}" +
                      $"Game Minute:{gameMinute}" +
                      $"Game Second:{gameSecond}" +
                      $"Game Week:{gameDayOfWeek}");
        }
    }

    private void UpdateGameSecond()
    {
        gameSecond++;
        if (gameSecond > 59)
        {
            gameSecond = 0;
            gameMinute++;
            //调用分事件
            EventHandler.CallAdvanceGameMinuteEvent(gameYear,gameSeason,gameDay,gameDayOfWeek,gameHour,gameMinute,gameSecond);
            if (gameMinute > 59)
            {
                gameMinute = 0;
                gameHour++;
                //调用时事件
                EventHandler.CallAdvanceGameHourEvent(gameYear,gameSeason,gameDay,gameDayOfWeek,gameHour,gameMinute,gameSecond);
                if (gameHour > 23)
                {
                    gameHour = 0;
                    gameDay++;
                    //调用天事件
                    EventHandler.CallAdvanceGameDayEvent(gameYear,gameSeason,gameDay,gameDayOfWeek,gameHour,gameMinute,gameSecond);
                    //转换成周
                    gameDayOfWeek = GetDayOfWeek();
                    if (gameDay > 30)
                    {
                        gameDay = 1;
                        int gs = (int) gameSeason;
                        gs++;
                        gameSeason = (Season) gs;
                        //调用季节事件
                        EventHandler.CallAdvanceGameSeasonEvent(gameYear,gameSeason,gameDay,gameDayOfWeek,gameHour,gameMinute,gameSecond);
                        if (gs > 3)
                        {
                            gs = 0;
                            gameYear++;
                            //调用年事件
                            EventHandler.CallAdvanceGameYearEvent(gameYear,gameSeason,gameDay,gameDayOfWeek,gameHour,gameMinute,gameSecond);
                            if (gameYear > 999)
                            {
                                gameYear = 1;
                                
                            }
                        }
                    }
                }
            }
        }
    }
    
    private string GetDayOfWeek()
    {
        int totalDay = (int) gameSeason * 30 + gameDay;
        int week = totalDay % 7;
        switch (week)
        {
            case 1:
                return "Mon";
            case 2:
                return "Tue";
            case 3:
                return "Wed";
            case 4:
                return "Thu";
            case 5:
                return "Fri";
            case 6: 
                return "Sat";
            case 0:
                return "Sun";
            default:
                return "";
        }
    }
}


