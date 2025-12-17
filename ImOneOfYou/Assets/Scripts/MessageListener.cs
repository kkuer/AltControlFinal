using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MessageListener : MonoBehaviour
{
    public float LastPitch { get; private set; }
    public float LastSpeed { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Invoked when a line of data is received from the serial device.
    void OnMessageArrived(string msg)
    {
        //Debug.Log("Arrived: " + msg);

        if (msg.Contains("Pitch"))
        {
            string numString = msg.Substring(msg.IndexOf("Pitch: ") + "Pitch: ".Length);
            LastPitch = float.Parse(numString);

            Debug.Log("Pitch: " + numString);
        }
        else if (msg.Contains("Speed"))
        {
            string numString = msg.Substring(msg.IndexOf("Speed: ") + "Speed: ".Length);
            LastSpeed = float.Parse(numString);

            Debug.Log("Speed: " + numString);
        }
    }
    // Invoked when a connect/disconnect event occurs. The parameter 'success'
    // will be 'true' upon connection, and 'false' upon disconnection or
    // failure to connect.
    void OnConnectionEvent(bool success)
    {
        Debug.Log(success ? "Device connected" : "Device disconnected");
    }
}
