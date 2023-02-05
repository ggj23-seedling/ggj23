using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuActionButton : MonoBehaviour
{
    public TMPro.TextMeshProUGUI buttonText;
    public NodeEvolution evolution;
    public Action callback;
    public Button button;

    private int cost;
    private bool available;
    private bool tooExpensive;
    
    // Start is called before the first frame update
    void Start()
    {
        Economy.Instance().BeReady();
        Economy.Instance().AddListener(OnEconomyChanged);
        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        if (available && !tooExpensive)
        {
            callback.Invoke();
        }
    }

    private void OnEconomyChanged(Economy economy)
    {
        tooExpensive = cost > economy.GlobalResources;
        Refresh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SetCost(int cost)
    {
        this.cost = cost;
        Refresh();
    }

    public void SetAvailable(bool available)
    {
        this.available = available;
        Refresh();
    }
    
    private void Refresh()
    {
        if (!available)
        {
            buttonText?.SetText("X");
        }
        else
        {
            buttonText?.SetText(cost.ToString());
        }
        if (buttonText != null)
        {
            buttonText.alpha = tooExpensive ? 0.5f : 1.0f;
        }
    }
}
