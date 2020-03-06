using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class FindClient : MonoBehaviour
{
    bool needToSearch = true;

    public int port;

    public string localIP;


    private void Start()
    {
           StartCoroutine(FindServers());
    }

    private IEnumerator FindServers()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            while (needToSearch)
            {
                Search();
                yield return new WaitForSeconds(3);
            }
        }
    }

    public List<string> adresses;
    private void Search()
    {
        adresses = new List<string>();
        foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211|| ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
            {
                foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        adresses.Add(ip.Address.ToString());
                    }
                }
            }
        }

        SendMessages();
    }

    private TcpClient SendMessages()
    {
        TcpClient _client;
        foreach (string ip in adresses)
        {
            try
            {
                IPAddress ipAd = IPAddress.Parse(ip);
                _client = new TcpClient();
                _client.Connect(ipAd, port);
                if (_client.Connected)
                {
                    TcpClient foundClient = _client;
                    NetworkStream foundStream = _client.GetStream();
                    SendMessageToServer(localIP, foundClient, foundStream);
                    print(localIP + " Sent!");

                    _client.Dispose();
                }
            }
            catch (SocketException)
            {
                continue;
            }
        }
        return null;
    }

    public void SendMessageToServer(string sendMsg, TcpClient _foundClient, NetworkStream _foundStream)
    {
        if (_foundClient == null || !_foundClient.Connected) return;
        byte[] msg = Encoding.ASCII.GetBytes(sendMsg);
        _foundStream.Write(msg, 0, msg.Length);
    }
}
