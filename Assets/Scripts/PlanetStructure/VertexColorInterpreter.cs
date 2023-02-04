using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexColorInterpreter : MonoBehaviour
{
    public Color rootColor;
    public Color seaColor;
    public Color mountainColor;
    public Color cityColor;
    public Color townColor;
    public Color villageColor;
    public Color grassColor;
    public Color desertColor;

    public int populatedAreaResources = 3;
    public int desertResources = 1; // also mountains, if passable
    public bool mountainsCanBePassed = false;

    private int cityPopulation;
    private int townPopulation;
    private int villagePopulation;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<ModelConfiguration>().BeReady();
        cityPopulation = ModelConfiguration.values.maxPopulation;
        villagePopulation = 1;
        townPopulation = (cityPopulation + villagePopulation) / 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public NodeModel GetNodeModelFromColor(uint colorNumber)
    {
        string colorHtml = "#" + colorNumber.ToString("X6");
        Color color;
        if (!ColorUtility.TryParseHtmlString(colorHtml, out color))
        {
            Debug.Log($"Color not recognized: {colorNumber}");
            return new NodeModel();
        }        
        if (color == mountainColor)
        {
            return new NodeModel(impassable: !mountainsCanBePassed, resources: desertResources);
        }
        else if (color == grassColor)
        {
            return new NodeModel();
        }
        else if (color == seaColor)
        {
            return new NodeModel(impassable: true, resources: 0);
        }
        else if (color == desertColor)
        {
            return new NodeModel(resources: desertResources);
        }
        else if (color == cityColor)
        {
            return new NodeModel(population: cityPopulation, resources: populatedAreaResources);
        }
        else if (color == townColor)
        {
            return new NodeModel(population: townPopulation, resources: populatedAreaResources);
        }
        else if (color == villageColor)
        {
            return new NodeModel(population: villagePopulation, resources: populatedAreaResources);
        }
        else if (color == rootColor)
        {           
            NodeModel.Root = new();
            return NodeModel.Root;
        }
        else
        {
            Debug.Log($"Color not recognized: {colorNumber}");
            return new NodeModel();
        }
    }
}
