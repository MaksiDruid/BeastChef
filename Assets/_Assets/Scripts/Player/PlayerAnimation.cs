using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerAnimation : NetworkBehaviour
{
    private const string IS_WALKING = "IsWalking";
    private Animator animator;
    [SerializeField] private PlayerControl player;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (!IsOwner) return;
        animator.SetBool(IS_WALKING, player.IsWalking());
    }
}
