using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform target;
    NavMeshAgent navMesh;
    public int chasingDistance = 10;
    public int health = 100;
    public Animator skeletonAnimator;
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        navMesh.SetDestination(target.position);

        if (Vector3.Distance(transform.position, target.position) < chasingDistance)
        {
            navMesh.isStopped = false;
            skeletonAnimator.SetBool("isChasing", true);
        }
        else
        {
            navMesh.isStopped = true;
            skeletonAnimator.SetBool("isChasing", false);
        }

    }
}
