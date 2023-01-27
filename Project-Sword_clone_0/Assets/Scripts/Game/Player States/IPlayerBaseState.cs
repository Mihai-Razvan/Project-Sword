using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerBaseState
{
#nullable enable
    public abstract void EnterState(PlayerStateManager player, ArrayList? data);

    public abstract void UpdateState(PlayerStateManager player);

    public abstract void ExitState(PlayerStateManager player);

    public abstract void ReadData(ArrayList data);
}
