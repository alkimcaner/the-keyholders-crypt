using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator swordAnimator;
    public Animator shieldAnimator;
    bool isAttack = false;
    bool isGuarded = false;
    public float damage = 10f;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !isGuarded)
        {
            isAttack = true;
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            isAttack = false;
        }

        if (Input.GetButtonDown("Fire2") && !isAttack)
        {
            isGuarded = true;
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            isGuarded = false;
        }

        swordAnimator.SetBool("isAttack", isAttack);
        shieldAnimator.SetBool("isGuarded", isGuarded);
    }
}
