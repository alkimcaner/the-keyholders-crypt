using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWaypoint : MonoBehaviour
{
    public GameObject[] waypoints;
    int currentWaypoint = 1;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].transform.position, 5 * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Waypoint"))
        {
            currentWaypoint++;
            if (currentWaypoint > waypoints.Length - 1)
            {
                currentWaypoint = 0;
            }
        }
    }
}
