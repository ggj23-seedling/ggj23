using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelConfigurationValues
{
    public int initialResources;
    public int minResources;    
    public int maxPopulation; 
    public int[] attackValues;
    public int[] defenseValues;
    public int[] extractionValues;
    public int[] extensionValues;    

    // Costs in resources
    public int expansionCost;
    public int evolutionCost;

    // Hostility growth
    public int initialHostility;   
    public int[] hostilityThresholds;

    // What defines a vulnerable node
    public int maxLinksForVulnerableNode;

    // Index = hostility
    public int[] nativeAttackChance; // percentage per vulnerable node
    public int[] nativeAttackLevels;
}

public class ModelConfiguration : MonoBehaviour
{
    public static ModelConfigurationValues values = null;

    public int initialResources = 10;
    public int minResources = 1;
    public int maxPopulation = 5;
    public int[] attackValues = new int[] { 3, 6, 12 };
    public int[] defenseValues = new int[] { 2, 4, 8 };
    public int[] extractionValues = new int[] { 1, 2, 3 };
    public int[] extensionValues = new int[] { 1 };
    public int expansionCost = 20;
    public int evolutionCost = 5;
    public int initialHostility = 0;
    public int[] hostilityThresholds = new int[] { 3, 7, 10, 12 };
    public int maxLinksForVulnerableNode = 3;
    public int[] nativeAttackChance = new int[] { 0, 10, 20, 50, 90 }; 
    public int[] nativeAttackLevels = new int[] { 0, 1, 3, 6, 10 }; 

    // Start is called before the first frame update
    void Start()
    {
        BeReady();
    }
    
    public void BeReady()
    {
        if (values == null)
        {
            values.initialResources = initialResources;
            values.minResources = minResources;
            values.maxPopulation = maxPopulation;
            values.attackValues = attackValues;
            values.defenseValues = defenseValues;
            values.extractionValues = extractionValues;
            values.extensionValues = extensionValues;
            values.expansionCost = expansionCost;
            values.evolutionCost = evolutionCost;
            values.initialHostility = initialHostility;
            values.hostilityThresholds = hostilityThresholds;
            values.maxLinksForVulnerableNode = maxLinksForVulnerableNode;
            values.nativeAttackChance = nativeAttackChance;
            values.nativeAttackLevels = nativeAttackLevels;
            Debug.Log("Model configuration is ready");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
