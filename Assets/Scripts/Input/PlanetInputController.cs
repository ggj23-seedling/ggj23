using PlanetStructureTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetInputController : MonoBehaviour
{
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

        bool canOpenMenu = true; // TODO

        if (activeNode != null)
        {
            if (activeNode != nodeHandler)
            {
                if (activeNodeBehaviour == activeNodeBehaviourType.MENU)
                {
                    // close menu
                }
                else if (activeNodeBehaviour == activeNodeBehaviourType.START_LINK)
                {
                    NodeModel activeNodeModel = activeNode.nodeData.model;
                    NodeModel clickedNodeModel = nodeHandler.nodeData.model;                    
                    if (activeNodeModel.CanExpandTo(clickedNodeModel))
                    {
                        activeNodeModel.ExpandTo(nodeHandler.nodeData.model);
                        planetStructureGenerator.spawnLinkObject(activeNode, nodeHandler);
                        // TODO: la grafica deve ascoltare il modello perchè altrimenti si rompe con le logiche interne degli attacchi
                        nodeHandler.SetActivated(true);
                    } else
                    {
                        Debug.Log("Cannot expand to that node");
                    }
                }
                else
                {
                    Debug.LogError("Unsupported Operation: there is an active nove with an invalid state");
                }
            }            

            activeNodeBehaviour = activeNodeBehaviourType.NONE;
            activeNode.SetHighlight(false);
            setHintHighlightToNeighbours(activeNode, false);
            activeNode = null;
        }
        else
        {
            activeNode = nodeHandler;
            if (canOpenMenu)
            {
                activeNodeBehaviour = activeNodeBehaviourType.MENU;
                // TODO openmenu
                OnLinkStartRequest(); // TODO remove
            }
        }
    }

    private void OnLinkStartRequest () // TODO attach: attach when the link button is pressed
    {
        bool canStartLink = true; // TODO

        if (activeNode != null)
        {
            if (canStartLink)
            {
                activeNodeBehaviour = activeNodeBehaviourType.START_LINK;
                activeNode.SetHighlight(true);
                setHintHighlightToNeighbours(activeNode, true);
            }
            else
            {
                UnityEngine.Debug.LogError("A link has start has been requested to an invalid node.");
                activeNode.SetHighlight(false);
                setHintHighlightToNeighbours(activeNode, false);
                activeNodeBehaviour = activeNodeBehaviourType.NONE;
                activeNode = null;
            }

            // close menu

        }
        else
        {
            UnityEngine.Debug.LogWarning("A link has been started without an active node.");
            activeNode.SetHighlight(false);
            setHintHighlightToNeighbours(activeNode, false);
            activeNodeBehaviour = activeNodeBehaviourType.NONE;
            activeNode = null;
        }
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
                    h.SetHintHighlight(true);
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
                    h.SetHintHighlight(false);
                }
            }
        }
    }
}
