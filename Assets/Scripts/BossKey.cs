using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKey : MonoBehaviour
{


    float initialHealth;
    public float health = 100;
    public GameObject healthBarObject;
    public GameObject smokeObject;
    public GameObject shieldObject;
    public Boss boss;
    public bool isVulnerable = false;

    void Start()
    {
        initialHealth = health;
        healthBarObject.SetActive(false);
        shieldObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Time.deltaTime * 100, 0, Space.World);

        float interpolatedX = Mathf.Lerp(healthBarObject.transform.localScale.x, health / initialHealth, Time.deltaTime * 10);
        healthBarObject.transform.localScale = new Vector3(interpolatedX, 1, 1);

        if (health <= 0)
        {
            Destroy(gameObject);
            boss.health -= 105;

            Instantiate(smokeObject, transform.position + transform.TransformDirection(new Vector3(0, 1, 0)), transform.rotation);
        }

        if (isVulnerable)
        {
            healthBarObject.SetActive(true);
            shieldObject.SetActive(false);
        }
        else
        {
            healthBarObject.SetActive(false);
            shieldObject.SetActive(true);
        }
    }
}
