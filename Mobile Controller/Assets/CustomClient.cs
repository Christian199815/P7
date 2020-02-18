using System;
using System.Collections;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This Client inheritated class acts like Client using UI elements like buttons and input fields.
/// </summary>
public class CustomClient : Client
{
    public GameObject connectPanel;
    public Text inputText;
    public Button connectButton;
    public TextMeshProUGUI connectionStatus;

    public Vector2 axis;

    //Set UI interactable properties
    private void Awake()
    {
        //connectButton.onClick.AddListener(() => StartConnection());
        base.OnClientStarted += ClientStarted;
        base.OnClientClosed += ClientClosed;

        //StartConnection();
        connectButton.onClick.AddListener(() => StartConnection());

    }
    public void StartConnection()
    {
        if (ipAddress.Length <= 3)
        {
            connectionStatus.text = "Invalid IP!";
            return;
        }

        //base.ipAddress = inputText.text;
        connectButton.interactable = false;
        base.StartClient(connectionStatus);
    }
    private void ClientClosed()
    {
        /*
        connectButton.interactable = true;
        connectPanel.SetActive(true);
        connectionStatus.text = "Connection closed";*/
    }
    private void ClientStarted()
    {
        connectPanel.SetActive(false);
    }

    private void OnDisable()
    {
        SendCloseToServer();
    }


    private void SendCloseToServer()
    {
        base.SendMessageToServer("Close");
    }
}