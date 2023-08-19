using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Unity.Netcode;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button optionsButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(()=>
        {
            KitchenGameManager.Instance.PauseGame();
        });

        optionsButton.onClick.AddListener(()=>
        {
            OptionsUI.Instance.gameObject.SetActive(true);
        });

        quitButton.onClick.AddListener(()=>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnLocalGamePaused += KitchenGameManager_OnLocalGamePaused;
        KitchenGameManager.Instance.OnLocalGameUnPaused += KitchenGameManager_OnLocalGameUnPaused;
        Hide();
    }

    private void KitchenGameManager_OnLocalGameUnPaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void KitchenGameManager_OnLocalGamePaused(object sender, EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }
}
