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

    private void Awake()
    {
        planetStructureGenerator = GetComponent<PlanetStructureGenerator>();
    }

    private void OnEnable()
    {
        InputController.instance.OnGameObjectClicked += OnNodeClicked;
    }

    private void OnDisable()
    {
        InputController.instance.OnGameObjectClicked -= OnNodeClicked;
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

        bool canOpenMenu = false; // TODO
        bool canLinkNodes = true; // TODO

        if (activeNode != null)
        {
            if (activeNodeBehaviour == activeNodeBehaviourType.MENU)
            {
                // close menu
            }
            else if (activeNodeBehaviour == activeNodeBehaviourType.START_LINK)
            {
                if (canLinkNodes)
                {
                    activeNode.nodeData.model.ExpandTo(nodeHandler.nodeData.model);
                    planetStructureGenerator.spawnLinkObject(activeNode, nodeHandler);
                }
            }
            else
            {
                Debug.LogError("Unsupported Operation: there is an active nove with an invalid state");
            }

            activeNode = null;
            activeNodeBehaviour = activeNodeBehaviourType.NONE;
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
            }
            else
            {
                Debug.LogError("A link has start has been requested to an invalid node.");
                activeNode = null;
                activeNodeBehaviour = activeNodeBehaviourType.NONE;
            }

            // close menu

        }
        else
        {
            Debug.LogWarning("A link has been started without an active node.");
            activeNode = null;
            activeNodeBehaviour = activeNodeBehaviourType.NONE;
        }
    }

    
}
