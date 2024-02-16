using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Animator swordAnimator;
    public Animator shieldAnimator;
    public Camera playerCamera;
    public AudioSource hitSound;
    public AudioSource swingSound;
    public AudioSource blockSound;
    public AudioSource hurtSound;
    public AudioSource doorSound;
    public int health = 100;
    public int gold = 0;
    public int damage = 10;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private GameObject interactionParent;
    [SerializeField] private TMP_Text interactionText;
    [SerializeField] private Slider healthBar;
    private CharacterController characterController;
    private bool hasRedKey = false;
    private bool hasGreenKey = false;
    private bool hasBlueKey = false;
    private bool isAttack = false;
    private bool isGuarded = false;
    private Vector3 knockback = Vector3.zero;
    private bool isVulnerable = true;
    private int distance = 3;
    private LayerMask enemyLayer;
    private LayerMask interactableLayer;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        enemyLayer = LayerMask.GetMask("Enemy");
        interactableLayer = LayerMask.GetMask("Interactable");

        hasRedKey = Convert.ToBoolean(PlayerPrefs.GetInt("hasRedKey", 0));
        hasBlueKey = Convert.ToBoolean(PlayerPrefs.GetInt("hasBlueKey", 0));
        hasGreenKey = Convert.ToBoolean(PlayerPrefs.GetInt("hasGreenKey", 0));
        health = PlayerPrefs.GetInt("health", 100);
        gold = PlayerPrefs.GetInt("gold", 0);
        damage = PlayerPrefs.GetInt("damage", 20);
    }

    void Update()
    {
        // Update UI
        coinText.text = gold.ToString();
        healthBar.value = Mathf.Lerp(healthBar.value, (float)health / 100, Time.deltaTime * 10);

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

    void LateUpdate()
    {
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

        #region Handles Interaction
        RaycastHit interactionHit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out interactionHit, distance, interactableLayer))
        {
            interactionParent.SetActive(true);

            switch (interactionHit.transform.tag)
            {
                case "Exit":
                    interactionText.text = "To the Town";

                    if (Input.GetKeyDown("e"))
                    {
                        SceneManager.LoadScene("Town");
                    }
                    break;
                case "Horse":
                    interactionText.text = "To the Keyholder's Crypt";

                    if (Input.GetKeyDown("e"))
                    {
                        SceneManager.LoadScene("Crypt");
                    }
                    break;
                case "Blacksmith":
                    interactionText.text = "Upgrade the Weapon (25 gold)";

                    if (Input.GetKeyDown("e") && gold >= 25)
                    {
                        gold -= 25;
                        PlayerPrefs.SetInt("gold", gold);

                        damage += 10;
                        PlayerPrefs.SetInt("damage", damage);

                        Debug.Log("upgrade weapon");
                    }
                    break;
                case "Innkeeper":
                    interactionText.text = "Sleep at the Inn (25 Gold)";

                    if (Input.GetKeyDown("e") && gold >= 25)
                    {
                        gold -= 25;
                        PlayerPrefs.SetInt("gold", gold);

                        health = 100;
                        PlayerPrefs.SetInt("health", health);

                        Debug.Log("sleep");
                    }
                    break;
                case "Door":
                    if (Input.GetKeyDown("e"))
                    {
                        Door door = interactionHit.transform.GetComponent<Door>();
                        door.isOpen = !door.isOpen;
                        doorSound.Play();
                    }
                    break;
                case "RedKey":
                    interactionText.text = "Buy Red Key (150 Gold)";

                    if (Input.GetKeyDown("e") && gold >= 150)
                    {
                        hasRedKey = true;
                        PlayerPrefs.SetInt("hasRedKey", 1);

                        gold -= 150;
                        PlayerPrefs.SetInt("gold", gold);

                        Destroy(interactionHit.transform.gameObject);
                    }
                    break;
                case "GreenKey":
                    interactionText.text = "Buy Green Key (150 Gold)";

                    if (Input.GetKeyDown("e") && gold >= 150)
                    {
                        hasGreenKey = true;
                        PlayerPrefs.SetInt("hasGreenKey", 1);

                        gold -= 150;
                        PlayerPrefs.SetInt("gold", gold);

                        Destroy(interactionHit.transform.gameObject);
                    }
                    break;
                case "BlueKey":
                    interactionText.text = "Buy Blue Key (150 Gold)";

                    if (Input.GetKeyDown("e") && gold >= 150)
                    {
                        hasBlueKey = true;
                        PlayerPrefs.SetInt("hasBlueKey", 1);

                        gold -= 150;
                        PlayerPrefs.SetInt("gold", gold);

                        Destroy(interactionHit.transform.gameObject);
                    }
                    break;
                default:
                    interactionText.text = "Interact";
                    break;
            }
        }
        else
        {
            interactionParent.SetActive(false);
        }
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
                PlayerPrefs.SetInt("health", health);

                hurtSound.Play();

                if (health <= 0)
                {
                    health = 100;
                    PlayerPrefs.SetInt("health", health);

                    gold = 0;
                    PlayerPrefs.SetInt("gold", gold);

                    SceneManager.LoadScene("Town");
                }
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
        yield return new WaitForSeconds(1);
        isVulnerable = true;
    }
}
