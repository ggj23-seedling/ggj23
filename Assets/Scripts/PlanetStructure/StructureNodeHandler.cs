using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PlanetStructureTypes;

public class StructureNodeHandler : MonoBehaviour
{
    private StructureNode _nodeData = null;
    public StructureNode nodeData { get { return _nodeData; } }

    public void InitializeNodeData (StructureNode nodeData)
    {
        if (this._nodeData != null)
        {
            Debug.LogError("Initializing twice the same StructureNodeHandler");
        }
        this._nodeData = nodeData;
    }
}
