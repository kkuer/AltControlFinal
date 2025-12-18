using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MessageListener : MonoBehaviour
{
    public float LastPitch { get; private set; }
    public float LastSpeed { get; private set; }

    [SerializeField] AudioEffectsManager m_EffectsManager;

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

            float roundedLP = LastPitch / 1023; // normalize pitch
            roundedLP = Mathf.Round(roundedLP * 100) / 10; // round to 2 decimal places
            // min should be 0.5f, max should be 2
            float newPitch = Mathf.Lerp(0.5f, 2, roundedLP);

            m_EffectsManager.AdjustClipPitch(newPitch);

            Debug.Log("Pitch: " + newPitch.ToString());
        }
        else if (msg.Contains("Speed"))
        {
            string numString = msg.Substring(msg.IndexOf("Speed: ") + "Speed: ".Length);
            LastSpeed = float.Parse(numString);

            float roundedLS = LastSpeed / 1023; // normalize speed
            roundedLS = Mathf.Round(roundedLS * 100) / 10; // round to 2 decimal places
            float newSpeed = Mathf.Lerp(0.25f, 2, roundedLS);

            m_EffectsManager.AdjustClipSpeed(newSpeed);

            Debug.Log("Speed: " + newSpeed.ToString());
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
