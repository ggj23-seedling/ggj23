using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : Listenable<EnemyAi>
{
    private static readonly EnemyAi instance = new();
    
    private bool initialized = false;
    
    public static EnemyAi Instance()    
    {
        if (instance.initialized)
        {
            Clock.Instance().AddListener(instance.OnNextTurn);
        }
        return instance;
    }


    const int defaultAggressivity = 50;

    int aggressivity = defaultAggressivity;

    private void OnNextTurn(Clock listenable)
    {
        bool willAttack = UnityEngine.Random.Range(0, 100) < aggressivity;
        Debug.Log($"Enemy will {(willAttack ? "attack" : "not attack")}");
        Clock.Instance().NextTurn();
    }

}
