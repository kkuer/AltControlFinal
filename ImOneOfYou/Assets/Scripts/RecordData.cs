using UnityEngine;
using System.Collections.Generic;

public class RecordData : MonoBehaviour
{
    public List<float> audioData = new List<float>();
    public AudioVisualizeManager avmScript;
    public float prevValue;
    public float newValue;

    public void Start()
    {
        prevValue = 999;
    }

    public void Update()
    {
        if (avmScript.Output != prevValue)
        {
            newValue = avmScript.Output;
            audioData.Add(newValue);
            prevValue = avmScript.Output;
        }
    }

}
