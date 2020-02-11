using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This Server inheritated class acts like Server using UI elements like buttons and input fields.
/// </summary>
public class CustomServer : Server
{

    //Set UI interactable properties
    protected virtual void Awake()
    {
        StartServer();
    }

    //Start server and wait for clients
    protected override void StartServer()
    {
        base.StartServer();
    }

    //Get input field text and send it to client

    //Close connection with the client
    protected override void CloseClientConnection()
    {
        base.CloseClientConnection();
    }

    //Close client connection and disables the server
    protected override void CloseServer()
    {
        base.CloseServer();
    }

    //Custom Server Log
    #region ServerLog
    //With Text Color
    protected override void ServerLog(string msg)
    {
        base.ServerLog(msg);
        print(msg);
    }
    //Without Text Color
    protected override void ServerLog(string msg, Color color)
    {
        base.ServerLog(msg, color);
        print(msg);
    }
    #endregion
}