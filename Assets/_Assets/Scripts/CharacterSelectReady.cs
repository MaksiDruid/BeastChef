using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System;


public class CharacterSelectReady : NetworkBehaviour
{
    public static CharacterSelectReady Instance {get; private set;}

    public event EventHandler OnReadyChanged;

    private Dictionary<ulong, bool> playerReadyDirtionary;

    private void Awake()
    {
        Instance = this;
        playerReadyDirtionary = new Dictionary<ulong, bool>();
    }

    public void SetPlayerReady()
    {
        SetPlayerReadyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        SetPlayerReadyClientRpc(serverRpcParams.Receive.SenderClientId);

        playerReadyDirtionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientsReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!playerReadyDirtionary.ContainsKey(clientId) || !playerReadyDirtionary[clientId])
            {
                //не готов
                allClientsReady = false;
                break;
            }
        }
        if (allClientsReady) 
        {
            KitchenGameLobby.Instance.DeleteLobby();
            Loader.LoadNetwork(Loader.Scene.GameScene);
        }
    }

    [ClientRpc]
    private void SetPlayerReadyClientRpc(ulong clientId)
    {
        playerReadyDirtionary[clientId] = true;

        OnReadyChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsPlayerReady(ulong clientId)
    {
        return playerReadyDirtionary.ContainsKey(clientId) && playerReadyDirtionary[clientId];
    }
}
