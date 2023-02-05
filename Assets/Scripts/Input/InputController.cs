using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public delegate void OnGameObjectClickedDelegate(GameObject gameObject);

public class InputController : MonoBehaviour
{
    //private static InputController _instance;
    //public static InputController instance { get { return _instance; } }

    //private void Awake()
    //{
    //    _instance = new InputController();
    //}
    
    //private void OnDestroy()
    //{
    //    _instance = null;
    //}
    //
    public event OnGameObjectClickedDelegate OnGameObjectClicked;

    public void NotifyGameObjectClicked(GameObject gameObject) { OnGameObjectClicked(gameObject); }
}
