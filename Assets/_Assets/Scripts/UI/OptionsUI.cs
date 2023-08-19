using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance{get; private set;}

    [SerializeField] private Button soundButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI soundText;
    [SerializeField] private TextMeshProUGUI musicText;

    private void Awake()
    {
        Instance = this;
        soundButton.onClick.AddListener(()=>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        musicButton.onClick.AddListener(()=>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        backButton.onClick.AddListener(()=>
        {
            Hide();
        });

        
    }

    private void Start()
    {
        UpdateVisual();
        Hide();
    }

    private void UpdateVisual()
    {
        soundText.text = "Sound: " + Mathf.Round(SoundManager.Instance.GetVolume()*5f);
        musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume()*5f);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
