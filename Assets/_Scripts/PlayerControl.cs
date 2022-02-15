using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem;

public class PlayerControl : NetworkBehaviour
{
    [SerializeField] private float walkSpeed = 3.5f;
    [SerializeField] private Vector2 defaultPositionRange = new Vector2(-4, 4);
    [SerializeField] private NetworkVariable<float> forwardBackPosition = new NetworkVariable<float>();
    [SerializeField] private NetworkVariable<float> leftRightPosition = new NetworkVariable<float>();
    [SerializeField] private Animator animator;

    private static readonly int Speed = Animator.StringToHash("Speed");

    private bool pressingKey;
    //client caching
    //private float oldForwardBackPosition;
    //private float oldLeftRightPosition;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Random.Range(defaultPositionRange.x, defaultPositionRange.y), 0,
            Random.Range(defaultPositionRange.x, defaultPositionRange.y));
    }

    private void Update()
    {
        /*Debug.Log("Client: " + IsClient + " Server: " + IsServer + " Owner: " + IsOwner + 
                  " OwnedByServer: " + IsOwnedByServer + " IsLocalPlayer: " + IsLocalPlayer + " IsHost: " + IsHost);*/
        if (IsClient && IsOwner)
        {
            Move();
        }
    }

    void Move()
    {
        float playerSpeed = 3;
        if (Keyboard.current.wKey.isPressed)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed);
            //animator.SetFloat(Speed, 5);
            SetAnimatorSpeedValueServerRpc(5);
        }


        if (Keyboard.current.dKey.isPressed)
        {
            transform.Translate(Vector3.right * Time.deltaTime * playerSpeed);
            SetAnimatorSpeedValueServerRpc(5);
        }

        if (Keyboard.current.sKey.isPressed)
        {
            transform.Translate(Vector3.back * Time.deltaTime * playerSpeed);
            SetAnimatorSpeedValueServerRpc(5);
        }

        if (Keyboard.current.aKey.isPressed)
        {
            transform.Translate(Vector3.left * Time.deltaTime * playerSpeed);
            SetAnimatorSpeedValueServerRpc(5);
        }

        if (!Keyboard.current.aKey.isPressed && !Keyboard.current.wKey.isPressed && !Keyboard.current.dKey.isPressed &&
            !Keyboard.current.sKey.isPressed)
        {
            SetAnimatorSpeedValueServerRpc(0);
        }
        
        /*
        if (IsOwner)
        {
            if (Keyboard.current.rKey.wasReleasedThisFrame)
            {
                FindObjectOfType<UIManager>().CountPlayers();
            }    
        }
        */
    }

    [ServerRpc]
    private void SetAnimatorSpeedValueServerRpc(float value)
    {
        SetAnimatorSpeedValueClientRpc(value);
    }

    [ClientRpc]
    private void SetAnimatorSpeedValueClientRpc(float value)
    {
        animator.SetFloat(Speed, value);
    }

}
