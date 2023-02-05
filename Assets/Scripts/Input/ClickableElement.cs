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
        inputController.NotifyGameObjectClicked(gameObject);
    }
}
