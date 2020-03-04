using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using TMPro;
using System.Text;
using System;

public class NiekServer : MonoBehaviour
{
    private TcpListener Server = null;
    private TcpClient Client = null;
    private NetworkStream netStream = null;
    private byte[] Buffer = new byte[49152];
    private string receivedMessage = "";

    public TextMeshProUGUI statusText;

    private int bytesReceived = 0;
    public int port;

    InputManager iMan;

    private void OnDestroy()
    {
        StopServer();
    }

    void Awake()
    {
        iMan = GetComponent<InputManager>();
    }

    public void StartServer()
    {
        try
        {
            IPAddress ip = IPAddress.Parse(LocalIPAddress());
            Server = new TcpListener(ip, port);
            Server.Start();
            Server.BeginAcceptTcpClient(ClientConnected, null);
            statusText.text = "Establishing connection...";
            Debug.Log("Server started!");
        }
        catch(SocketException e) { Debug.Log(e.ToString()); }
    }

    public void StopServer()
    {
        if (Client != null)
        {
            if (Client.Connected)
            {
                netStream?.Close();
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
    }

    private void ClientConnected(IAsyncResult res)
    {
        Client = Server.EndAcceptTcpClient(res);
        statusText.text = "Connection Established!";
        StartCoroutine(CheckClientConnection());
        StartCoroutine(ListenClientMessages());
        
    }

    public IEnumerator CheckClientConnection()
    {
        while (Client.Connected)
        {
            yield return null;
        }
        statusText.text = "Connection Lost!";
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




    private IEnumerator ListenClientMessages()
    {
        bytesReceived = 0;
        Buffer = new byte[49152];
        netStream = Client.GetStream();
        do
        {
            netStream.BeginRead(Buffer, 0, Buffer.Length, MessageReceived, netStream);
            if (bytesReceived > 0)
            {
                OnMessageReceived(receivedMessage);
                bytesReceived = 0;
            }

            yield return null;

        } while (bytesReceived >= 0 && netStream != null);
        Debug.Log("end");
    }

    protected virtual void OnMessageReceived(string receivedMessage)
    {
        switch (receivedMessage)
        {
            case "Close":
                SendMessageToClient("Close");
                break;
            default:
                Debug.Log(receivedMessage);

                if (iMan.inputType == InputType.MobileController)
                {
                    string[] message = receivedMessage.Split('/');
                    iMan.axis.x = float.Parse(message[0]);
                    iMan.axis.y = float.Parse(message[1]);
                }

                break;
        }
    }

    //Send custom string msg to client
    protected void SendMessageToClient(string sendMsg)
    {
        //early out if there is nothing connected       
        if (netStream == null)
        {
            return;
        }

        //Build message to client        
        byte[] msgOut = Encoding.ASCII.GetBytes(sendMsg); //Encode message as bytes
        //Start Sync Writing
        netStream.Write(msgOut, 0, msgOut.Length);
    }

    //AsyncCallback called when "BeginRead" is ended, waiting the message response from client
    private void MessageReceived(IAsyncResult result)
    {
        if (result.IsCompleted && Client.Connected)
        {
            //build message received from client
            bytesReceived = netStream.EndRead(result);                              //End async reading
            receivedMessage = Encoding.ASCII.GetString(Buffer, 0, bytesReceived); //De-encode message as string
        }
    }


}
