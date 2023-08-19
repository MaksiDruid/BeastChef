using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance{get; private set;}

    private void Awake()
    {
        Instance = this;
    }

    public override void Interact(PlayerControl player)
    {
        if (player.HasKitchenObject())
        {
            // удаляем только тарелки
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);

                KitchenObject.DestroyKitchenObject(player.GetKitchenObject());
            }
        }
    }
}
