using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public enum Turn
{
    intro,
    conversation,
    player,
    enemy,
    gameOver,
}



public class Clock : Listenable<Clock>
{
    private static readonly Clock instance = new();    
    
    public static Clock Instance()
    {
        return instance;
    }
    
    private Turn t = Turn.intro;
    
    public Turn turn {
        get => t;
    }

    public void NextTurn()
    {
        EnemyAi.Instance().BeReady(); // Initializes the AI if needed
        Turn previousTurn = turn;
        switch (t)
        {
            case Turn.intro:
                t = Turn.conversation;
                break;
            case Turn.conversation:
                t = Turn.player;
                break;
            case Turn.player:
                t = Turn.enemy;
                break;
            case Turn.enemy:
                t = Turn.conversation;
                break;
            case Turn.gameOver:
                t = Turn.intro;
                break;
        }
        Debug.Log($"Clock: {previousTurn} -> {turn}");
        NotifyListeners();
    }

    public void GameOver()
    {
        t = Turn.gameOver;
        NotifyListeners();
    }
}
