using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CameraPhase
{
    falling,
    orbiting
}

public class Orbiting : MonoBehaviour
{
    public Transform target;
    public float fallingSpeed = 1000f;
    public float fallingHeight = 2000f;
    public float impactHeight = 1f;    
    public float orbitingSpeed = 100f;        
    public float orbitingHeight = 20f;

    private float height;
    private CameraPhase phase;
    private Vector3 startingPosition;
    private Quaternion startingRotation;

    // Start is called before the first frame update
    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Please set target of Orbiting Camera");
        }
        transform.LookAt(target);
        startingPosition = transform.position;
        startingRotation = transform.rotation;
        height = fallingHeight;
    }

    // Update is called once per frame
    void Update()
    {
        switch (phase)
        {
            case CameraPhase.falling:
                height = Mathf.MoveTowards(height, impactHeight, fallingSpeed * Time.deltaTime);
                if (height == impactHeight)
                {
                    phase = CameraPhase.orbiting;
                    OnImpact();
                    transform.position = startingPosition;
                    transform.rotation = startingRotation;
                    height = orbitingHeight;
                }
                break;
            case CameraPhase.orbiting:
                height = orbitingHeight;
                break;
        }
        

        Vector3 radiusVersor = (transform.position - target.position).normalized;
        transform.position = target.position + radiusVersor * height;        

        if (phase == CameraPhase.orbiting)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log($"radiusVersor = {radiusVersor} ({target.position} -> {transform.position})");
                Debug.Log($"transform.up = {transform.up}");
            }

            if (Input.GetKey(KeyCode.W))
            {
                Up();
            }
            if (Input.GetKey(KeyCode.A))
            {
                Left();
            }
            if (Input.GetKey(KeyCode.S))
            {
                Down();
            }
            if (Input.GetKey(KeyCode.D))
            {
                Right();
            }
        }

    }

    void OnImpact()
    {
        Debug.LogWarning("OnImpact");
        Clock.Instance().NextTurn();
    }

    public void Up()
    {
        transform.RotateAround(target.position, transform.right, orbitingSpeed * Time.deltaTime);
    }

    public void Down()
    {
        transform.RotateAround(target.position, -transform.right, orbitingSpeed * Time.deltaTime);
    }

    public void Right()
    {
        transform.RotateAround(target.position, -transform.up, orbitingSpeed * Time.deltaTime);
    }

    public void Left()
    {
        transform.RotateAround(target.position, transform.up, orbitingSpeed * Time.deltaTime);
    }
}
