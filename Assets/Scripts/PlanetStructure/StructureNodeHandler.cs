using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PlanetStructureTypes;
using System;
using System.Runtime.CompilerServices;
using Random = UnityEngine.Random;

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
    public GameObject cosmeticObjectContainer;
    [SerializeField]
    public GameObject hintObject;
    [SerializeField]
    public GameObject highlightObject;

    private NodesCosmeticResources cosmeticResourcesHandler;
    private VertexColorInterpreter vertexColorInterpreter;

    private GameObject cosmeticObjectPrefab = null;
    
    private Collider nodeCollider;

    private void Awake()
    { 
        cosmeticResourcesHandler = FindObjectOfType<NodesCosmeticResources>();
        vertexColorInterpreter = FindObjectOfType<VertexColorInterpreter>();

        nodeCollider = GetComponent<Collider>();
        if (nodeCollider == null) nodeCollider = GetComponentInChildren<Collider>();
    }

    private void OnEnable()
    {
        RefreshView();
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
        hintObject.SetActive(Hinted);
        highlightObject.SetActive(Highlighted);
        if (nodeCollider != null)
        {
            nodeCollider.enabled = Connected || Hinted || Highlighted;
        }

        GameObject cosmeticObjectPrefabBackup = cosmeticObjectPrefab;

        cosmeticObjectPrefab = null;
        if (cosmeticResourcesHandler != null && vertexColorInterpreter != null
            && nodeData != null && nodeData.model != null)
        {
            if (nodeData.model.IsConnected)
            {
                // It's a node of mine
                if (nodeData.model.IsRoot)
                {
                    cosmeticObjectPrefab = cosmeticResourcesHandler.rootPrefab;
                }
                else
                {
                    // nodeData.model.Evolution
                    // nodeData.model.Superevolved
                    if (nodeData.model.Evolution == NodeEvolution.attack)
                    {
                        cosmeticObjectPrefab = nodeData.model.Superevolved ? cosmeticResourcesHandler.superAttackEvolutionPrefab : cosmeticResourcesHandler.attackEvolutionPrefab;
                    }
                    else if (nodeData.model.Evolution == NodeEvolution.defense)
                    {
                        cosmeticObjectPrefab = nodeData.model.Superevolved ? cosmeticResourcesHandler.superDefenseEvolutionPrefab : cosmeticResourcesHandler.defenseEvolutionPrefab;
                    }
                    else if (nodeData.model.Evolution == NodeEvolution.extraction)
                    {
                        cosmeticObjectPrefab = nodeData.model.Superevolved ? cosmeticResourcesHandler.superExtractionEvolutionPrefab : cosmeticResourcesHandler.extractionEvolutionPrefab;
                    }
                    else
                    {
                        cosmeticObjectPrefab = nodeData.model.Superevolved ? cosmeticResourcesHandler.superExpansionEvolutionPrefab : cosmeticResourcesHandler.expansionEvolutionPrefab;
                    }
                }
            }
            else
            {
                // It's not a node of mine
                if (nodeData.model.warZone)
                {
                    cosmeticObjectPrefab = cosmeticResourcesHandler.warZoneNodePrefab;
                }
                else if (nodeData.model.Population < vertexColorInterpreter.VillagePopulation * 0.95)
                {
                    cosmeticObjectPrefab = null; // no city
                }
                else if (nodeData.model.Population < vertexColorInterpreter.TownPopulation * 0.95)
                {
                    cosmeticObjectPrefab = cosmeticResourcesHandler.villageNodePrefabs.Count > 0 ? cosmeticResourcesHandler.villageNodePrefabs[Random.Range(0, cosmeticResourcesHandler.villageNodePrefabs.Count - 1)] : null;
                }
                else if (nodeData.model.Population < vertexColorInterpreter.CityPopulation * 0.95)
                {
                    cosmeticObjectPrefab = cosmeticResourcesHandler.townNodePrefabs.Count > 0 ? cosmeticResourcesHandler.townNodePrefabs[Random.Range(0, cosmeticResourcesHandler.townNodePrefabs.Count - 1)] : null;
                }
                else
                {
                    cosmeticObjectPrefab = cosmeticResourcesHandler.cityNodePrefabs.Count > 0 ? cosmeticResourcesHandler.cityNodePrefabs[Random.Range(0, cosmeticResourcesHandler.cityNodePrefabs.Count - 1)] : null;
                }
            }
        }

        if (cosmeticObjectPrefab != null)
        {
            if (cosmeticObjectPrefab != cosmeticObjectPrefabBackup)
            {
                GameObject newGameObject = Instantiate(cosmeticObjectPrefab, cosmeticObjectContainer.transform);
                
                Vector3 tmpScale = Vector3.one;
                Transform parentTransform = newGameObject.transform;
                do
                {
                    newGameObject.transform.localScale = Vector3.Scale(tmpScale, parentTransform.localScale);
                    parentTransform = parentTransform.parent;
                } while (parentTransform != null);

                newGameObject.transform.localScale = new Vector3 (1.0f / tmpScale.x, 1.0f / tmpScale.y, 1.0f / tmpScale.z);

                Vector3 upDirection = (this.transform.position - this.transform.parent.position).normalized;
                Vector3 lookDirection = Vector3.Cross(upDirection, Vector3.forward);
                Quaternion globalRotation = Quaternion.LookRotation(lookDirection, upDirection);

                newGameObject.transform.SetPositionAndRotation(this.transform.position, globalRotation);
            }
            cosmeticObjectContainer.SetActive(true);
        }
        else
        {
            cosmeticObjectContainer.SetActive(false);
        }
    }
}
