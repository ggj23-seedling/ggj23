using Fungus;
using InkFungus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : Listenable<EnemyAi>
{
    private static readonly EnemyAi instance = new();
    
    public static EnemyAi Instance()    
    {
        return instance;
    }

    public void BeReady()
    {
        Clock.Instance().AddListener(OnNextTurn);
    }
    
    const int defaultAggressivity = 50;

    int aggressivity = defaultAggressivity;

    public void SetAggressivity(int value)
    {
        Debug.Log("Aggressivity changed to " + value);
        aggressivity = value;
    }

    public void OnNextTurn(Clock clock)
    {
        switch (clock.turn)
        {
            case Turn.conversation:
                Debug.Log($"Resuming conversation");
                Flowchart.BroadcastFungusMessage("conversation");
                break;         
            case Turn.enemy:
                bool willAttack = UnityEngine.Random.Range(0, 100) < aggressivity;
                Debug.Log($"Enemy will {(willAttack ? "attack" : "not attack")}");
                Clock.Instance().NextTurn();
                break;
            default:
                // It's not the AI's turn
                break;
        }
    }

}
