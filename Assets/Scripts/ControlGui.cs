using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ControlGui : MonoBehaviour
{
    public GameObject resourcesPanel;
    public TMPro.TextMeshProUGUI resourceCounter;
    public TMPro.TextMeshProUGUI resourceMessage;
    public string extractionMessage = "EXPLOITATION";
    public float updateSpeed = 10f;

    private float resourcesTemporaryValue = 0;
    private int resourcesTargetValue = 0;
    private bool goingUp = true;

    // Start is called before the first frame update
    void Start()
    {
        resourcesPanel?.SetActive(false);
        Clock.Instance().AddListener(OnTurnChange);
        Economy.Instance().AddListener(OnEconomyChange);
        OnEconomyChange(Economy.Instance());
    }

    private void OnEconomyChange(Economy economy)
    {
        Debug.Log($"OnEconomyChange: {economy.GlobalResourcesBeforeLastExtraction} " +
            $"-> {economy.GlobalResources}");
        resourcesTemporaryValue = economy.GlobalResourcesBeforeLastExtraction;
        resourcesTargetValue = economy.GlobalResources;
        goingUp = resourcesTargetValue > resourcesTemporaryValue;
    }

    private void OnTurnChange(Clock clock)
    {        
        switch (clock.Turn)
        {
            case Turn.extraction:
                resourceMessage?.SetText(extractionMessage);
                break;
            case Turn.player:
                resourcesPanel?.SetActive(true); // Enables it the first time                
                break;
            default:
                resourceMessage?.SetText("");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float modifiedUpdateSpeed = updateSpeed;
        if (Math.Abs(resourcesTemporaryValue - resourcesTargetValue) > 5)
        {
            modifiedUpdateSpeed *= 2;
        }
        
        if (goingUp && resourcesTemporaryValue < resourcesTargetValue)
        {
            resourcesTemporaryValue += modifiedUpdateSpeed * Time.deltaTime;
        }
        else if (!goingUp && resourcesTemporaryValue > resourcesTargetValue)
        {
            resourcesTemporaryValue -= modifiedUpdateSpeed * Time.deltaTime;
        } else
        {
            resourcesTemporaryValue = resourcesTargetValue;
            resourceMessage?.SetText("");
        }
        resourceCounter.text = Mathf.CeilToInt(resourcesTemporaryValue).ToString();
    }
}
