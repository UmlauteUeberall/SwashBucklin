using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class GameJamLogic : MonoBehaviour
{
    #if !DISABLE_AIRCONSOLE

    void Awake()
    {
        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;
    }

    void OnConnect(int device_id)
    {
        if (AirConsole.instance.GetActivePlayerDeviceIds.Count == 0)
        {
            if (AirConsole.instance.GetControllerDeviceIds().Count >= 2)
            {
                // Start Game with at least 2 Players
                StartGame();
            }
            else
            {
                // Need more Players
            }
        }
    }

    void OnDisconnect(int device_id)
    {
        int active_player = AirConsole.instance.ConvertDeviceIdToPlayerNumber(device_id);
        if (active_player != -1)
        {
            if (AirConsole.instance.GetControllerDeviceIds().Count >= 2)
            {
                // Player Count changed but there are still enough Players to play the game. Restart it!
                StartGame();
            }
            else
            {
                // A Player left the Game and more are needed for the Game to Start again!
                AirConsole.instance.SetActivePlayers(0);
            }
        }
    }

    void OnMessage(int device_id, JToken data)
    {
        int active_player = AirConsole.instance.ConvertDeviceIdToPlayerNumber(device_id);
        if (active_player != -1)
        {
            // Player Message handling here!
        }
    }

    void StartGame()
    {
        AirConsole.instance.SetActivePlayers(2);
    }

    void FixedUpdate()
    {
        // Use Fixed Update for Updating. Never just Update!
    }

    void OnDestroy()
    {
        // Unregister airconsole events on scene change
        if (AirConsole.instance != null)
        {
            AirConsole.instance.onMessage -= OnMessage;
        }
    }

    #endif
}
