using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling : MonoBehaviour
{
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Orbiting>().enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
