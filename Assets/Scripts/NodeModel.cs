using System.Collections;
using System.Collections.Generic;

public class NodeModel : Listenable<NodeModel>
{
    int resources;
    int population;
    int attack;
    int defense;
    int extraction;
    int extension;

    List<NodeModel> neighbors;
    List<NodeModel> links = new();

    public NodeModel(
        int? resources = null,
        int? population = null,
        int? attack = null,
        int? defense = null,
        int? extraction = null,
        int? extension = null,
        List<NodeModel> neighbors = null
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
    }

    public void LinkWith(NodeModel other)
    {
        {
            links.Add(other);
            other.links.Add(this);
        }
    }
}
