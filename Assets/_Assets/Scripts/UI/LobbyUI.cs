using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button createButton;
    [SerializeField] private Button joinButton;
    [SerializeField] private Button joinCodeButton;
    [SerializeField] private TMP_InputField codeInputField;
    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private LobbyCreateUI lobbyCreateUI;
    [SerializeField] private Transform lobbyContainer;
    [SerializeField] private Transform lobbyTemplate;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(()=>
        {
            KitchenGameLobby.Instance.LeaveLobby();
            Loader.Load(Loader.Scene.MainMenuScene);
        });

        createButton.onClick.AddListener(()=>
        {
            lobbyCreateUI.Show();
        });

        joinButton.onClick.AddListener(()=>
        {
            KitchenGameLobby.Instance.QuickJoin();
        });

        joinCodeButton.onClick.AddListener(()=>
        {
            KitchenGameLobby.Instance.JoinWithCode(codeInputField.text);
        });
    }

    private void Start()
    {
        if (KitchenGameMultiplayer.playMultiplayer == false)
        {
            Hide();
            return;
        }

        playerNameInputField.text = KitchenGameMultiplayer.Instance.GetPlayerName();
        playerNameInputField.onValueChanged.AddListener((string newText)=>
        {
            KitchenGameMultiplayer.Instance.SetPlayerName(newText);
        });

        KitchenGameLobby.Instance.OnLobbiesListChanged += KitchenGameLobby_OnLobbiesListChanged;
        UpdateLobbiesList(new List<Lobby>());
    }

    private void KitchenGameLobby_OnLobbiesListChanged(object sender, KitchenGameLobby.OnLobbiesListChangedEventArgs e)
    {
        UpdateLobbiesList(e.lobbiesList);
    }

    private void UpdateLobbiesList(List<Lobby> lobbiesList)
    {
        foreach(Transform child in lobbyContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Lobby lobby in lobbiesList)
        {
            Transform lobbyTransform = Instantiate(lobbyTemplate, lobbyContainer);
            lobbyTransform.GetComponent<LobbiesListSingleUI>().SetLobby(lobby);
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        KitchenGameLobby.Instance.OnLobbiesListChanged -= KitchenGameLobby_OnLobbiesListChanged;
    }
}
