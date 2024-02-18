using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    [SerializeField] private Transform target;
    public bool isAttacking = false;
    float initialHealth;
    public float health = 300;
    public Rigidbody projectile;
    public GameObject healthBarObject;

    void Start()
    {
        target = GameObject.Find("Player").transform;

        initialHealth = health;

        StartCoroutine(RangedAttack());
    }

    // Update is called once per frame
    void Update()
    {
        float interpolatedX = Mathf.Lerp(healthBarObject.transform.localScale.x, health / initialHealth, Time.deltaTime * 10);
        healthBarObject.transform.localScale = new Vector3(interpolatedX, 1, 1);

        if (health <= 0)
        {
            isAttacking = false;
            StartCoroutine(Dialogue.ShowDialogue(4));
            StartCoroutine(Ending());
        }

        transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));

    }

    IEnumerator RangedAttack()
    {
        while (true)
        {
            if (isAttacking)
            {
                Rigidbody clone = Instantiate(projectile, transform.position + transform.TransformDirection(new Vector3(0, 1, 2)), transform.rotation);
                clone.velocity = transform.forward * 50;
            }
            yield return new WaitForSeconds(1.5f);
        }
    }

    IEnumerator Ending()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("Ending");
    }
}