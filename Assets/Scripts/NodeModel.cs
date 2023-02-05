using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeModel : Listenable<NodeModel>
{
    static NodeModel root;
    public static NodeModel Root
    {
        get => root;
        set => root = value;
    }

    static readonly HashSet<NodeModel> connected = new();
    public static ISet<NodeModel> Connected
    {
        get => connected;
    }

    int resources; // resources per extraction
    int population; // natives
    int attack;
    int defense;
    int extraction; // extractions per turn
    int extension; // unused

    public bool warZone = false;
    private bool impassable;
    public bool CanPass => impassable;

    HashSet<NodeModel> neighbors;
    HashSet<NodeModel> links = new();

    public NodeModel(
        int? resources = null,
        int? population = null,
        int? attack = null,
        int? defense = null,
        int? extraction = null,
        int? extension = null,
        List<NodeModel> neighbors = null,
        bool impassable = false
    )
    {
        this.resources = resources ?? ModelConfiguration.values.initialResources;
        this.population = population ?? 0;
        this.attack = attack ?? ModelConfiguration.values.attackValues[0];
        this.defense = defense ?? ModelConfiguration.values.defenseValues[0];
        this.extraction = extraction ?? ModelConfiguration.values.extractionValues[0];
        this.extension = extension ?? ModelConfiguration.values.extensionValues[0];
        if (neighbors == null)
        {
            this.neighbors = new();
        }
        this.impassable = impassable;
        connected.Add(this);
    }

    public void LinkWith(NodeModel other)
    {
        links.Add(other);
        other.links.Add(this);
    }

    public bool CanExpand(NodeModel other)
    {
        return neighbors.Contains(other) && other.CanPass;
    }

    public void ExpandTo(NodeModel other)
    {
        LinkWith(other);
        E.Spend(E.ExpansionCost);
    }

    private Economy E => Economy.Instance();

    private bool CanEvolve(int[] values, int from)
    {
        return E.CanSpend(E.EvolutionCost) && from < values[^1];
    }

    private int Evolve(int[] values, int from)
    {
        E.Spend(E.EvolutionCost);
        foreach (int value in values)
        {
            if (value > from)
            {
                return value;
            }
        }
        Debug.LogWarning("Check CanEvolve() before calling Evolve()");
        return from;
    }
    public bool CanEvolveDefense => CanEvolve(ModelConfiguration.values.defenseValues, defense);
    public bool CanEvolveAttack => CanEvolve(ModelConfiguration.values.attackValues, attack);
    public bool CanEvolveExtraction => CanEvolve(ModelConfiguration.values.extractionValues, extraction);
    public bool CanEvolveExtension => CanEvolve(ModelConfiguration.values.extensionValues, extension);

    public bool IsVulnerable => links.Count <= ModelConfiguration.values.maxLinksForVulnerableNode;

    public void EvolveDefense()
    {
        defense = Evolve(ModelConfiguration.values.defenseValues, defense);
        NotifyListeners();
    }

    public void EvolveAttack()
    {
        attack = Evolve(ModelConfiguration.values.attackValues, attack);
        NotifyListeners();
    }

    public void EvolveExtraction()
    {
        extraction = Evolve(ModelConfiguration.values.extractionValues, extraction);
        NotifyListeners();
    }

    public void EvolveExtension()
    {
        extension = Evolve(ModelConfiguration.values.extensionValues, extension);
        NotifyListeners();
    }

    public void Unlink()
    {
        Debug.Log($"Unlinking a node with {links.Count} links");
        foreach (NodeModel link in links)
        {
            link.links.Remove(this);
        }
        links.Clear();
        connected.Remove(this);
        NotifyListeners();
    }

    public static void UnlinkAllDisconnected()
    {
        List<NodeModel> verified = new();
        List<NodeModel> toVerify = new() { Root };
        while (verified.Count < toVerify.Count)
        {
            NodeModel nextNode = toVerify[verified.Count];
            verified.Add(nextNode);
            foreach (NodeModel frontier in nextNode.links)
            {
                if (!toVerify.Contains(frontier))
                {
                    toVerify.Add(frontier);
                }
            }
        }
        foreach (NodeModel node in connected)
        {
            if (!verified.Contains(node))
            {
                node.Unlink();
            }
        }
    }

    public int Extract()
    {
        int extracted = 0;
        for (int i = 0; i < extraction; i++)
        {
            extracted += resources;
            if (resources > ModelConfiguration.values.minResources)
            {
                resources--; // depletion
                NotifyListeners();
            }
        }
        return extracted;
    }

    public bool SufferAttack(int nativeAttackLevel)
    {
        warZone = true;
        if (nativeAttackLevel > defense)
        {
            Unlink();
            return true;
        }
        NotifyListeners();
        return false;
    }

    public void AddNeighbour(NodeModel other)
    {
        if (this != other)
        {
            neighbors.Add(other);
            other.neighbors.Add(this);
        }
    }
}
