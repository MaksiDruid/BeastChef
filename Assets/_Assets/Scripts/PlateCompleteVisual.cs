using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GO
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GO> kitchenObjectSO_GOList;

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
        
        foreach (KitchenObjectSO_GO kitchenObjectSO_GO in kitchenObjectSO_GOList)
        {
            kitchenObjectSO_GO.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (KitchenObjectSO_GO kitchenObjectSO_GO in kitchenObjectSO_GOList)
        {
            if (kitchenObjectSO_GO.kitchenObjectSO == e.kitchenObjectSO)
            {
                kitchenObjectSO_GO.gameObject.SetActive(true);
            }
        }
    }
}
