using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerJumpingState : MonoBehaviour, IPlayerBaseState
{
    [SerializeField] PhotonView view;
    [SerializeField] float jumpForce;
    [SerializeField] float gravity;
    [SerializeField] float minimumVelocity;
    [SerializeField] float midAirMovementSpeed;
    [SerializeField] CharacterController controller;
    float velocity;
    float horizontalMovement;
    float verticalMovement;
    Vector3 fall;
    float timeSinceJumpStart;
    [SerializeField] AnimationClip jumpAnimClip;

    public void OW_EnterState(PlayerStateManager player, ArrayList data)
    {
        ReadData(data);
        velocity = jumpForce;
        timeSinceJumpStart = 0;
        player.getAnimator().SetBool("Jumping", true);
    }

    public void NT_EnterState(PlayerStateManager player)
    {
        player.getAnimator().SetBool("Jumping", true);
    }


    public void UpdateState(PlayerStateManager player)
    {
        inAirMovement(player);
        timeSinceJumpStart += Time.deltaTime;

        if (timeSinceJumpStart >= jumpAnimClip.length)
        {
            ArrayList data = new ArrayList();
            data.Add("JumpingState");
            data.Add(velocity);

            player.SwitchState(player.MidAirState, data);
        }
    }

    public void OW_ExitState(PlayerStateManager player)
    {
        player.getAnimator().SetBool("Jumping", false);
    }

    public void NT_ExitState(PlayerStateManager player)
    {
        player.getAnimator().SetBool("Jumping", false);
    }

    public void ReadData(ArrayList data)
    {

    }

    /////////////////////////////////////////////////////////////////////////////////////////

    void inAirMovement(PlayerStateManager player)        //this happens when player is not grounded; it is jumoing, mid-air or falling
    {
        velocity += gravity * Time.deltaTime;
        if (velocity < minimumVelocity)
            velocity = minimumVelocity;

        fall = player.transform.up * velocity;
        controller.Move(fall * Time.deltaTime);

        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");

        Vector3 move = player.transform.right * horizontalMovement + player.transform.forward * verticalMovement;
        controller.Move(move * midAirMovementSpeed * Time.deltaTime);
    }
}
