using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class PlayerStateManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [SerializeField] PhotonView view;
    [SerializeField] PlayerData playerData;
    [SerializeField] Animator animator;

    public IPlayerBaseState currentState;
    [SerializeField] public PlayerIdleState IdleState;
    [SerializeField] public PlayerRunningState RunningState;
    [SerializeField] public PlayerJumpingState JumpingState;
    [SerializeField] public PlayerMidAirState MidAirState;
    [SerializeField] public PlayerSwordIdleState SwordIdleState;


    void Start()
    {
        currentState = IdleState;
        currentState.EnterState(this, null);
        REswitchState();
    }

    void Update()
    {
        if (!view.IsMine)
            return;

        currentState.UpdateState(this);
    }

    public void SwitchState(IPlayerBaseState state)
    {
        currentState.ExitState(this);
        currentState = state;
        currentState.EnterState(this, null);
        REswitchState();
        Debug.Log("State: " + currentState.GetType().Name);
    }

    public void SwitchState(IPlayerBaseState state, ArrayList data)   //used when we want to pass data between states
    {
        currentState.ExitState(this);
        currentState = state;
        currentState.EnterState(this, data);
        REswitchState();
        Debug.Log("State: " + currentState.GetType().Name);
    }

    void REswitchState()        //send a raise event when player state changes
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
    }

    public void OnEvent(ExitGames.Client.Photon.EventData eventData)
    {
        if (eventData.Code == EventsList.SWITCH_PLAYER_STATE_EVENT)
        {
            object[] data = (object[])eventData.CustomData;

            if (PhotonView.Find((int)data[0]) != null && (int)data[0] == view.ViewID)       //player could leave during event propagation so we need to make this check
                SetAnimatorState(data[1].ToString());
        }
    }

    void SetAnimatorState(string state)        //called by onEvent; used to change player animations for character that is not yours! the animation handle for your own player character is done in state classes
    {
        for (int i = 0; i < animator.parameterCount; i++)
            animator.SetBool(animator.parameters[i].name, false);

        switch (state)
        {
            case "PlayerIdleState":
                animator.SetBool("Idle", true);
                break;
            case "PlayerRunningState":
                animator.SetBool("Running", true);
                break;
            case "PlayerJumpingState":
                animator.SetBool("Jumping", true);
                break;
            case "PlayerMidAirState":
                animator.SetBool("Mid-Air", true);
                break;
            case "PlayerSwordIdleState":
                animator.SetBool("Sword-Idle", true);
                break;
        }
    }

    //////////////////////////////////////////////////////////////////////////////// 

    public Animator getAnimator()
    {
        return animator;
    }
}
