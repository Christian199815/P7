using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using TMPro;
using System.Text;
using System;
public class NiekClient : MonoBehaviour
{
    [SerializeField] private GameObject connectPanel;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private int port;

    private TcpClient activeClient;

    public string connectionIP;

    protected NetworkStream m_NetStream = null;
    private byte[] m_Buffer = new byte[49152];
    private int m_BytesReceived = 0;
    private string m_ReceivedMessage = "";

    public List<string> adresses = new List<string>();
    void Start()
    {
        StartCoroutine(ConnectSequence());
    }

    private IEnumerator ConnectSequence()
    {
        connectPanel.SetActive(true);
        statusText.text = "Finding server...";

        while(connectionIP == string.Empty)
        {
            yield return null;
        }

        activeClient = TryToConnect();
        if (activeClient == null)
        {
            statusText.text = "Connection error";
            statusText.text += "\nTrying again in 10 seconds...";
            yield return new WaitForSeconds(10);
            StartCoroutine(ConnectSequence());
            yield break;
        }
        else
        {
            statusText.text = "Server found!";
            statusText.text += "\nSetting up connection...";
            yield return new WaitForSeconds(3);
            connectPanel.SetActive(false);
            yield break;
        }
    }

    private TcpClient TryToConnect()
    {
        TcpClient _client;
        try
        {
            IPAddress ipAd = IPAddress.Parse(connectionIP);
            _client = new TcpClient();
            _client.Connect(ipAd, port);
            if (_client.Connected)
            {
                activeClient = _client;
                m_NetStream = _client.GetStream();
                StartCoroutine(ConnectionCheck());
                return _client;
            }
        }
        catch (SocketException)
        {

        }
        return null;
    }



    private IEnumerator ConnectionCheck()
    {
        while (activeClient.Connected)
        {
            yield return new WaitForSeconds(1);
        }
        CloseClient();
    }

    private void CloseClient()
    {
        if (activeClient.Connected)
            activeClient.Close();

        if (activeClient != null)
            activeClient = null;

        StopAllCoroutines();
        StartCoroutine(ConnectSequence());
    }
    private IEnumerator ListenServerMessages()
    {
        if (activeClient != null)
        {


        m_NetStream = activeClient.GetStream();
        do
        {
            m_NetStream.BeginRead(m_Buffer, 0, m_Buffer.Length, MessageReceived, null);

            if (m_BytesReceived > 0)
            {
                OnMessageReceived(m_ReceivedMessage);
                m_BytesReceived = 0;
            }
            
            yield return new WaitForSeconds(1);

        } while (m_BytesReceived >= 0 && m_NetStream != null);
        CloseClient();
        }
    }

    private void MessageReceived(IAsyncResult result)
    {
        if (result.IsCompleted && activeClient.Connected)
        {
            m_BytesReceived = m_NetStream.EndRead(result);
            m_ReceivedMessage = Encoding.ASCII.GetString(m_Buffer, 0, m_BytesReceived);
        }
    }

    protected virtual void OnMessageReceived(string receivedMessage)
    {
        print(receivedMessage);
        if (m_ReceivedMessage == "Server_Close")
        {
            CloseClient();
        }
    }

    public void SendMessageToServer(string sendMsg)
    { 
        if (activeClient == null || !activeClient.Connected) return;
        byte[] msg = Encoding.ASCII.GetBytes(sendMsg);
        m_NetStream.Write(msg, 0, msg.Length);
    }
}
