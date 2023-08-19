using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabObj;

    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(PlayerControl player)
    {
        if (!player.HasKitchenObject())
        {
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
            
            InteractLogicServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();
    }

    [ClientRpc]
    private void InteractLogicClientRpc()
    {
        OnPlayerGrabObj?.Invoke(this, EventArgs.Empty);
    }
}
