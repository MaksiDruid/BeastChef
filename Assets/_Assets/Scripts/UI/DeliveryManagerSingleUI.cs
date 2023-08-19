using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform ingredientTemplate;
    
    public void SetRecipeSO(RecipeSO recipeSO)
    {
        icon.sprite = recipeSO.sprite;

        foreach(Transform child in iconContainer)
        {
            Destroy(child.gameObject);
        }

        foreach(KitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectSOList)
        {
            Transform iconTransform = Instantiate(ingredientTemplate, iconContainer);
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }
    }
}
