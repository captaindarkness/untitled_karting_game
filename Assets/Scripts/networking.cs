﻿using UnityEngine;
using System.Collections;
using System;

public class networking : MonoBehaviour 
{

    public string connectToIp = "127.0.0.1";
    public int connectPort = 25000;
    public string ipadress = "";
    public string port = "";

    string playerName = "<NAME ME>";

    void OnGUI() 
    {
        if (Network.peerType == NetworkPeerType.Disconnected)
        {
            if (GUILayout.Button("Connect"))
            {
                if (playerName != "<NAME ME>")
                {
                    
                    Network.Connect(connectToIp, connectPort);
                    PlayerPrefs.SetString("playerName", playerName);
                }
            }

            if (GUILayout.Button("Start Server"))
            {
                if (playerName != "<NAME ME>")
                {
                    Network.InitializeServer(32, connectPort,false);

                    foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
                    {
                        go.SendMessage("OnNetworkLoadedLevel", SendMessageOptions.DontRequireReceiver);
                    }
                    PlayerPrefs.SetString("playerName", playerName);
                }
            }
            playerName = GUILayout.TextField(playerName);
            connectToIp = GUILayout.TextField(connectToIp);
            connectPort = Convert.ToInt32(GUILayout.TextField(connectPort.ToString()));
        }
        else 
        {
            if (Network.peerType == NetworkPeerType.Connecting)
            {
                GUILayout.Label("Connect Status: Connecting");
            }
            else if(Network.peerType == NetworkPeerType.Client)
            {
                GUILayout.Label("Connection Status: Client!!");
                GUILayout.Label("Ping to Server: " + Network.GetAveragePing(Network.connections[0]));
            }
            else if (Network.peerType == NetworkPeerType.Server)
            {
                GUILayout.Label("Connection Status: Server!");
                GUILayout.Label("Connections: " + Network.connections.Length);
                if(Network.connections.Length >= 1)
                {
                    GUILayout.Label("Ping to Server: " + Network.GetAveragePing(Network.connections[0]));
                }
            }

            if(GUILayout.Button("Disconnect"))
            {
                Network.Disconnect(200);
            }

            ipadress = Network.player.ipAddress;
            port = Network.player.port.ToString();
            GUILayout.Label("IP Adress: " + ipadress + ":" + port);
        }
           
        
    }

    void OnConnectedToServer() 
    {
        foreach (GameObject go in FindObjectsOfType(typeof(GameObject))) 
        {
            go.SendMessage("OnNetworkLoadedLevel", SendMessageOptions.DontRequireReceiver);
        }
    }
    
}
