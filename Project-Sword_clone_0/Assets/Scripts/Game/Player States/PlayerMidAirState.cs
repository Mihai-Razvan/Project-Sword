using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMidAirState : MonoBehaviour, IPlayerBaseState
{
    [SerializeField] PlayerInventory playerInventory;
    [SerializeField] float gravity;
    [SerializeField] float minimumVelocity;
    [SerializeField] float midAirMovementSpeed;
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float fallEndHeight;          //the height over the ground at which mid air turns into falling
    float velocity;
    float horizontalMovement;
    float verticalMovement;
    Vector3 fall;

    public void EnterState(PlayerStateManager player, ArrayList data)
    {
        ReadData(data);
        player.getAnimator().SetBool("Mid-Air", true);
    }

    public void UpdateState(PlayerStateManager player)
    {
        inAirMovement(player);

        RaycastHit hit;
        if (Physics.Raycast(player.transform.position, -player.transform.up, out hit, Mathf.Infinity, groundMask))
            if (player.transform.position.y - hit.point.y < fallEndHeight)
            {
                if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
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
                else
                    player.SwitchState(player.RunningState);
            }
    }

    public void ExitState(PlayerStateManager player)
    {
        player.getAnimator().SetBool("Mid-Air", false);
    }

    public void ReadData(ArrayList data)
    {
        if (data == null)
            return;

        switch (data[0])
        {
            case "JumpingState":
                velocity = (float)data[1];
                break;
        }
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
