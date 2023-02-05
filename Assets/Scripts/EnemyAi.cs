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
    
    public int hostility;
    int nextThresholdIndex = 0;

    EnemyAi()
    {
        this.hostility = ModelConfiguration.values.initialHostility;        
    }
    
    public void Attack()
    {
        CheckHostilityThreshold();
        bool somethingHappened = false;
        foreach (NodeModel node in NodeModel.Connected)
        {
            if (node.IsVulnerable)
            {
                if (UnityEngine.Random.Range(0, 100) < ModelConfiguration.values.nativeAttackChance[hostility])
                {                    
                    int nativeAttackLevel = ModelConfiguration.values.nativeAttackLevels[hostility];
                    somethingHappened = somethingHappened || node.SufferAttack(nativeAttackLevel);
                }
            }
        }
        if (somethingHappened)
        {
            NotifyListeners();
        }
    }

    private void CheckHostilityThreshold()
    {
        foreach (int threshold in ModelConfiguration.values.hostilityThresholds)
        {
            if (threshold == nextThresholdIndex && NodeModel.Connected.Count >= threshold)
            {
                nextThresholdIndex++;
                hostility++;
                NotifyListeners();
            }
        }        
    }

    public void OnNextTurn(Clock clock)
    {
        switch (clock.Turn)
        {
            case Turn.conversation:
                Debug.Log($"Resuming conversation");
                Flowchart.BroadcastFungusMessage("conversation");
                break;         
            case Turn.enemy:
                Attack();
                Clock.Instance().NextTurn();
                break;
            default:
                // It's not the AI's turn
                break;
        }
    }

}
