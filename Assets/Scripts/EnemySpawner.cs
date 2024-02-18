using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public bool isRanged = false;
    public GameObject enemyObject;
    private GameObject currentClone;

    void Start()
    {
        Enemy enemy = enemyObject.GetComponent<Enemy>();
        enemy.isRanged = isRanged;

        currentClone = Instantiate(enemyObject, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentClone.IsDestroyed())
        {
            currentClone = Instantiate(enemyObject, transform.position, transform.rotation);

        }
    }

}
