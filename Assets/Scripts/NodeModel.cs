using System.Collections;
using System.Collections.Generic;

public class NodeModel : Listenable
{
    const int DefaultResources = 50;
    const int DefaultPopulation = 50;
    const int DefaultAttack = 50;
    const int DefaultDefense = 50;
    const int DefaultExtraction = 50;
    const int DefaultExtension = 50;

    int Resources;
    int Population;
    int Attack;
    int Defense;
    int Extraction;
    int Extension;

    List<NodeModel> Neighbors;
    List<NodeModel> Links = new();

    public NodeModel(
        int resources = DefaultResources,
        int population = DefaultPopulation,
        int attack = DefaultAttack,
        int defense = DefaultDefense,
        int extraction = DefaultExtraction,
        int extension = DefaultExtension,
        List<NodeModel> neighbors = null
    )
    {
        Resources = resources;
        Population = population;
        Attack = attack;
        Defense = defense;
        Extraction = extraction;
        Extension = extension;
        if (neighbors == null)
        {
            Neighbors = new();
        }
    }

    public void LinkWith(NodeModel other)
    {
        {
            Links.Add(other);
            other.Links.Add(this);
        }
    }
}
