using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public float distance = 2f;
    bool raycastHit;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        raycastHit = Physics.Raycast(transform.position, forward, distance);
        Debug.Log(raycastHit);
    }
}
