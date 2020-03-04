using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using TMPro;

/// <summary>
/// Server class shows how to implement and use TcpListener in Unity.
/// </summary>
public class GetServer : MonoBehaviour
{
    public TextMeshProUGUI status;
    #region Public Variables
    [Header("Network")]
    public string ipAdress;
    public int port = 54010;
    public float waitingMessagesFrequency = 2;
    #endregion

    #region  Private m_Variables
    private TcpListener m_Server = null;
    private TcpClient m_Client = null;
    private NetworkStream m_NetStream = null;
    private byte[] m_Buffer = new byte[49152];
    private int m_BytesReceived = 0;
    private string m_ReceivedMessage = "";
    private IEnumerator m_ListenClientMsgsCoroutine = null;
    #endregion


    private NiekClient nClient;

    private void Start()
    {
        nClient = GetComponent<NiekClient>();
        ipAdress = LocalIPAddress();
        StartServer();
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
        m_Server = new TcpListener(ip, port);
        m_Server.Start();
        //Wait for async client connection 
        m_Server.BeginAcceptTcpClient(ClientConnected, null);
        status.text = "Controller ID: " + ipAdress.Split('.')[3];
    }


    //Callback called when "BeginAcceptTcpClient" detects new client connection
    private void ClientConnected(IAsyncResult res)
    {
        //set the client reference
        m_Client = m_Server.EndAcceptTcpClient(res);
        string a = m_Client.Client.RemoteEndPoint.ToString();
        nClient.connectionIP = a.Split(':')[0];
        status.text += "\nFound server!";
        Debug.Log("Starting server");
        nClient.StartClient();
        CloseServer();

    }






    #region Close Server/ClientConnection
    //Close client connection and disables the server
    protected virtual void CloseServer()
    {
        //Close client connection
        if (m_Client != null)
        {
            if (m_Client.Connected)
            {
                m_NetStream.Close();
                m_NetStream = null;
                m_Client.Close();
                m_Client = null;
            }

        }

        if (m_Server != null)
        {
            m_Server.Stop();
            m_Server = null;
        }
    }

    //Close connection with the client
    protected virtual void CloseClientConnection()
    {
        //Reset everything to defaults
        StopCoroutine(m_ListenClientMsgsCoroutine);
        m_ListenClientMsgsCoroutine = null;
        m_Client.Close();
        m_Client = null;

        //Waiting to Accept a new Client
        m_Server.BeginAcceptTcpClient(ClientConnected, null);
    }
    #endregion

    #region ServerLog
    //Custom Server Log - With Text Color
    protected virtual void ServerLog(string msg, Color color)
    {
        print(msg);
    }
    //Custom Server Log - Without Text Color
    protected virtual void ServerLog(string msg)
    {
        print(msg);
    }
    #endregion

    string m_receivedMessage;
    private IEnumerator ListenServerMessages()
    {
        //early out if there is nothing connected       
        if (!m_Client.Connected)
            yield break;

        //Stablish Client NetworkStream information
        m_NetStream = m_Client.GetStream();

        //Start Async Reading from Server and manage the response on MessageReceived function
        do
        {
            //Start Async Reading from Server and manage the response on MessageReceived function
            m_NetStream.BeginRead(m_Buffer, 0, m_Buffer.Length, MessageReceived, null);

            if (m_BytesReceived > 0)
            {
                print(m_receivedMessage);
                m_BytesReceived = 0;
            }
            yield return new WaitForSeconds(waitingMessagesFrequency);

        } while (m_BytesReceived >= 0 && m_NetStream != null);
        //The communication is over
        //CloseClient();
    }

    private void MessageReceived(IAsyncResult result)
    {
        if (result.IsCompleted && m_Client.Connected)
        {
            //build message received from server
            m_BytesReceived = m_NetStream.EndRead(result);
            m_ReceivedMessage = Encoding.ASCII.GetString(m_Buffer, 0, m_BytesReceived);
        }
    }

    public void SendMessageToServer(string sendMsg, TcpClient _foundClient, NetworkStream _foundStream)
    {
        if (_foundClient == null || !_foundClient.Connected) return;
        byte[] msg = Encoding.ASCII.GetBytes(sendMsg);
        _foundStream.Write(msg, 0, msg.Length);
    }

}