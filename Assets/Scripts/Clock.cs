using System;
using System.Collections;
using System.Collections.Generic;

public enum Turn
{
    Conversation,
    Player,
    Enemy,
}


public class Clock : Listenable
{
    private static readonly Clock I = new();
    
    public static Clock Instance()
    {
        return I;
    }
    
    private Turn T;
    
    public Turn Turn {
        get => T;
    }

    public void NextTurn()
    {
        switch (T)
        {
            case Turn.Conversation:
                T = Turn.Player;
                break;
            case Turn.Player:
                T = Turn.Enemy;
                break;
            case Turn.Enemy:
                T = Turn.Conversation;
                break;
        }
        NotifyListeners();
    }    
}
