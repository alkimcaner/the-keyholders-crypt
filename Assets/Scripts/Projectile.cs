using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime * 500, Space.World);
    }
    void OnCollisionEnter()
    {
        Destroy(gameObject);
    }
}
