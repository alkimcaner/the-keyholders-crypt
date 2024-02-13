using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator swordAnimator;
    public Animator shieldAnimator;
    public Camera playerCamera;
    bool isAttack = false;
    bool isGuarded = false;
    public int coin = 0;
    public int damage = 10;
    int distance = 3;
    LayerMask enemyLayer;
    [SerializeField] private TMP_Text coinText;

    void Start()
    {
        enemyLayer = LayerMask.GetMask("Enemy");
    }

    void Update()
    {
        coinText.text = coin.ToString();

        if (Input.GetButtonDown("Fire1") && !isGuarded && !isAttack)
        {
            StartCoroutine(AttackWait());

            swordAnimator.Play("sword_attack");

            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hit, distance, enemyLayer))
            {
                Enemy enemy = hit.transform.gameObject.GetComponent<Enemy>();
                enemy.health -= damage;
                Debug.Log(enemy.health);
            }
        }

        if (Input.GetButton("Fire2") && !isAttack)
        {
            isGuarded = true;
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            isGuarded = false;
        }

        shieldAnimator.SetBool("isGuarded", isGuarded);
    }

    IEnumerator AttackWait()
    {
        isAttack = true;
        yield return new WaitForSeconds(.3f);
        isAttack = false;
    }
}
