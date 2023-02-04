using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbiting : MonoBehaviour
{
    public Transform target;
    public float speed = 100f;
    public float height = 20f;

    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(target);
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 radiusVersor = (transform.position - target.position).normalized;
        transform.position = target.position + radiusVersor * height;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log($"radiusVersor = {radiusVersor} ({target.position} -> {transform.position})");
            Debug.Log($"transform.up = {transform.up}");
        }

        if (Input.GetKey(KeyCode.W))
        {            
            transform.RotateAround(target.position, transform.right, speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.RotateAround(target.position, transform.up, speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.RotateAround(target.position, -transform.right, speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.RotateAround(target.position, -transform.up, speed * Time.deltaTime);
        }

    }
}
