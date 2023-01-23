using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    PhotonView view;
    [SerializeField] PlayerData playerData;

    [SerializeField] CharacterController controller;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckSize;
    [SerializeField] LayerMask groundMask;
    string movementState;                          //IDLE, RUNNING, JUMPING, MID-AIR, FALLING
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
        view = this.gameObject.GetComponent<PhotonView>();

        movementState = "IDLE";
        animator.SetBool("Running", false);
        animator.SetBool("Jumping", false);
        animator.SetBool("Idle", true);
    }

    void Update()
    {
        Debug.Log(movementState);
      //  Debug.Log(isGrounded());
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
            case "FALLING":
                fallingState();
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

        if (animator.GetCurrentAnimatorStateInfo(0).length <= animator.GetCurrentAnimatorStateInfo(0).normalizedTime)         //jump start has ended
            changeMovementState("MID-AIR");
    }

    void midAirState()
    {
        inAirMovement();

        RaycastHit hit;
        if (Physics.Raycast(this.gameObject.transform.position, -transform.up, out hit, Mathf.Infinity, groundMask))
            if (this.gameObject.transform.position.y - hit.point.y < fallEndHeight)
                changeMovementState("FALLING");
    }

    void fallingState()
    {
        inAirMovement();

        if (isGrounded() == true)
            changeMovementState("IDLE");
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
        return Physics.CheckSphere(groundCheck.position, groundCheckSize, groundMask); 
    }

    void handleAnimations()
    {
        string state = "IDLE";

        if (view.IsMine)
            state = movementState;
        else if (playerData.getPlayerMovementState() != null)
            state = playerData.getPlayerMovementState();

        resetStates();

        switch(state)
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
            case "FALLING":
                animator.SetBool("Falling", true);
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
        animator.SetBool("Falling", false);
    }
}
