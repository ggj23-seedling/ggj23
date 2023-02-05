using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public delegate void OnGameObjectClickedDelegate(GameObject gameObject);

public class InputController : MonoBehaviour
{

    public event OnGameObjectClickedDelegate OnGameObjectClicked;

    public void NotifyGameObjectClicked(GameObject gameObject)
    {
        OnGameObjectClicked(gameObject);
    }
}
