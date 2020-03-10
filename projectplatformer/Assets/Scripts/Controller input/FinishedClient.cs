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

[RequireComponent(typeof(InputManager))]
public class FinishedClient : MonoBehaviour
{
    public TextMeshProUGUI statusText;
    public Button connectButton;
    public TMP_InputField controllerID;
    public string gameSceneName;

    public int port;
    public string localIP;

    private TcpClient Client = null;
    private NetworkStream netStream = null;
    private byte[] Buffer = new byte[49152];
    private string receivedMessage = "";
    private int bytesReceived = 0;

    private InputManager iManager;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        iManager = GetComponent<InputManager>();
        localIP = LocalIPAddress();
    }

    private void OnDisable()
    {
        CloseClient();
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

    public void TryConnect()
    {
        connectButton.enabled = false;
        string id = int.Parse(controllerID.text).ToString();
        string ip = localIP.Split('.')[0] + "." + localIP.Split('.')[1] + "." + localIP.Split('.')[2] + "." + id;
        Connect(ip);
    }
    private void Connect(string ip)
    {
        Client = null;
        try
        {
            IPAddress ipAd = IPAddress.Parse(ip);
            Client = new TcpClient();
            Client.Connect(ipAd, port);
            TcpClient foundClient = Client;
            StartCoroutine(ListenServerMessages());
        }
        catch
        {
            connectButton.enabled = true;
            statusText.text = "Controller not found!";
            //connection failed
            return;
        }
    }

    private void CloseClient()
    {
        Debug.Log("A controller disconnected! ;-(");
        SendMessageToServer("Disconnected");

        if (Client.Connected)
            Client.Close();

        if (Client != null)
            Client = null;
    }

    private IEnumerator ListenServerMessages()
    {
        //early out if there is nothing connected       
        if (!Client.Connected)
            yield break;

        //Stablish Client NetworkStream information
        netStream = Client.GetStream();

        //Start Async Reading from Server and manage the response on MessageReceived function
        do
        {
            //Start Async Reading from Server and manage the response on MessageReceived function
            netStream.BeginRead(Buffer, 0, Buffer.Length, MessageReceived, null);

            if (bytesReceived > 0)
            {
                OnMessageReceived(receivedMessage);
                bytesReceived = 0;
            }
            yield return null;

        } while (bytesReceived >= 0 && netStream != null);
        //The communication is over
        //CloseClient();
    }
    private void MessageReceived(IAsyncResult result)
    {
        if (result.IsCompleted && Client.Connected)
        {
            //build message received from server
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

    //What to do with the received message fron controller
    protected virtual void OnMessageReceived(string receivedMessage)
    {
        if (receivedMessage == "I'm Connected!") ControllerConnected();
        else if (receivedMessage == "Disconnected") CloseClient();

        Debug.Log("Controller > " + receivedMessage);

        ManageData(receivedMessage);
    }


    private void ControllerConnected()
    {
        SceneManager.LoadSceneAsync("BlockUpHomeWorld");
    }

    private void ManageData(string data)
    {
        if (!data.Contains("/")) return;
        string[] splitData = data.Split('/');
        iManager.axis.x = Mathf.Clamp(float.Parse(splitData[0]), -1, 1);
        iManager.axis.y = Mathf.Clamp(float.Parse(splitData[1]), -1, 1);
        iManager.buttonAxis.x = Mathf.Clamp(float.Parse(splitData[2]), 0, 1);
        iManager.buttonAxis.y = Mathf.Clamp(float.Parse(splitData[3]), 0, 1);
        iManager.buttonAxis.z = Mathf.Clamp(float.Parse(splitData[4]), 0, 1);
        iManager.buttonAxis.w = Mathf.Clamp(float.Parse(splitData[5]), 0, 1);
    }
}
