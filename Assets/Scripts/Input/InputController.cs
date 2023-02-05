using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public delegate void OnGameObjectClickedDelegate(GameObject gameObject);
public delegate void OnGameObjectPairClickedDelegate(GameObject firstGameObject, GameObject secondGameObject);

public class InputController
{
    private static InputController _instance;
    public static InputController instance
    {
        get 
        { 
            if (_instance == null)
            {
                _instance = new InputController();
            }
            return _instance;
        }
    }

    public event OnGameObjectClickedDelegate OnGameObjectClicked;

    public void NotifyGameObjectClicked(GameObject gameObject) { instance.OnGameObjectClicked(gameObject); }
}
