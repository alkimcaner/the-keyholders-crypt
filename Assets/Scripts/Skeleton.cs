using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Skeleton : MonoBehaviour
{
    [SerializeField] private Transform target;
    NavMeshAgent agent;
    bool isChasing = false;
    public int chasingDistance = 50;
    float initialHealth;
    public float health = 100;
    public bool isRanged = false;
    public Rigidbody projectile;
    public Animator skeletonAnimator;
    public GameObject healthBarObject;
    public GameObject smokeObject;

    void Start()
    {
        if (!target)
        {
            target = GameObject.Find("Player").transform;
        }

        initialHealth = health;

        agent = GetComponent<NavMeshAgent>();

        if (isRanged)
        {
            StartCoroutine(RangedAttack());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isOnOffMeshLink)
        {
            agent.speed = 1.75f;
        }
        else
        {
            agent.speed = 3.5f;
        }

        agent.SetDestination(target.position);

        if (Vector3.Distance(transform.position, target.position) < chasingDistance && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.hasPath)
        {
            agent.isStopped = false;
            isChasing = true;
            skeletonAnimator.SetBool("isChasing", true);
        }
        else
        {
            agent.isStopped = true;
            isChasing = false;
            skeletonAnimator.SetBool("isChasing", false);
        }

        float interpolatedX = Mathf.Lerp(healthBarObject.transform.localScale.x, health / initialHealth, Time.deltaTime * 10);
        healthBarObject.transform.localScale = new Vector3(interpolatedX, 1, 1);

        if (health <= 0)
        {
            Destroy(gameObject);

            Player player = target.gameObject.GetComponent<Player>();
            player.gold += 5;
            PlayerPrefs.SetInt("gold", player.gold);

            Instantiate(smokeObject, transform.position + transform.TransformDirection(new Vector3(0, 1, 0)), transform.rotation);
        }

    }

    IEnumerator RangedAttack()
    {
        while (true)
        {
            if (isChasing)
            {
                Rigidbody clone = Instantiate(projectile, transform.position + transform.TransformDirection(new Vector3(0, 1, 2)), transform.rotation);
                clone.velocity = transform.forward * 50;
            }
            yield return new WaitForSeconds(2);
        }
    }
}
