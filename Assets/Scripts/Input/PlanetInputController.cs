using PlanetStructureTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetInputController : MonoBehaviour
{
    public ControlGui controlGui;
    public MenuActionButton[] menu;
    
    enum activeNodeBehaviourType
    {
        NONE,
        MENU,
        START_LINK
    }

    StructureNodeHandler tmpClickedNodeHandler = null;

    StructureNodeHandler activeNode = null;
    activeNodeBehaviourType activeNodeBehaviour = activeNodeBehaviourType.NONE;

    PlanetStructureGenerator planetStructureGenerator;

    InputController inputController = null;

    private void Awake()
    {
        planetStructureGenerator = GetComponent<PlanetStructureGenerator>();
        inputController = FindObjectOfType<InputController>();
    }

    private void OnEnable()
    {
        inputController.OnGameObjectClicked += OnNodeClicked;
    }

    private void OnDisable()
    {
        inputController.OnGameObjectClicked -= OnNodeClicked;
    }

    private void OnNodeClicked(GameObject gameObject)
    {
        StructureNodeHandler nodeHandler = gameObject.GetComponent<StructureNodeHandler>();
        if (nodeHandler != null)
        {
            StartCoroutine(OnNodeClickedCoroutine(nodeHandler));
        }
    }

    private IEnumerator OnNodeClickedCoroutine(StructureNodeHandler nodeHandler)
    {
        if (tmpClickedNodeHandler == null)
        {
            tmpClickedNodeHandler = nodeHandler;
        }

        yield return null;

        if (tmpClickedNodeHandler != null && tmpClickedNodeHandler == nodeHandler)
        {
            tmpClickedNodeHandler = null;
            ClickSelection(nodeHandler);
        }
    }

    private void ClickSelection(StructureNodeHandler nodeHandler)
    {
        // TODO: fare roba o aprire menù, o altro
        // qui si setta nodeMenuOpened

        if (!Clock.Instance().CanClickOnNodes)
        {
            Debug.Log("Can't click on nodes: it's not the player's turn");
            return;
        }
        
        if (activeNode != null)
        {
            if (activeNode != nodeHandler)
            {
                if (activeNodeBehaviour == activeNodeBehaviourType.MENU)
                {
                    SetUpMenu();
                }
                else if (activeNodeBehaviour == activeNodeBehaviourType.START_LINK)
                {
                    NodeModel activeNodeModel = activeNode.nodeData.model;
                    NodeModel clickedNodeModel = nodeHandler.nodeData.model;                    
                    if (activeNodeModel.CanExpandTo(clickedNodeModel))
                    {
                        activeNodeModel.ExpandTo(nodeHandler.nodeData.model);
                        planetStructureGenerator.spawnLinkObject(activeNode, nodeHandler);
                        Clock.Instance().NextTurn();
                    } else
                    {
                        Debug.Log("Cannot expand to that node");
                    }
                }
                else
                {
                    Debug.LogError("Unsupported Operation: there is an active node with an invalid state");
                }
            }            

            activeNodeBehaviour = activeNodeBehaviourType.NONE;
            activeNode.Highlighted = false;
            setHintHighlightToNeighbours(activeNode, false);
            activeNode = null;
            SetUpMenu();
        }
        else
        {
            activeNode = nodeHandler;
            activeNodeBehaviour = activeNodeBehaviourType.MENU;
            SetUpMenu();
        }
    }

    private void OnLinkStartRequest ()
    {        
        if (activeNode != null)
        {
            if (activeNode.Connected)
            {
                activeNodeBehaviour = activeNodeBehaviourType.START_LINK;
                activeNode.Highlighted = true;
                setHintHighlightToNeighbours(activeNode, true);
            }
            else
            {
                // A link start has been requested to an invalid node
                activeNode.Highlighted = false;
                setHintHighlightToNeighbours(activeNode, false);
                activeNodeBehaviour = activeNodeBehaviourType.NONE;
                activeNode = null;
            }            
        }
        else
        {
            UnityEngine.Debug.LogWarning("A link has been started without an active node.");
            activeNode.Highlighted = false;
            setHintHighlightToNeighbours(activeNode, false);
            activeNodeBehaviour = activeNodeBehaviourType.NONE;
            activeNode = null;
        }
        SetUpMenu(forceClose: true);
    }

    private void setHintHighlightToNeighbours(StructureNodeHandler centerNode, bool hintHighlight)
    {
        if (hintHighlight == true)
        {
            foreach (StructureNode n in centerNode.nodeData.neighbours)
            {
                StructureNodeHandler h = planetStructureGenerator.getHandler(n);
                if (h != null && centerNode.nodeData.model.CanExpandTo(h.nodeData.model))
                {
                    h.Hinted = true;
                }
            }
        }
        else
        {
            foreach (StructureNode n in centerNode.nodeData.neighbours)
            {
                StructureNodeHandler h = planetStructureGenerator.getHandler(n);
                if (h != null)
                {
                    h.Hinted = false;
                }
            }
        }
    }

    private void SetUpMenu(bool forceClose = false)
    {
        if (forceClose || activeNode == null)
        {
            controlGui?.DisplayMenu(false);
        }
        else
        {
            controlGui?.DisplayMenu(true);
            foreach (MenuActionButton button in menu)
            {
                switch (button.evolution)
                {
                    case NodeEvolution.none:
                        button.SetCost(ModelConfiguration.values.expansionCost);
                        button.SetAvailable(true);
                        button.callback = OnLinkStartRequest;
                        break;
                    case NodeEvolution.attack:
                        button.SetCost(ModelConfiguration.values.evolutionCost);
                        button.SetAvailable(activeNode.nodeData.model.CanEvolveAttack);
                        button.callback = EvolveAttack;
                        break;
                    case NodeEvolution.defense:
                        button.SetCost(ModelConfiguration.values.evolutionCost);
                        button.SetAvailable(activeNode.nodeData.model.CanEvolveDefense);
                        button.callback = EvolveDefense;
                        break;
                    case NodeEvolution.extraction:
                        button.SetCost(ModelConfiguration.values.evolutionCost);
                        button.SetAvailable(activeNode.nodeData.model.CanEvolveExtraction);
                        button.callback = EvolveExtraction;
                        break;
                }
            }
        }
    }

    private void EvolveAttack()
    {
        activeNode.nodeData.model.EvolveAttack();        
        ResetActiveNode();
        Clock.Instance().NextTurn();
    }

    private void EvolveDefense()
    {
        activeNode.nodeData.model.EvolveDefense();
        ResetActiveNode();
        Clock.Instance().NextTurn();
    }

    private void EvolveExtraction()
    {
        activeNode.nodeData.model.EvolveExtraction();
        ResetActiveNode();
        Clock.Instance().NextTurn();
    }

    private void ResetActiveNode()
    {
        activeNodeBehaviour = activeNodeBehaviourType.NONE;
        activeNode = null;
        SetUpMenu();
    }
}