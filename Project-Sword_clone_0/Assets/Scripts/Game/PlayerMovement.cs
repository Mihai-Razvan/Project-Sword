using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    PhotonView photonView;

    [SerializeField] CharacterController controller;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckSize;
    [SerializeField] LayerMask groundMask;   
    string movementState;
    [SerializeField] float walkingSpeed;
    [SerializeField] float runningSpeed;
    float velocity;
    [SerializeField] float gravity;
    [SerializeField] float minimumVelocity;
    [SerializeField] float jumpForce;
    bool grounded;


    void Start()
    {
        photonView = this.gameObject.GetComponent<PhotonView>();

        movementState = "WALKING";
    }

    void Update()
    {
        if (!photonView.IsMine)
            return;

        movement();
        jump();
        verticalMovement();
    }

    void movement()
    {
        switch (movementState)
        {
            case "WALKING":
                walkingState();
                break;
            case "RUNNING":
                runningState();
                break;
        }
    }

    void walkingState()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontalMovement + transform.forward * verticalMovement;
        controller.Move(move * walkingSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftShift))
            movementState = "RUNNING";
    }

    void runningState()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontalMovement + transform.forward * verticalMovement;
        controller.Move(move * runningSpeed * Time.deltaTime);

        if (!Input.GetKey(KeyCode.LeftShift))
            movementState = "WALKING";
    }

    void verticalMovement()
    {
        velocity += gravity * Time.deltaTime;
        if (velocity < minimumVelocity)
            velocity = minimumVelocity;

        Vector3 fall = transform.up * velocity;
        controller.Move(fall * Time.deltaTime);
    }

    void jump()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundCheckSize, groundMask);

        if (Input.GetKeyDown(KeyCode.Space) && grounded == true)
            velocity = jumpForce;
    }

    public void setPlayerPosition(Vector3 newPos)
    {
        controller.enabled = false;
        transform.position = newPos;
        controller.enabled = true;
    }
}
