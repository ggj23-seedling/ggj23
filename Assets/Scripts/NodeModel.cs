using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;

public class NodeModel : Listenable<NodeModel>
{
    const int defaultResources = 50;
    const int defaultPopulation = 50;
    const int defaultAttack = 50;
    const int defaultDefense = 50;
    const int defaultExtraction = 50;
    const int defaultExtension = 50;

    int resources;
    int population;
    int attack;
    int defense;
    int extraction;
    int extension;

    List<NodeModel> neighbors;
    List<NodeModel> links = new();

    public NodeModel(
        int resources = defaultResources,
        int population = defaultPopulation,
        int attack = defaultAttack,
        int defense = defaultDefense,
        int extraction = defaultExtraction,
        int extension = defaultExtension
    )
    {
        this.resources = resources;
        this.population = population;
        this.attack = attack;
        this.defense = defense;
        this.extraction = extraction;
        this.extension = extension;
        neighbors = new List<NodeModel>();
    }

    public void LinkWith(NodeModel other)
    {
        if (this != other)
        {
            links.Add(other);
            other.links.Add(this);
        }
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
