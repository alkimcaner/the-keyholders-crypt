using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    CharacterController characterController;
    public Animator swordAnimator;
    public Animator shieldAnimator;
    public Camera playerCamera;
    public AudioSource hitSound;
    public AudioSource swingSound;
    public AudioSource blockSound;
    public AudioSource hurtSound;
    bool isAttack = false;
    bool isGuarded = false;
    public int health = 100;
    public int coin = 0;
    public int damage = 10;
    Vector3 knockback = Vector3.zero;
    bool isVulnerable = true;
    int distance = 3;
    LayerMask enemyLayer;
    LayerMask interactionLayer;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private GameObject interactionUI;
    [SerializeField] private TMP_Text interactionText;
    [SerializeField] private Slider healthBar;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        enemyLayer = LayerMask.GetMask("Enemy");
        interactionLayer = LayerMask.GetMask("Interaction");
    }

    void Update()
    {
        // Update UI
        coinText.text = coin.ToString();
        healthBar.value = (float)health / 100;

        #region Handles Interaction
        RaycastHit interactionHit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out interactionHit, distance, interactionLayer))
        {
            interactionUI.SetActive(true);

            switch (interactionHit.transform.tag)
            {
                case "horse":
                    interactionText.text = "Travel to dungeon";
                    break;
                case "merchant":
                    interactionText.text = "Buy";
                    break;
                case "blacksmith":
                    interactionText.text = "Upgrade weapon";
                    break;
                default:
                    interactionText.text = "Interact";
                    break;
            }
        }
        else
        {
            interactionUI.SetActive(false);
        }
        #endregion

        #region Handles Attack
        if (Input.GetButtonDown("Fire1") && !isGuarded && !isAttack)
        {
            StartCoroutine(AttackCooldown());

            swordAnimator.Play("sword_attack");
            swingSound.Play();

            RaycastHit attackHit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out attackHit, distance, enemyLayer))
            {
                Enemy enemy = attackHit.transform.gameObject.GetComponent<Enemy>();
                enemy.health -= damage;
                hitSound.Play();
            }
        }
        #endregion

        #region Handles Block
        if (Input.GetButton("Fire2") && !isAttack)
        {
            isGuarded = true;
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            isGuarded = false;
        }

        shieldAnimator.SetBool("isGuarded", isGuarded);
        #endregion

        #region Handles Knockback
        if (knockback.magnitude > 0.2F)
        {
            characterController.Move(knockback * Time.deltaTime);
        }

        knockback = Vector3.Lerp(knockback, Vector3.zero, 5 * Time.deltaTime);
        #endregion
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 6 && isVulnerable)
        {
            StartCoroutine(VulnerableCooldown());

            if (isGuarded)
            {
                blockSound.Play();
            }
            else
            {
                health -= 10;
                hurtSound.Play();
            }

            knockback = other.transform.forward * 50;
        }
    }

    IEnumerator AttackCooldown()
    {
        isAttack = true;
        yield return new WaitForSeconds(.3f);
        isAttack = false;
    }

    IEnumerator VulnerableCooldown()
    {
        isVulnerable = false;
        yield return new WaitForSeconds(2);
        isVulnerable = true;
    }
}
