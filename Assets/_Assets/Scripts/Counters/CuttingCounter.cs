using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public static event EventHandler OnAnyCut;

    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs: EventArgs
    {
        public float progressNormalized;
    }
    public event EventHandler OnCut;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;

    public override void Interact(PlayerControl player)
    {
        // Здесь нет KitchenObject
        if (!HasKitchenObject())
        {
            // У игрока есть KitchenObject
            if (player.HasKitchenObject())
            {
                // У игрока нужный ингредиент
                if (HasRecipe(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    KitchenObject kitchenObject = player.GetKitchenObject();
                    // Положить на доску
                    kitchenObject.SetKitchenObjectParent(this);
                    
                    InteractLogicPlaceOnCounterServerRpc();
                }
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
                }
            } else // У игрока нет KitchenObject
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicPlaceOnCounterServerRpc()
    {
        InteractLogicPlaceOnCounterClientRpc();
    }

    [ClientRpc]
    private void InteractLogicPlaceOnCounterClientRpc()
    {
        cuttingProgress = 0;
        
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
            progressNormalized = 0f
        });
    }

    public override void Slice(PlayerControl player)
    {
        if (HasKitchenObject() && HasRecipe(GetKitchenObject().GetKitchenObjectSO()))
        {
            CutObjectServerRpc();

            TestCuttingProgressDoneServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void CutObjectServerRpc()
    {
        CutObjectClientRpc();
    }

    [ClientRpc]
    private void CutObjectClientRpc()
    {
        cuttingProgress++;

        OnCut?.Invoke(this, EventArgs.Empty);
        OnAnyCut?.Invoke(this, EventArgs.Empty);

        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
            progressNormalized = (float)cuttingProgress/cuttingRecipeSO.cuttingProgressMax
        });
    }

    [ServerRpc(RequireOwnership = false)]
    private void TestCuttingProgressDoneServerRpc()
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

        if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
        {
            KitchenObjectSO output = GetOutput(GetKitchenObject().GetKitchenObjectSO());
            
            KitchenObject.DestroyKitchenObject(GetKitchenObject());
            
            KitchenObject.SpawnKitchenObject(output, this);
        }
    }

    private bool HasRecipe(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    }

    private KitchenObjectSO GetOutput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else return null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
