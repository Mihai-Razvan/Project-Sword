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
    float horizontalMovement;
    float verticalMovement;
    Vector3 move;

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
            player.SwitchState(player.MidAirState);

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
}
