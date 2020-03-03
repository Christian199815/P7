using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FindClient : MonoBehaviour
{
    public TextMeshProUGUI statusText;
    public Button connectButton;
    public TMP_InputField controllerID;
    public string gameSceneName;
    bool needToSearch = true;

    public int port;
    private CustomServer s;
    public string localIP;


    private void Start()
    {
        s = GetComponent<CustomServer>();
        DontDestroyOnLoad(this.gameObject);
        localIP = LocalIPAddress();
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


    public List<string> adresses;
    private void TryConnect()
    {
        connectButton.enabled = false;
        statusText.text = "Connecting...";
        string id = int.Parse(controllerID.text).ToString();
        string ip = localIP.Split('.')[0] + "." + localIP.Split('.')[1] + "." + localIP.Split('.')[2] + "." + id;
        print(ip);
        SendMsg(ip);
    }

    private void SendMsg(string ip)
    {
        TcpClient _client;
        try
        {
            IPAddress ipAd = IPAddress.Parse(ip);
            _client = new TcpClient();
            _client.Connect(ipAd, port);
            TcpClient foundClient = _client;
            NetworkStream foundStream = _client.GetStream();
            SendMessageToServer(localIP, foundClient, foundStream);
            print(ip + " Sent!");
            _client.Client.Disconnect(true);
            _client.Dispose();
            
            s.StartS();
            SceneManager.LoadSceneAsync(gameSceneName);
        }
        catch (SocketException ex)
        {
            print(ex.ToString());
            connectButton.enabled = true;
            statusText.text = "Controller not found!";
            //connection failed
        }
    }



        public void SendMessageToServer(string sendMsg, TcpClient _foundClient, NetworkStream _foundStream)
    {
        if (_foundClient == null || !_foundClient.Connected) return;
        byte[] msg = Encoding.ASCII.GetBytes(sendMsg);
        _foundStream.Write(msg, 0, msg.Length);
    }
}
