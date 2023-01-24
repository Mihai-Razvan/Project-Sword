using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PhotonView view;
    [SerializeField] PlayerData playerData;

    [SerializeField] CharacterController controller;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckHeight;
    [SerializeField] float groundCheckWidth;
    [SerializeField] LayerMask groundMask;
    string movementState;                          //IDLE, RUNNING, JUMPING, MID-AIR
    [SerializeField] float runningSpeed;
    [SerializeField] float midAirMovementSpeed;
    float velocity;
    [SerializeField] float gravity;
    [SerializeField] float minimumVelocity;
    [SerializeField] float jumpForce;
    [SerializeField] Animator animator;
    [SerializeField] float fallEndHeight;          //the height over the ground at which mid air turns into falling


    void Start()
    {
        movementState = "IDLE";
        resetStates();
        animator.SetBool("Idle", true);
    }

    void Update()
    {
        handleAnimations();

        if (!view.IsMine)
            return;

        movement();
        jump();
    }

    void movement()
    {
        switch (movementState)
        {
            case "IDLE":
                idleState();
                break;
            case "RUNNING":
                runningState();
                break;
            case "JUMPING":
                jumpingState();
                break;
            case "MID-AIR":
                midAirState();
                break;
        }
    }

    void idleState()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 && isGrounded() == true)
            changeMovementState("RUNNING");
        else if (isGrounded() == false)
            changeMovementState("MID-AIR");
    }

    void runningState()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontalMovement + transform.forward * verticalMovement;
        controller.Move(move * runningSpeed * Time.deltaTime);

        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0 && isGrounded() == true)
            changeMovementState("IDLE");
        else if (isGrounded() == false)
            changeMovementState("MID-AIR");
    }

    void jumpingState()
    {
        inAirMovement();

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)         //jump start has ended
            changeMovementState("MID-AIR");
    }

    void midAirState()
    {
        inAirMovement();

        RaycastHit hit;
        if (Physics.Raycast(this.gameObject.transform.position, -transform.up, out hit, Mathf.Infinity, groundMask))
            if (this.gameObject.transform.position.y - hit.point.y < fallEndHeight)
            {
                if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
                    changeMovementState("IDLE");
                else
                    changeMovementState("RUNNING");
            }
    }

    void jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded() == true)
        {
            velocity = jumpForce;
            changeMovementState("JUMPING");
        }
    }

    void inAirMovement()        //this happens when player is not grounded; it is jumoing, mid-air or falling
    {
        velocity += gravity * Time.deltaTime;
        if (velocity < minimumVelocity)
            velocity = minimumVelocity;

        Vector3 fall = transform.up * velocity;
        controller.Move(fall * Time.deltaTime);

        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontalMovement + transform.forward * verticalMovement;
        controller.Move(move * midAirMovementSpeed * Time.deltaTime);
    }


    bool isGrounded()
    {
        return Physics.CheckBox(groundCheck.position, new Vector3(groundCheckWidth, groundCheckHeight, groundCheckWidth), Quaternion.identity, groundMask);
       // return Physics.CheckSphere(groundCheck.position, groundCheckSize, groundMask);
    }

    void handleAnimations()
    {
        string state = "IDLE";

        if (view.IsMine)
            state = movementState;
        else if (playerData.getPlayerMovementState() != null)
            state = playerData.getPlayerMovementState();

        resetStates();

        switch (state)
        {
            case "IDLE":
                animator.SetBool("Idle", true);
                break;
            case "RUNNING":
                animator.SetBool("Running", true);
                break;
            case "JUMPING":
                animator.SetBool("Jumping", true);
                break;
            case "MID-AIR":
                animator.SetBool("Mid-Air", true);
                break;
        }
    }

    void changeMovementState(string newState)
    {
        movementState = newState;
        playerData.setPlayerMovementState(newState);
    }

    void resetStates()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Running", false);
        animator.SetBool("Jumping", false);
        animator.SetBool("Mid-Air", false);
    }
}
