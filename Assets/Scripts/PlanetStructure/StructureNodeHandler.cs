using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PlanetStructureTypes;

public class StructureNodeHandler : MonoBehaviour
{
    enum HighlightState
    {
        NONE, // niente
        HINT, // fase di collegamento, nodo destinazione suggerito
        HIGHLIGHT, // fase di collegamento, nodo di partenza confermato
        ACTIVATED, // collegato alla root
        ACTIVATED_HIGHLIGHT // collegato alla root, in fase di collegamento, come nodo di partenza
    }

    private StructureNode _nodeData = null;
    public StructureNode nodeData { get { return _nodeData; } }

    [SerializeField]
    private GameObject baseCosmeticObject;
    [SerializeField]
    private GameObject hintHighlightCosmeticObject;
    [SerializeField]
    private GameObject highlightCosmeticObject;
    [SerializeField]
    private GameObject activateCosmeticObject;
    [SerializeField]
    private GameObject activateHighlightCosmeticObject;

    private Collider collider;

    HighlightState highlightState = HighlightState.NONE;

    private void Awake()
    {
        RefreshView();

        collider = GetComponent<Collider>();
        if (collider == null) collider = GetComponentInChildren<Collider>();
    }

    public void InitializeNodeData (StructureNode nodeData)
    {
        if (this._nodeData != null)
        {
            Debug.LogError("Initializing twice the same StructureNodeHandler");
        }
        this._nodeData = nodeData;

        if (nodeData.model == NodeModel.Root)
        {
            highlightState = HighlightState.ACTIVATED;
        }
        else
        {
            highlightState = HighlightState.NONE;
        }

        RefreshView();
    }

    public void SetHintHighlight(bool hintHighlight)
    {
        if (hintHighlight && highlightState == HighlightState.NONE)
        {
            highlightState = HighlightState.HINT;
        }
        else if (!hintHighlight && highlightState == HighlightState.HINT)
        {
            highlightState = HighlightState.NONE;
        }
        RefreshView();
    }

    public void SetHighlight (bool highlight)
    {
        if (highlight)
        {
            switch (highlightState)
            {
                case HighlightState.NONE:
                case HighlightState.HINT: 
                    highlightState = HighlightState.HIGHLIGHT;
                    break;
                case HighlightState.ACTIVATED:
                    highlightState = HighlightState.ACTIVATED_HIGHLIGHT;
                    break;
            }
        }
        else
        {
            switch (highlightState)
            {
                case HighlightState.HIGHLIGHT:
                    highlightState = HighlightState.NONE;
                    break;
                case HighlightState.ACTIVATED_HIGHLIGHT:
                    highlightState = HighlightState.ACTIVATED;
                    break;
            }
        }
        RefreshView();
    }

    public void SetActivated(bool activated)
    {
        if (activated)
        {
            switch (highlightState)
            {
                case HighlightState.NONE: 
                case HighlightState.HINT: 
                    highlightState = HighlightState.ACTIVATED; 
                    break;
                case HighlightState.HIGHLIGHT: 
                    highlightState = HighlightState.ACTIVATED_HIGHLIGHT; 
                    break;
            }
        }
        else
        {
            switch (highlightState)
            {
                case HighlightState.ACTIVATED:
                    highlightState = HighlightState.NONE;
                    break;
                case HighlightState.ACTIVATED_HIGHLIGHT:
                    highlightState = HighlightState.HIGHLIGHT;
                    break;
            }
        }
        RefreshView();
    }

    private void RefreshView()
    {
        baseCosmeticObject.SetActive(highlightState == HighlightState.NONE);
        hintHighlightCosmeticObject.SetActive(highlightState == HighlightState.HINT);
        highlightCosmeticObject.SetActive(highlightState == HighlightState.HIGHLIGHT);
        activateCosmeticObject.SetActive(highlightState == HighlightState.ACTIVATED);
        activateHighlightCosmeticObject.SetActive(highlightState == HighlightState.ACTIVATED_HIGHLIGHT);
        
        if (collider != null)
        {
            collider.enabled = (highlightState != HighlightState.NONE);
        }
    }
}
