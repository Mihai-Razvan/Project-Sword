using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerIdleState : MonoBehaviour, IPlayerBaseState
{
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckHeight;
    [SerializeField] float groundCheckWidth;
    [SerializeField] LayerMask groundMask;

    public void EnterState(PlayerStateManager player, ArrayList data)
    {
        ReadData(data);
        player.getAnimator().SetBool("Idle", true);
    }

    public void UpdateState(PlayerStateManager player)
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 && isGrounded() == true)
            player.SwitchState(player.RunningState);
        else if (isGrounded() == false)
            player.SwitchState(player.MidAirState);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded() == true)       //jump
            player.SwitchState(player.JumpingState);
    }

    public void ExitState(PlayerStateManager player)
    {
        player.getAnimator().SetBool("Idle", false);
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
