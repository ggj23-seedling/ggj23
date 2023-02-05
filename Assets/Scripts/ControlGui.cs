using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ControlGui : MonoBehaviour
{
    public GameObject resourcesPanel;
    public GameObject actionMenu;
    public TMPro.TextMeshProUGUI resourceCounter;
    public TMPro.TextMeshProUGUI resourceMessage;
    public string extractionMessage = "EXPLOITATION";
    public float updateSpeed = 10f;

    private float resourcesTemporaryValue = 0;
    private int resourcesTargetValue = 0;
    private bool goingUp = true;
    
    public static float leftEdgeOfMenu = float.PositiveInfinity;

    // Start is called before the first frame update
    void Start()
    {
        resourcesPanel?.SetActive(false);
        DisplayMenu(false);
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
                break;
            case Turn.player:
                resourcesPanel?.SetActive(true); // Enables it the first time                
                break;
            case Turn.enemy:
            case Turn.conversation:
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
        
        resourcesTemporaryValue = Mathf.MoveTowards(resourcesTemporaryValue, resourcesTargetValue, 
            modifiedUpdateSpeed * Time.deltaTime);
        if (goingUp && resourcesTemporaryValue < resourcesTargetValue)
        {
            resourceMessage?.SetText(extractionMessage);
        }
        else if (!goingUp && resourcesTemporaryValue > resourcesTargetValue)
        {
            resourceMessage?.SetText("");
        } 
        else
        {
            resourcesTemporaryValue = resourcesTargetValue;
            resourceCounter.text = resourcesTemporaryValue.ToString();
            resourceMessage?.SetText("");
            return;
        }
        resourceCounter.text = Mathf.CeilToInt(resourcesTemporaryValue).ToString();
    }

    public void DisplayMenu(bool display)
    {
        actionMenu?.SetActive(display);
        leftEdgeOfMenu = display ? 0.6f * Screen.width : float.PositiveInfinity;
    }
}
