using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePausedMPUI : MonoBehaviour
{
    private void Start()
    {
        KitchenGameManager.Instance.OnMPGamePaused += KitchenGameManager_OnMPGamePaused;
        KitchenGameManager.Instance.OnMPGameUnPaused += KitchenGameManager_OnMPGameUnPaused;

        Hide();
    }

    private void KitchenGameManager_OnMPGamePaused(object sender, EventArgs e)
    {
        Show();
    }

    private void KitchenGameManager_OnMPGameUnPaused(object sender, EventArgs e)
    {
        Hide();
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
