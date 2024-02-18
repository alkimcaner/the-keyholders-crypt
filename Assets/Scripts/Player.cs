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
    public Boss boss;
    public GameObject trailObject;
    public Animator swordAnimator;
    public Animator shieldAnimator;
    public Camera playerCamera;
    public AudioSource hitSound;
    public AudioSource swingSound;
    public AudioSource blockSound;
    public AudioSource hurtSound;
    public AudioSource doorSound;
    public AudioSource metalSound;
    public AudioSource sleepSound;
    public AudioSource music;
    public float health = 100;
    public int gold = 0;
    public int damage = 10;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private GameObject interactionParent;
    [SerializeField] private TMP_Text interactionText;
    [SerializeField] private Slider healthBar;
    private CharacterController characterController;
    public bool hasRedKey = false;
    public bool hasGreenKey = false;
    public bool hasBlueKey = false;
    private bool isAttacking = false;
    private bool isBlocking = false;
    private Vector3 knockback = Vector3.zero;
    private bool isVulnerable = true;
    private int distance = 3;
    private LayerMask enemyLayer;
    private LayerMask interactableLayer;
    public GameObject dialogueObject;
    public TMP_Text dialogueText;
    private string[] lines = {
        "I wonder what's behind that door...",
        "I found these keys at the barn. I can sell them to you if you want.",
        "HAHA! I fooled you! You've paid a lot gold for those keys. Now it's time to pay with your life.",
        "I need more gold."
        };

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        enemyLayer = LayerMask.GetMask("Enemy");
        interactableLayer = LayerMask.GetMask("Interactable");

        hasRedKey = Convert.ToBoolean(PlayerPrefs.GetInt("hasRedKey", 0));
        hasBlueKey = Convert.ToBoolean(PlayerPrefs.GetInt("hasBlueKey", 0));
        hasGreenKey = Convert.ToBoolean(PlayerPrefs.GetInt("hasGreenKey", 0));
        health = PlayerPrefs.GetFloat("health", 100);
        gold = PlayerPrefs.GetInt("gold", 0);
        damage = PlayerPrefs.GetInt("damage", 20);
    }

    void Update()
    {
        // Update UI
        coinText.text = gold.ToString();
        healthBar.value = Mathf.Lerp(healthBar.value, health / 100, Time.deltaTime * 10);

        #region Handles Block
        if (Input.GetButton("Fire2") && !isAttacking)
        {
            isBlocking = true;
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            isBlocking = false;
        }

        shieldAnimator.SetBool("isBlocking", isBlocking);
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
        if (Input.GetButtonDown("Fire1") && !isBlocking && !isAttacking)
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
                    interactionText.text = "Travel to the Town";

                    if (Input.GetKeyDown("e"))
                    {
                        SceneManager.LoadScene("Town");
                    }
                    break;
                case "Horse":
                    interactionText.text = "Travel to the Keyholder's Crypt";

                    if (Input.GetKeyDown("e"))
                    {
                        SceneManager.LoadScene("Crypt");
                    }
                    break;
                case "Blacksmith":
                    interactionText.text = "Upgrade the Sword (25 gold)";

                    if (Input.GetKeyDown("e") && gold >= 25)
                    {
                        gold -= 25;
                        PlayerPrefs.SetInt("gold", gold);

                        damage += 10;
                        PlayerPrefs.SetInt("damage", damage);

                        metalSound.Play();
                    }
                    else if (Input.GetKeyDown("e"))
                    {
                        StartCoroutine(StartDialogue(3));
                    }
                    break;
                case "Innkeeper":
                    interactionText.text = "Sleep at the Inn (25 Gold)";

                    if (Input.GetKeyDown("e") && gold >= 25)
                    {
                        gold -= 25;
                        PlayerPrefs.SetInt("gold", gold);

                        health = 100;
                        PlayerPrefs.SetFloat("health", health);

                        sleepSound.Play();
                    }
                    else if (Input.GetKeyDown("e"))
                    {
                        StartCoroutine(StartDialogue(3));
                    }
                    break;
                case "KeySeller":
                    interactionText.text = "Interact";

                    if (Input.GetKeyDown("e"))
                    {
                        StartCoroutine(StartDialogue(1));
                    }
                    break;
                case "Boss":
                    interactionText.text = "Interact";

                    if (Input.GetKeyDown("e"))
                    {
                        StartCoroutine(StartDialogue(2));
                        StartCoroutine(StartBossSequence());
                    }
                    break;
                case "Door":
                    interactionText.text = "Interact";

                    if (Input.GetKeyDown("e"))
                    {
                        Door door = interactionHit.transform.GetComponent<Door>();
                        door.isOpen = !door.isOpen;
                        doorSound.Play();
                    }
                    break;
                case "BossDoor":
                    interactionText.text = "Interact";

                    if (Input.GetKeyDown("e") && hasRedKey && hasGreenKey && hasBlueKey)
                    {
                        Door door = interactionHit.transform.GetComponent<Door>();
                        door.isOpen = !door.isOpen;
                        doorSound.Play();
                    }
                    else if (Input.GetKeyDown("e"))
                    {
                        StartCoroutine(StartDialogue(0));
                    }
                    break;
                case "RedKey":
                    interactionText.text = "Buy Red Key (100 Gold)";

                    if (Input.GetKeyDown("e") && gold >= 100)
                    {
                        hasRedKey = true;
                        PlayerPrefs.SetInt("hasRedKey", 1);

                        gold -= 100;
                        PlayerPrefs.SetInt("gold", gold);

                        Destroy(interactionHit.transform.gameObject);
                    }
                    else if (Input.GetKeyDown("e"))
                    {
                        StartCoroutine(StartDialogue(3));
                    }
                    break;
                case "GreenKey":
                    interactionText.text = "Buy Green Key (100 Gold)";

                    if (Input.GetKeyDown("e") && gold >= 100)
                    {
                        hasGreenKey = true;
                        PlayerPrefs.SetInt("hasGreenKey", 1);

                        gold -= 100;
                        PlayerPrefs.SetInt("gold", gold);

                        Destroy(interactionHit.transform.gameObject);
                    }
                    else if (Input.GetKeyDown("e"))
                    {
                        StartCoroutine(StartDialogue(3));
                    }
                    break;
                case "BlueKey":
                    interactionText.text = "Buy Blue Key (100 Gold)";

                    if (Input.GetKeyDown("e") && gold >= 100)
                    {
                        hasBlueKey = true;
                        PlayerPrefs.SetInt("hasBlueKey", 1);

                        gold -= 100;
                        PlayerPrefs.SetInt("gold", gold);

                        Destroy(interactionHit.transform.gameObject);
                    }
                    else if (Input.GetKeyDown("e"))
                    {
                        StartCoroutine(StartDialogue(3));
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

            if (isBlocking)
            {
                blockSound.Play();
            }
            else
            {
                health -= 10;
                PlayerPrefs.SetFloat("health", health);

                hurtSound.Play();

                if (health <= 0)
                {
                    health = 100;
                    PlayerPrefs.SetFloat("health", health);

                    gold = 0;
                    PlayerPrefs.SetInt("gold", gold);

                    SceneManager.LoadScene("MainMenu");
                }
            }

            knockback = other.transform.forward * 50;
        }
    }

    IEnumerator AttackCooldown()
    {
        isAttacking = true;
        trailObject.SetActive(true);
        yield return new WaitForSeconds(.3f);
        isAttacking = false;
        trailObject.SetActive(false);
    }

    IEnumerator VulnerableCooldown()
    {
        isVulnerable = false;
        yield return new WaitForSeconds(1);
        isVulnerable = true;
    }

    IEnumerator StartDialogue(int lineIndex)
    {
        if (!dialogueObject.activeInHierarchy)
        {
            dialogueText.text = lines[lineIndex];
            dialogueObject.SetActive(true);
            yield return new WaitForSeconds(3);
            dialogueObject.SetActive(false);
        }
    }

    IEnumerator StartBossSequence()
    {
        yield return new WaitForSeconds(3);
        boss.isAttacking = true;
        music.pitch = 1.25f;
    }

}
