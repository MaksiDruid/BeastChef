using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    private const string POP = "Pop";

    [SerializeField] private Image icon;
    [SerializeField] private Sprite success;
    [SerializeField] private Sprite fail;
    [SerializeField] private Color successColor;
    [SerializeField] private Color failColor;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSucceed += DeliveryManager_OnRecipeSucceed;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        
        Hide();
    }

    private void DeliveryManager_OnRecipeSucceed(object sender, EventArgs e)
    {
        Show();
        animator.SetTrigger(POP);
        icon.sprite = success;
        icon.color = successColor;
    }
    private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e)
    {
        Show();
        animator.SetTrigger(POP);
        icon.sprite = fail;
        icon.color = failColor;
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
