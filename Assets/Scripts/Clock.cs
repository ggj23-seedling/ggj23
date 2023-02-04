using System;
using System.Collections;
using System.Collections.Generic;

public enum Turn
{
    conversation,
    player,
    enemy,
}


public class Clock : Listenable<Clock>
{
    private static readonly Clock instance = new();
    
    public static Clock Instance()
    {
        return instance;
    }
    
    private Turn t;
    
    public Turn turn {
        get => t;
    }

    public void NextTurn()
    {
        switch (t)
        {
            case Turn.conversation:
                t = Turn.player;
                break;
            case Turn.player:
                t = Turn.enemy;
                break;
            case Turn.enemy:
                t = Turn.conversation;
                break;
        }
        NotifyListeners();
    }    
}
