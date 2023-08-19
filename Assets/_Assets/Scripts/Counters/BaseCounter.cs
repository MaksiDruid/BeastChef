using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BaseCounter : NetworkBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnAnyObjectPlaceHere;

    public static void ResetStaticData()
    {
        OnAnyObjectPlaceHere = null;
    }

    public virtual void Interact(PlayerControl player){}
    public virtual void Slice(PlayerControl player){}
    
    [SerializeField] private Transform counterTop;

    private KitchenObject kitchenObject;

    public Transform GetKitcheObjectFollowTransform()
    {
        return counterTop;
    }
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null) OnAnyObjectPlaceHere?.Invoke(this, EventArgs.Empty);
    }
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }
}
