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

    public int CityPopulation { get { return cityPopulation; } }
    public int TownPopulation { get { return townPopulation; } }
    public int VillagePopulation{ get { return villagePopulation; } }

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

    private Color? uintToColor(uint colorNumber)
    {
        string colorHtml = "#" + colorNumber.ToString("X6");
        Color color;
        if (!ColorUtility.TryParseHtmlString(colorHtml, out color)) {
            return null;
        }
        return color;
    }

    private bool AreClose(float x, float y)
    {
        return Mathf.Abs(x - y) < 0.1f;
    }

    public bool LooksLike(Vector4 rgba, Color color)
    {
        if (AreClose(color.r, rgba.x) &&
            AreClose(color.g, rgba.y) &&
            AreClose(color.b, rgba.z))
        {
            return true;
        }
        return false;
    }

    public NodeModel GetNodeModelFromColor(Vector4 color)
    {
        if (LooksLike(color, rootColor))
        {
            Debug.Log("Root node detected");
            NodeModel.Root = new();
            return NodeModel.Root;
        }
        else if (LooksLike(color, mountainColor))
        {
            // Debug.Log("Mountain detected");
            return new NodeModel(impassable: !mountainsCanBePassed, resources: desertResources);
        }
        else if (LooksLike(color, grassColor))
        {
            // Debug.Log("Grass detected");
            return new NodeModel();
        }
        else if (LooksLike(color, seaColor))
        {
            // Debug.Log("Sea detected");
            return new NodeModel(impassable: true, resources: 0);
        }
        else if (LooksLike(color, desertColor))
        {
            // Debug.Log("Desert detected");
            return new NodeModel(resources: desertResources);
        }
        else if (LooksLike(color, cityColor))
        {
            // Debug.Log("City detected");
            return new NodeModel(population: cityPopulation, resources: populatedAreaResources);
        }
        else if (LooksLike(color, townColor))
        {
            // Debug.Log("Town detected");
            return new NodeModel(population: townPopulation, resources: populatedAreaResources);
        }
        else if (LooksLike(color, villageColor))
        {
            // Debug.Log("Village detected");
            return new NodeModel(population: villagePopulation, resources: populatedAreaResources);
        }
        else 
        {
            Debug.Log($"Color not recognized: {color}");
            Debug.Log($"Valid colors: {mountainColor}, {grassColor}, {seaColor}, {desertColor}, {cityColor}, {townColor}, {villageColor}, {rootColor}");
            return new NodeModel();
        }
    }
}
