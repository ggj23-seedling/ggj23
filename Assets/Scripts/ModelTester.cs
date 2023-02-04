using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        FindObjectOfType<ModelConfiguration>().BeReady();
        Debug.Log($"Tapped {gameObject.name}");
        Clock.Instance().NextTurn();
    }
}
