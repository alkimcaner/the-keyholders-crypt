using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform target;
    NavMeshAgent agent;
    bool isChasing = false;
    public int chasingDistance = 50;
    public int health = 100;
    public bool isRanged = false;
    public GameObject bullet;
    public Animator skeletonAnimator;
    public GameObject healthBar;

    void Start()
    {
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

        if (Vector3.Distance(transform.position, target.position) < chasingDistance)
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

        float interpolatedX = Mathf.Lerp(healthBar.transform.localScale.x, (float)health / 100, Time.deltaTime * 10);
        healthBar.transform.localScale = new Vector3(interpolatedX, 1, 1);

        if (health <= 0)
        {
            Destroy(gameObject);

            Player player = target.gameObject.GetComponent<Player>();
            player.gold += 5;
            PlayerPrefs.SetInt("gold", player.gold);
        }

    }

    IEnumerator RangedAttack()
    {
        while (true)
        {
            if (isChasing)
            {
                GameObject currentBullet = Instantiate(bullet);
                Rigidbody bulletRigidbody = currentBullet.GetComponent<Rigidbody>();
                currentBullet.transform.LookAt(target);
                bulletRigidbody.velocity = transform.forward * Time.deltaTime * 10;
                yield return new WaitForSeconds(2);
                Destroy(currentBullet);
            }
        }
    }
}
