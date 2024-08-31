using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colyseus;
using Colyseus.Schema;
using UnityEditor.Timeline.Actions;
using System;
using Unity.Mathematics;
using System.Xml.Serialization;
public class ColyseusHandler : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerPrefab;
    private ColyseusRoom<ChatRoomState> room;
    private ColyseusClient client;
    private Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();
    private float movementInterval = 0.05f; // Time interval for sending updates
    private float lastMovementTime;
    void Start()
    {
        // Replace with your server address and port
        string serverUrl = "ws://localhost:8082"; 

        // Initialize the Colyseus client
        client = new ColyseusClient(serverUrl);
        ConnectToRoom();
    }

    async void ConnectToRoom()
    {
        try
        {
            // Replace "room_name" with your actual room name
            room = await client.JoinOrCreate<ChatRoomState>("chat");

            room.OnMessage<string>("message", message => {OnSomeEvent(message);});

            room.State.players.OnAdd((key, player) => {OnPlayerAdd(key, player);
            
            player.OnChange(() => 
            {
                Debug.Log("position changed" + player.x);
            
            });
            // player.OnXChange((value, previousValue) =>{
            //     Debug.Log("Player changed: " + value);
            //     if (players.ContainsKey(key))
            //     {
            //         GameObject playerObject = players[key];
            //         playerObject.transform.position = new Vector3(value, playerObject.transform.position.y, playerObject.transform.position.z);
            //     }
            // });
            });

            room.State.players.OnRemove((key, player) => {OnPlayerRemove(key, player);});

            room.State.OnChange(() => {
                Debug.Log("State has been updated!");
            });

            Debug.Log("Connected to room successfully!");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to connect to room: " + ex.Message);
        }
    }

    public async void sendMessageTest(string message)
    {
        // Send a message to the server
        await room.Send("message", message);
    }
    void OnSomeEvent(string message)
    {
        // Handle messages from the server
        Debug.Log("Received message: " + message);
    }

    void OnChangeEvent(string key, string player)
    {
        Debug.Log($"{key} has been changed! {player} and {player} and {players.ContainsKey(key)}");
        if (players.ContainsKey(key))
        {
            GameObject playerObject = players[key];
            //playerObject.transform.position = new Vector3(player, 0, 0);
        }
    }


    void OnPlayerAdd(string key, Player player)
    {
        if (!players.ContainsKey(key))
        {
            GameObject newPlayerObject = Instantiate(playerPrefab);
            newPlayerObject.transform.position = new Vector3(player.x, 0, 0);
            players[key] = newPlayerObject;
        }

    }

    void OnPlayerRemove(string key, Player player)
    {
        if (players.ContainsKey(key))
        {
            Destroy(players[key]);
            players.Remove(key);
        }
    }

    async void Update()
    {
        if (Time.time - lastMovementTime > movementInterval)
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput != 0)
        {
            player.transform.position += new Vector3(horizontalInput, 0, 0);
            await room.Send("move", player.transform.position.x);
            lastMovementTime = Time.time;
        }
    }
    }
}
