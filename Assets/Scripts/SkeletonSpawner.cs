using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkeletonSpawner : MonoBehaviour
{
    public bool isRanged = false;
    public GameObject skeletonObject;
    private GameObject currentClone;

    void Start()
    {
        Skeleton skeleton = skeletonObject.GetComponent<Skeleton>();
        skeleton.isRanged = isRanged;

        currentClone = Instantiate(skeletonObject, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentClone.IsDestroyed())
        {
            currentClone = Instantiate(skeletonObject, transform.position, transform.rotation);

        }
    }

}
