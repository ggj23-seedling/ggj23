using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableElement : MonoBehaviour
{
    private bool bPressed = false;

    InputController inputController = null;

    protected virtual void Awake ()
    {
        inputController = FindObjectOfType<InputController>();
        bPressed = false;
    }

    protected virtual void OnDisable()
    {
        bPressed = false;
    }

    private void OnMouseExit()
    {
        bPressed = false;
    }

    private void OnMouseDown()
    {
        if (enabled)
        {
            bPressed = true;
        }
    }

    private void OnMouseUp()
    {
        if (enabled && bPressed)
        {
            bPressed = false;
            OnClicked();
        }
    }

    protected virtual void OnClicked()
    {
        if (isBelowMenu())
        {
            Debug.Log("OnClicked " + gameObject.name + " but it's below the menu");
        }
        else
        {
            inputController.NotifyGameObjectClicked(gameObject);
        }
        
    }

    // Very dirty fix :-)
    public bool isBelowMenu()
    {        
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        return screenPoint.x > ControlGui.leftEdgeOfMenu;
    }
}
