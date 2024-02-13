using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 20f;
    public float lookSpeed = 2f;
    public float lookXLimit = 90f;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    public bool canMove = true;
    CharacterController characterController;
    public Animator swordAnimator;
    public Animator shieldAnimator;
    public GameObject walkingAudio;
    public GameObject runningAudio;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {

        #region Handles Movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        #endregion

        #region Handles Jumping
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        #endregion

        #region Handles Rotation
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        #endregion

        #region Handles Animation Speed
        float absCurSpeedX = Mathf.Abs(curSpeedX);
        float absCurSpeedY = Mathf.Abs(curSpeedY);
        float fasterDirection = absCurSpeedX > absCurSpeedY ? absCurSpeedX : absCurSpeedY;
        float moveSpeed = Mathf.Clamp(fasterDirection, .1f, 12f) / 6;
        swordAnimator.SetFloat("moveSpeed", moveSpeed);
        shieldAnimator.SetFloat("moveSpeed", moveSpeed);
        #endregion

        #region Handles Footstep
        if (moveSpeed > 1.5f && characterController.isGrounded)
        {
            runningAudio.SetActive(true);
            walkingAudio.SetActive(false);
        }
        else if (moveSpeed > .5f && characterController.isGrounded)
        {
            walkingAudio.SetActive(true);
            runningAudio.SetActive(false);

        }
        else
        {
            walkingAudio.SetActive(false);
            runningAudio.SetActive(false);
        }
        #endregion
    }
}
