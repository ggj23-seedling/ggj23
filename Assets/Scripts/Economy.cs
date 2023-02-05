using Fungus;
using InkFungus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Economy : Listenable<Economy>
{
    private static readonly Economy instance = new();

    public static Economy Instance()
    {
        return instance;
    }

    private int globalResources;
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
                UpdateEconomy();
                Clock.Instance().NextTurn();
                break;
            default:
                // It's not the Economy's turn
                break;
        }
    }

    private void UpdateEconomy()
    {
        int globallyExtracted = 0;
        foreach (NodeModel node in NodeModel.Connected)
        {
            globallyExtracted += node.Extract();
        }
        GlobalResources = GlobalResources + globallyExtracted;
        Debug.Log($"Globally extracted {globallyExtracted} more resources. " +
            $"Total = {GlobalResources}");
    }

    public bool CanSpend(int amount) => GlobalResources >= amount;

    public void Spend(int amount)
    {
        GlobalResources -= amount;
        if (amount < 0)
        {
            Debug.LogWarning("Check CanSpend() before calling CanSpend()");
        }
    }

    public int ExpansionCost => ModelConfiguration.values.expansionCost;

    public int EvolutionCost => ModelConfiguration.values.evolutionCost;
}
