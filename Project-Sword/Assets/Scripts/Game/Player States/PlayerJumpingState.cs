using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerJumpingState : MonoBehaviour, IPlayerBaseState
{
    [SerializeField] float jumpForce;
    [SerializeField] float gravity;
    [SerializeField] float minimumVelocity;
    [SerializeField] float midAirMovementSpeed;
    [SerializeField] CharacterController controller;
    float velocity;
    float horizontalMovement;
    float verticalMovement;
    Vector3 fall;

    public void EnterState(PlayerStateManager player, ArrayList data)
    {
        ReadData(data);
        player.getAnimator().SetBool("Jumping", true);
        velocity = jumpForce;
    }

    public void UpdateState(PlayerStateManager player)
    {
        inAirMovement(player);

        if (player.getAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)         //jump start has ended
        {
            ArrayList data = new ArrayList();
            data.Add("JumpingState");
            data.Add(velocity);

            player.SwitchState(player.MidAirState, data);
        }
    }

    public void ExitState(PlayerStateManager player)
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
