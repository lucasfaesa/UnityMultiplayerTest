using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : NetworkBehaviour
{

    [SerializeField] private Button startServerButton;
    [SerializeField] private Button startHostButton;
    [SerializeField] private Button startClientButton;

    [SerializeField] private TextMeshProUGUI playersInGameText;
    public NetworkVariable<int> PlayersInGame { get; } = new NetworkVariable<int>();

    private int previousNumberOfPlayers;
    
    void Awake()
    {
        Cursor.visible = true;

    }
    
    public void CountPlayers()
    {
        UpdatePlayerCountServerRpc();

        /*
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if (IsServer)
            {
                Debug.Log("Player Connected, ID: "  + id);
                PlayersInGame.Value++;
            }

            if (IsOwner)
            {
                UpdatePlayerCountServerRpc();
            }
        };
        
        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if (IsServer)
            {
                Debug.Log("Player Disconnected, ID: " + id);
                PlayersInGame.Value--;
            }

            if (IsOwner)
            {
                UpdatePlayerCountServerRpc();
            }
        };  */
    }
    private void Update()
    {
        if (IsServer)
        {
            PlayersInGame.Value = NetworkManager.Singleton.ConnectedClientsList.Count;

            if (PlayersInGame.Value != previousNumberOfPlayers)
            {
                Debug.Log("Player Connected, ID: "  + NetworkManager.Singleton.ConnectedClientsList[NetworkManager.Singleton.ConnectedClientsList.Count - 1].ClientId);
                previousNumberOfPlayers = PlayersInGame.Value;
                UpdatePlayerCountServerRpc();
            }
        }
    }
    
    [ServerRpc]
    private void UpdatePlayerCountServerRpc()
    {
        UpdatePlayersCountClientRpc();
    }
    
    [ClientRpc]
    private void UpdatePlayersCountClientRpc()
    {
        StartCoroutine(UpdatePlayers());
    }

    private IEnumerator UpdatePlayers()
    {
        yield return new WaitForSeconds(2f);
        playersInGameText.text = "Players in game: " + PlayersInGame.Value;
        Debug.LogError("Updated");
    }

    private void Start()
    {
        startHostButton.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartHost())
            {
                
                Debug.Log("Host started...");
            }
            else
            {
                Debug.Log("Host could not be started!");
            }
        });
        
        startServerButton.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartServer())
            {

                Debug.Log("Server started...");
            }
            else
            {
                Debug.Log("Server could not be started!");
            }
        });
        
        startClientButton.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartClient())
            {

                Debug.Log("Client entered...");
            }  
            else
            {
                Debug.Log("Client could not enter!");
            }
        });
    }
}