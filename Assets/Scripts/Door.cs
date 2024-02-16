using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    public float closedAngle = 0;
    public float openAngle = 90;
    public int speed = 5;
    private OffMeshLink offMeshLink;

    void Start()
    {
        offMeshLink = GetComponent<OffMeshLink>();
    }

    void Update()
    {
        offMeshLink.activated = isOpen;

        Vector3 currentRot = transform.localEulerAngles;

        if (isOpen)
        {
            transform.localEulerAngles = Vector3.Lerp(currentRot, new Vector3(currentRot.x, openAngle, currentRot.z), Time.deltaTime * speed);
        }
        else
        {
            transform.localEulerAngles = Vector3.Lerp(currentRot, new Vector3(currentRot.x, closedAngle, currentRot.z), Time.deltaTime * speed);
        }
    }
}
