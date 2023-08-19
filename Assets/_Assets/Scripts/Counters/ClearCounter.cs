using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{

    public override void Interact(PlayerControl player)
    {
        // Здесь нет KitchenObject
        if (!HasKitchenObject())
        {
            // У игрока есть KitchenObject
            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        } else // Здесь есть KitchenObject
        {
            // У игрока тоже есть KitchenObject
            if (player.HasKitchenObject())
            {
                // Это тарелка
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());
                    }
                } else // Это ингредиент
                {
                    // KitchenObject стола - тарелка
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            KitchenObject.DestroyKitchenObject(player.GetKitchenObject());
                        }
                    }
                }
            } else // У игрока нет KitchenObject
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
