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
            Debug.Log("Model configuration is ready");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
