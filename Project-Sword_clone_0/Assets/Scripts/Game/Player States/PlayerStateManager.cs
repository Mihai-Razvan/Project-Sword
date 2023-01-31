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
    [SerializeField] public PlayerSwordRunningState SwordRunningState;

    void Start()
    {
        currentState = IdleState;
        currentState.OW_EnterState(this, null);
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
        currentState.OW_ExitState(this);
        currentState = state;
        currentState.OW_EnterState(this, null);
        REswitchState();
        Debug.Log("State: " + currentState.GetType().Name);
    }

    public void SwitchState(IPlayerBaseState state, ArrayList data)   //used when we want to pass data between states
    {
        currentState.OW_ExitState(this);
        currentState = state;
        currentState.OW_EnterState(this, data);
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
                SetOthersState(data[1].ToString());
        }
    }

    void SetOthersState(string state)        //called by onEvent; used to change OTHER players states for character that is not yours!
    {
        if (currentState != null)   //currentState is null when join room because of cached events; you join room, the RE is received here before executing Start where currentState is set, so here will be null
            currentState.NT_ExitState(this);

        switch (state)
        {
            case "PlayerIdleState":
                currentState = IdleState;
                break;
            case "PlayerRunningState":
                currentState = RunningState;
                break;
            case "PlayerJumpingState":
                currentState = JumpingState;
                break;
            case "PlayerMidAirState":
                currentState = MidAirState;
                break;
            case "PlayerSwordIdleState":
                currentState = SwordIdleState;
                break;
            case "PlayerSwordRunningState":
                currentState = SwordRunningState;
                break;
        }

        currentState.NT_EnterState(this);
    }

    //////////////////////////////////////////////////////////////////////////////// 

    public Animator getAnimator()
    {
        return animator;
    }
}
