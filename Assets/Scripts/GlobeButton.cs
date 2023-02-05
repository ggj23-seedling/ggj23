using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum ButtonDirection
{
    up,
    down,
    left,
    right
}

public class GlobeButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Orbiting orbitingCamera;
    private bool isPressed = false;
    public ButtonDirection direction = ButtonDirection.up;

    // Start is called before the first frame update
    void Start()
    {
        if (orbitingCamera == null)
        {
            orbitingCamera = FindObjectOfType<Orbiting>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPressed)
        {
            switch (direction)
            {
                case ButtonDirection.up:
                    orbitingCamera.Up();
                    break;
                case ButtonDirection.down:
                    orbitingCamera.Down();
                    break;
                case ButtonDirection.left:
                    orbitingCamera.Left();
                    break;
                case ButtonDirection.right:
                    orbitingCamera.Right();
                    break;
            }
        }
        
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}
