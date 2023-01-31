using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerBaseState
{
#nullable enable
    public abstract void OW_EnterState(PlayerStateManager player, ArrayList? data);       //used if we are the owner of this character
     
    public abstract void NT_EnterState(PlayerStateManager player);                       //used if we this character is not ours

    public abstract void UpdateState(PlayerStateManager player);

    public abstract void OW_ExitState(PlayerStateManager player);

    public abstract void NT_ExitState(PlayerStateManager player);

    public abstract void ReadData(ArrayList data);
}
