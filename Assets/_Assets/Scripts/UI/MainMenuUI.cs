using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
   // [SerializeField] private Button singleButton;
    [SerializeField] private Button onlineButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Image loadingScrene;

    private void Awake()
    {
        // singleButton.onClick.AddListener(()=>
        // {
        //     KitchenGameMultiplayer.playMultiplayer = false;
        //     ShowLoading();
        //     Loader.Load(Loader.Scene.LobbyScene);
        // });

        onlineButton.onClick.AddListener(()=>
        {
            KitchenGameMultiplayer.playMultiplayer = true;
            ShowLoading();
            Loader.Load(Loader.Scene.LobbyScene);
        });

        quitButton.onClick.AddListener(()=>
        {
            Application.Quit();
        });
        Time.timeScale = 1f;
    }

    private void Start()
    {
        HideLoading();
    }

    private void ShowLoading()
    {
        loadingScrene.gameObject.SetActive(true);
    }

    private void HideLoading()
    {
        loadingScrene.gameObject.SetActive(false);
    }
    
}
