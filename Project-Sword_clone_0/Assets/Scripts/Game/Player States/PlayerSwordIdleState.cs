using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordIdleState : MonoBehaviour, IPlayerBaseState
{
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckHeight;
    [SerializeField] float groundCheckWidth;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float fallEndHeight;          //the height over the ground at which mid air turns into falling
    float velocity;
    Vector3 fallAmount;
    [SerializeField] float gravity;
    [SerializeField] float minimumVelocity;
    [SerializeField] CharacterController controller;

    [SerializeField] GameObject swordPrefab;
    [SerializeField] Transform hand;
    [SerializeField] PlayerInventory playerInventory;
    GameObject sword;

    public void EnterState(PlayerStateManager player, ArrayList data)
    {
        ReadData(data);
        setPrefab();
        player.getAnimator().SetBool("Sword-Idle", true);
    }

    public void UpdateState(PlayerStateManager player)
    {
        if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && isGrounded() == true)
            player.SwitchState(player.RunningState);
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
        player.getAnimator().SetBool("Sword-Idle", false);
        Destroy(sword);
    }

    public void ReadData(ArrayList data)
    {

    }

   /* void REspawnPrefab()        //send a raise event which spawns the sword prefab
    {
        object[] data = new object[]
        {
                view.ViewID, currentState.GetType().Name
        };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.Others,
            CachingOption = EventCaching.AddToRoomCache
        };

        ExitGames.Client.Photon.SendOptions sendOptions = new ExitGames.Client.Photon.SendOptions
        {
            Reliability = true
        };

        PhotonNetwork.RaiseEvent(EventsList.SWITCH_PLAYER_STATE_EVENT, data, raiseEventOptions, sendOptions);
    }*/

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
}