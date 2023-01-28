using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerRunningState : MonoBehaviour, IPlayerBaseState
{
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

    public void EnterState(PlayerStateManager player, ArrayList data)
    {
        ReadData(data);
        player.getAnimator().SetBool("Running", true);
    }

    public void UpdateState(PlayerStateManager player)
    {
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");

        move = player.transform.right * horizontalMovement + player.transform.forward * verticalMovement;
        controller.Move(move * runningSpeed * Time.deltaTime);

        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0 && isGrounded() == true)
            player.SwitchState(player.IdleState);
        else if (isGrounded() == false)
        {
            fall(player);
            RaycastHit hit;         //we also do this check because there are a few frames while is is not grounded but is under the height so it changes back and forth between this state and jumping for a few tens of frames
            if (Physics.Raycast(player.transform.position, -player.transform.up, out hit, Mathf.Infinity, groundMask) && player.transform.position.y - hit.point.y >= fallEndHeight)  
                player.SwitchState(player.MidAirState);
        }

         

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded() == true)       //jump
            player.SwitchState(player.JumpingState);
    }

    public void ExitState(PlayerStateManager player)
    {
        player.getAnimator().SetBool("Running", false);
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
}
