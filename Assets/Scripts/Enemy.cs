using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform target;
    NavMeshAgent navMesh;
    public int chasingDistance = 10;
    public int health = 100;
    public Animator skeletonAnimator;
    public GameObject healthBar;

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

        float interpolatedX = Mathf.Lerp(healthBar.transform.localScale.x, (float)health / 100, .1f);
        healthBar.transform.localScale = new Vector3(interpolatedX, 1, 1);

        if (health <= 0)
        {
            Destroy(gameObject);

            Player player = target.gameObject.GetComponent<Player>();
            player.coin += 3;
        }

    }
}
