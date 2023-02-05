using Fungus;
using InkFungus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Economy : Listenable<Economy>
{
    const int InitialBudget = 50; // TODO Get it from ModelConfiguration
    
    private static readonly Economy instance = new();

    public static Economy Instance()
    {
        return instance;
    }

    private int globalResourcesBeforeLastExtraction = InitialBudget;
    public int GlobalResourcesBeforeLastExtraction
    {
        get => globalResourcesBeforeLastExtraction;
        set
        {
            globalResourcesBeforeLastExtraction = value;
            NotifyListeners();
        }
    }

    private int globalResources = InitialBudget;
    public int GlobalResources
    {
        get => globalResources;
        set
        {
            globalResources = value;
            NotifyListeners();
        }
    }

    public void BeReady()
    {
        Clock.Instance().AddListener(OnNextTurn);
    }

    public void OnNextTurn(Clock clock)
    {
        switch (clock.Turn)
        {
            case Turn.extraction:
                Debug.Log($"Extraction");                
                Extract();
                Clock.Instance().NextTurn();
                break;
            default:
                // It's not the Economy's turn
                break;
        }
    }

    private void Extract()
    {
        int globallyExtracted = 0;
        foreach (NodeModel node in NodeModel.Connected)
        {
            globallyExtracted += node.Extract();
        }
        GlobalResources = GlobalResources + globallyExtracted;
        // Doesn't update GlobalResourcesBeforeLastExtraction
        Debug.Log($"Globally extracted {globallyExtracted} more resources. " +
            $"Total = {GlobalResources}");
    }

    public bool CanSpend(int amount) => GlobalResources >= amount;

    public void Spend(int amount, bool notifyListeners = false)
    {        
        globalResources -= amount;
        globalResourcesBeforeLastExtraction = GlobalResources;
        if (notifyListeners)
        {
            NotifyListeners();
        }
        if (amount < 0)
        {
            Debug.LogWarning("Check CanSpend() before calling CanSpend()");
        }
    }

    public int ExpansionCost => ModelConfiguration.values.expansionCost;

    public int EvolutionCost => ModelConfiguration.values.evolutionCost;
}
