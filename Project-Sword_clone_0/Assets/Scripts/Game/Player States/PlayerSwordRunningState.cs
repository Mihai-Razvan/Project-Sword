using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSwordRunningState : MonoBehaviour, IPlayerBaseState
{
    [SerializeField] PhotonView view;
    [SerializeField] PlayerInventory playerInventory;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckHeight;
    [SerializeField] float groundCheckWidth;
    [SerializeField] LayerMask groundMask;
    [SerializeField] CharacterController controller;
    [SerializeField] float runningSpeed;
    [SerializeField] float fallEndHeight;          //the height over the ground at which mid air turns into falling
    float horizontalMovement;
    float verticalMovement;
    Vector3 move;
    float velocity;
    Vector3 fallAmount;
    [SerializeField] float gravity;
    [SerializeField] float minimumVelocity;

    [SerializeField] GameObject swordPrefab;
    [SerializeField] Transform hand;
    GameObject sword;

    void Start()
    {
        if (!view.IsMine)
            return;

        playerInventory.onItemSelected += onItemChanged;
    }

    public void OW_EnterState(PlayerStateManager player, ArrayList data)
    {
        player.getAnimator().SetBool("Sword-Running", true);
        setPrefab();
        ReadData(data);
    }

    public void NT_EnterState(PlayerStateManager player)
    {
        player.getAnimator().SetBool("Sword-Running", true);
        setPrefab();
    }

    public void UpdateState(PlayerStateManager player)
    {
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");

        move = player.transform.right * horizontalMovement + player.transform.forward * verticalMovement;
        controller.Move(move * runningSpeed * Time.deltaTime);

        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0 && isGrounded() == true)
        {
            switch (playerInventory.usedItem)
            {
                case PlayerInventory.Items.None:
                    player.SwitchState(player.IdleState);
                    break;
                case PlayerInventory.Items.Sword:
                    player.SwitchState(player.SwordIdleState);
                    break;
            }
        }
        else if (isGrounded() == false)
        {
            fall(player);
            RaycastHit hit;      //we also do this check because there are a few frames while is is not grounded but is under the height so it changes back and forth between this state and jumping for a few tens of frames
            if (Physics.Raycast(player.transform.position, -player.transform.up, out hit, Mathf.Infinity, groundMask) && player.transform.position.y - hit.point.y >= fallEndHeight)
                player.SwitchState(player.MidAirState);
        }



        if (Input.GetKeyDown(KeyCode.Space) && isGrounded() == true)       //jump
            player.SwitchState(player.JumpingState);
    }

    public void OW_ExitState(PlayerStateManager player)
    {
        player.getAnimator().SetBool("Sword-Running", false);
        Destroy(sword);
    }

    public void NT_ExitState(PlayerStateManager player)
    {
        player.getAnimator().SetBool("Sword-Running", false);
        Destroy(sword);
    }

    public void ReadData(ArrayList data)
    {

    }

    /////////////////////////////////////////////////////////////////////////////////////////

    bool isGrounded()
    {
        return Physics.CheckBox(groundCheck.position, new Vector3(groundCheckWidth, groundCheckHeight, groundCheckWidth), Quaternion.identity, groundMask);
    }

    void fall(PlayerStateManager player)        //we use this because when we change from midair to this state it will be still a lil bit in air and will not be grounded so we want to make it fully fall
    {
        velocity += gravity * Time.deltaTime;
        if (velocity < minimumVelocity)
            velocity = minimumVelocity;

        fallAmount = player.transform.up * velocity;
        controller.Move(fallAmount * Time.deltaTime);
    }

    void setPrefab()
    {
        sword = Instantiate(swordPrefab, hand.position, Quaternion.Euler(hand.eulerAngles.x, hand.eulerAngles.y, hand.eulerAngles.z));
        sword.transform.SetParent(hand);
    }

    void onItemChanged(PlayerStateManager player)      //the player parameter is given from PlayerInventory
    {
        if (!(player.currentState is PlayerSwordRunningState))
            return;

        switch (playerInventory.usedItem)
        {
            case PlayerInventory.Items.None:
                player.SwitchState(player.RunningState);
                break;
            case PlayerInventory.Items.Sword:
                break;
        }
    }
}
