using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class PlayerStateManager : MonoBehaviour
{
    [SerializeField] PhotonView view;
    [SerializeField] Animator animator;

    IPlayerBaseState currentState;
    [SerializeField] public PlayerIdleState IdleState;
    [SerializeField] public PlayerRunningState RunningState;
    [SerializeField] public PlayerJumpingState JumpingState;
    [SerializeField] public PlayerMidAirState MidAirState;


    void Start()
    {
        currentState = IdleState;
        currentState.EnterState(this, null);
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
    }

    public void SwitchState(IPlayerBaseState state, ArrayList data)   //used when we want to pass data between scenes
    {
        currentState.ExitState(this);
        currentState = state;
        currentState.EnterState(this, data);
    }

    public Animator getAnimator()
    {
        return animator;
    }
}
