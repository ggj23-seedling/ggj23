using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ControlGui : MonoBehaviour
{
    public GameObject resourcesPanel;
    public TMPro.TextMeshProUGUI resourceCounter;
    public float updateSpeed = 50f;

    private float resourcesTemporaryValue = 0;
    private int resourcesTargetValue = 0;
    private bool goingUp = true;

    // Start is called before the first frame update
    void Start()
    {
        resourcesPanel?.SetActive(false);
        Clock.Instance().AddListener(OnTurnChange);
        Economy.Instance().AddListener(OnEconomyChange);
    }

    private void OnEconomyChange(Economy economy)
    {
        resourcesTargetValue = economy.GlobalResources;
        goingUp = resourcesTargetValue > resourcesTemporaryValue;
    }

    private void OnTurnChange(Clock clock)
    {
        if (clock.CanClickOnNodes)
        {
            resourcesPanel?.SetActive(true);
            OnEconomyChange(Economy.Instance());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (goingUp && resourcesTemporaryValue < resourcesTargetValue)
        {
            resourcesTemporaryValue += updateSpeed * Time.deltaTime;
        }
        if (!goingUp && resourcesTemporaryValue > resourcesTargetValue)
        {
            resourcesTemporaryValue -= updateSpeed * Time.deltaTime;
        }
        resourceCounter.text = Mathf.RoundToInt(resourcesTemporaryValue).ToString();
    }
}
