using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 3f;
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 3;

    private void Update()
    {
        if (!IsServer) return;

        spawnPlateTimer+=Time.deltaTime;

        if(spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;

            if (platesSpawnedAmount < platesSpawnedAmountMax) 
            {
                SpawnPlateServerRpc();
            }
        }
    }

    [ServerRpc]
    private void SpawnPlateServerRpc()
    {
        SpawnPlateClientRpc();
    }

    [ClientRpc]
    private void SpawnPlateClientRpc()
    {
        platesSpawnedAmount++;
        OnPlateSpawned?.Invoke(this, EventArgs.Empty);
    }

    public override void Interact(PlayerControl player)
    {
        if (!player.HasKitchenObject())
        {
            if (platesSpawnedAmount > 0)
            {
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                InteractLogicServerRpc();
            }
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
        platesSpawnedAmount--;
        OnPlateRemoved?.Invoke(this, EventArgs.Empty);
    }
}
