using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using TMPro;

public class FinishedClient : MonoBehaviour
{
    public TextMeshProUGUI status;
    public GameObject connectionPanel;
    public GameObject controllerHomeScreenPanel;
    #region Public Variables
    [Header("Network")]
    public string ipAdress;
    public int port = 54010;
    public float waitingMessagesFrequency = 2;
    #endregion

    private TcpListener Server = null;
    private TcpClient Client = null;
    private NetworkStream netStream = null;
    private byte[] Buffer = new byte[49152];
    private int bytesReceived = 0;
    private string receivedMessage = "";

    public bool PairedToGame = false;


    private void Start()
    {
        ipAdress = LocalIPAddress();
        StartServer();
        StartCoroutine(ConnectPanel());
    }

    private IEnumerator ConnectPanel()
    {
        while (true)
        {
            while (!PairedToGame)
            {
                yield return null;
            }
            connectionPanel.SetActive(false);
            controllerHomeScreenPanel.SetActive(true);
            while (PairedToGame)
            {
                yield return null;
            }
            connectionPanel.SetActive(true);
            status.text = "Disconnected. Restart app and try to reconnect.";
        }
    }

    public static string LocalIPAddress()
    {
        IPHostEntry host;
        host = Dns.GetHostEntry(Dns.GetHostName());
        List<string> ips = new List<string>();
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                ips.Add(ip.ToString());
            }
        }
        return ips[ips.Count - 1];
    }

    public void StartServer()
    {

        //Set and enable Server 
        IPAddress ip = IPAddress.Parse(ipAdress);
        Server = new TcpListener(ip, port);
        Server.Start();
        //Wait for async client connection 
        Server.BeginAcceptTcpClient(ClientConnected, null);
        status.text = "Controller ID:\n" + ipAdress.Split('.')[3];
        StartCoroutine(ListenServerMessages());
    }


    //Callback called when "BeginAcceptTcpClient" detects new client connection
    private void ClientConnected(IAsyncResult res)
    {
        Client = Server.EndAcceptTcpClient(res);
        Connected();
    }

    private void Connected()
    {
        PairedToGame = true;
        netStream = Client.GetStream();
        StartCoroutine(ConnectionCheck());
        SendMessageToServer("I'm Connected!");
    }

    private IEnumerator ConnectionCheck()
    {
        while (netStream != null)
        {
            yield return null;
        }
        connectionPanel.SetActive(true);
    }

    protected virtual void CloseServer()
    {
        SendMessageToServer("Disconnected");
        if (Client != null)
        {
            if (Client.Connected)
            {
                netStream.Close();
                netStream = null;
                Client.Close();
                Client = null;
            }

        }

        if (Server != null)
        {
            Server.Stop();
            Server = null;
        }
        PairedToGame = false;
    }

    //Close connection with the client
    protected virtual void CloseClientConnection()
    {
        //Reset everything to defaults
        StopCoroutine(ListenServerMessages());
        Client.Close();
        Client = null;

        //Waiting to Accept a new Client
        Server.BeginAcceptTcpClient(ClientConnected, null);
    }

    string m_receivedMessage;

    private IEnumerator ListenServerMessages()
    {
        while(Client == null)
        {
            yield return null;
            
        }
        if (Client != null)
        {


            netStream = Client.GetStream();
            do
            {
                netStream.BeginRead(Buffer, 0, Buffer.Length, MessageReceived, null);

                if (bytesReceived > 0)
                {
                    OnMessageReceived(receivedMessage);
                    bytesReceived = 0;
                }

                yield return null;

            } while (bytesReceived >= 0 && netStream != null);
            CloseServer();
        }
    }

    protected virtual void OnMessageReceived(string receivedMessage)
    {
        if (this.receivedMessage == "Disconnected")
        {
            CloseServer();
            status.text = "Disconnected. Restart app and try to reconnect.";
        }
        if (this.receivedMessage == "HOME")
        {
            controllerHomeScreenPanel.SetActive(true);
        }
        if (this.receivedMessage == "GAME")
        {
            controllerHomeScreenPanel.SetActive(false);
        }

        print(receivedMessage);
        //Commands toevoegen dependent op de message (voorbeeld bovenstaand)
    }

    private void MessageReceived(IAsyncResult result)
    {
        if (result.IsCompleted && Client.Connected)
        {
            bytesReceived = netStream.EndRead(result);
            receivedMessage = Encoding.ASCII.GetString(Buffer, 0, bytesReceived);
        }
    }

    public void SendMessageToServer(string sendMsg)
    {
        if (Client == null || !Client.Connected) return;
        byte[] msg = Encoding.ASCII.GetBytes(sendMsg);
        netStream.Write(msg, 0, msg.Length);
    }

}

