using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class PlayerControl : NetworkBehaviour,IKitchenObjectParent
{
    public static event EventHandler OnAnyPlayerSpawned;
    public static event EventHandler OnAnyPickedSomething;
    public static void ResetStaticData()
    {
        OnAnyPlayerSpawned = null;
        OnAnyPickedSomething = null;
    }

    public static PlayerControl LocalInstance {get; private set;}

    public event EventHandler OnPickedSomething; 

    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private Canvas canvas;
    [SerializeField] private LayerMask countersMask;
    [SerializeField] private List<Vector3> spawnPositionList;
    [SerializeField] private PlayerVisual playerVisual;

    private KitchenObject kitchenObject;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    [SerializeField] private float speed;
    private float verticalSpeed = 1.25f;
    private float rotateSpeed = 0.2f;

    private Vector3 lastDir;
    private Rigidbody rb;
    private bool isWalking;

    [SerializeField] private Button interactButton;
    [SerializeField] private Button sliceButton;

    private void Awake()
    {
        interactButton.onClick.AddListener(()=>
        {
            OnInteractButtonDown();
        });

        sliceButton.onClick.AddListener(()=>
        {
            OnSliceButtonDown();
        });
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;

        PlayerData playerData = KitchenGameMultiplayer.Instance.GetPlayerDataFromClientId(OwnerClientId);
        playerVisual.SetPlayerColor(KitchenGameMultiplayer.Instance.GetPlayerColor(playerData.colorId));
    }

    private void KitchenGameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (IsOwner)
        {
            if (KitchenGameManager.Instance.IsGamePlaying()) ShowControl();
            else HideControl();
        }
    }

    private void ShowControl()
    {
        canvas.gameObject.SetActive(true);
    }

    private void HideControl()
    {
        canvas.gameObject.SetActive(false);
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner) LocalInstance = this;

        OnAnyPlayerSpawned?.Invoke(this, EventArgs.Empty);
        transform.position = spawnPositionList[KitchenGameMultiplayer.Instance.GetPlayerDataIndexFromClientId(OwnerClientId)];
        HideControl();

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        }
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        if (clientId == OwnerClientId && HasKitchenObject())
        {
            KitchenObject.DestroyKitchenObject(GetKitchenObject());
        }
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        Movement();
    }

    private void Movement()
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) 
        {
            isWalking = false;
            rb.velocity = Vector3.zero;
        }
        else 
        {
            Vector3 moveDir = new Vector3(joystick.Horizontal, 0f, joystick.Vertical * verticalSpeed);
            if (moveDir!=Vector3.zero) lastDir = moveDir;
            isWalking = moveDir != Vector3.zero;
            rb.velocity = moveDir*speed;
            transform.forward = Vector3.Slerp(transform.forward, moveDir, rotateSpeed);
        }
    }

    private void OnInteractButtonDown()
    {
        float interactDistance = 2f;

        if (Physics.Raycast(transform.position, lastDir, out RaycastHit raycastHit, interactDistance, countersMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                baseCounter.Interact(this);
            }
        }
    }

    private void OnSliceButtonDown()
    {
        float interactDistance = 2f;

        if (Physics.Raycast(transform.position, lastDir, out RaycastHit raycastHit, interactDistance, countersMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                baseCounter.Slice(this);
            }
        }
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    public Transform GetKitcheObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null) 
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
            OnAnyPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }
}
