using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PlanetStructureTypes;
using System;

public class StructureNodeHandler : MonoBehaviour
{
    private bool hinted = false;
    public bool Hinted
    {
        get => hinted;
        set
        {
            hinted = value;
            RefreshView();
        }
    }
    
    private bool highlighted = false;
    public bool Highlighted
    {
        get => highlighted;
        set
        {
            highlighted = value;
            RefreshView();
        }
    }

    public bool Connected => _nodeData?.model?.IsConnected ?? false;

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

    private Collider nodeCollider;

    private void Awake()
    {
        RefreshView();

        nodeCollider = GetComponent<Collider>();
        if (nodeCollider == null) nodeCollider = GetComponentInChildren<Collider>();
    }

    public void InitializeNodeData (StructureNode nodeData)
    {
        if (this._nodeData != null)
        {
            Debug.LogError("Initializing twice the same StructureNodeHandler");
        }
        this._nodeData = nodeData;

        nodeData.ListenToModelChanges(OnModelChanged);

        if (nodeData.model == NodeModel.Root)
        {
            highlighted = true;
        }
        RefreshView();
    }

    private void OnModelChanged(NodeModel model)
    {
        RefreshView();
    }
        
    private void RefreshView()
    {
        baseCosmeticObject.SetActive(false);
        hintHighlightCosmeticObject.SetActive(false);
        highlightCosmeticObject.SetActive(false);
        activateCosmeticObject.SetActive(false);
        activateHighlightCosmeticObject.SetActive(false);
        
        GameObject activeShape = baseCosmeticObject;
        if (Connected)
        {
            activeShape = activateCosmeticObject;
            if (Highlighted)
            {
                activeShape = activateHighlightCosmeticObject;
            }
            if (Hinted)
            {
                activeShape = hintHighlightCosmeticObject;
            }
        }
        else if (Hinted)
        {
            activeShape = highlightCosmeticObject;
        }
        activeShape.SetActive(true);


        if (nodeCollider != null)
        {
            nodeCollider.enabled = Connected || Hinted || Highlighted;
        }
    }
}
