using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This Client inheritated class acts like Client using UI elements like buttons and input fields.
/// </summary>
public class CustomClient : Client
{
    public Text t;

    //Set UI interactable properties
    private void Awake()
    {
        base.StartClient();
    }

    private void OnDisable()
    {
        SendCloseToServer();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
         SendMessageToServer();
    }

    private void SendMessageToServer()
    {
        string newMsg = "Test message ;0 "+ Time.time;
        base.SendMessageToServer(newMsg);
    }

    private void SendCloseToServer()
    {
        base.SendMessageToServer("Close");
    }

    //Custom Client Log
    #region ClientLog
    protected override void ClientLog(string msg, Color color)
    {
        base.ClientLog(msg, color);
        t.text += '\n' + "<color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">- " + msg + "</color>";
    }
    protected override void ClientLog(string msg)
    {
        base.ClientLog(msg);
        t.text += '\n' + "- " + msg;
    }
    #endregion
}