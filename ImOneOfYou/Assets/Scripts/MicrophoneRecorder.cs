using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneRecorder : MonoBehaviour
{
    public bool isDrawing;
    public int sampleWindow = 64;
    public float audLoud;
    public float arrowoffsetx;
    public float moveForwards;
    public float forwardsCoefficient;
    public GameObject drawingCursor;
    public GameObject cursorCopy;
    public GameObject cursorCopyInst;
    public List<GameObject> cursorCopyList = new List<GameObject>();
    private AudioClip recordedClip;
    [SerializeField] AudioSource audioSource;
    public AudioDrawer audioDrawScript;

    public void Start()
    {
        drawingCursor.transform.position = new Vector3(arrowoffsetx,2.96f,-3.05f);
    }

    public void Update()
    {
        if (isDrawing)
        {
            cursorCopyInst = Instantiate(cursorCopy, drawingCursor.transform.position, Quaternion.identity);
            cursorCopyInst.transform.localScale = drawingCursor.transform.localScale;
            cursorCopyList.Add(cursorCopyInst);
            moveForwards = moveForwards+(Time.deltaTime * forwardsCoefficient);
            drawingCursor.transform.position = new Vector3(transform.position.x+(moveForwards+Time.deltaTime),2.96f,transform.position.z);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            startFunction();
        }
        //if (recordedClip != null)
        //{
        //    recordedClip.GetData(waveData, micPosition);
        //}
    }

    public float GetLoudnessFromMicrophone()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]),recordedClip);
    }

    public float GetLoudnessFromAudioClip(int clipPos, AudioClip clip)
    {
        int startPos = clipPos - sampleWindow;
        if (startPos < 0)
        {
            return 0;
        }
        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPos);

        //compute loudness 
        float totalLoud = 0;
        for (int i = 0; i <sampleWindow; i++)
        {
            totalLoud += Mathf.Abs(waveData[i]);
        }
        return totalLoud/sampleWindow;
    }

    public IEnumerator startProcess()
    {
        if (cursorCopyList.Count > 0)
        {
            for (int i = 0; i < cursorCopyList.Count; i++)
            {
                Destroy(cursorCopyList[i]);
            }
        }
        StartRecording();
        isDrawing = true;
        yield return new WaitForSeconds(6);
        StopRecording();
        isDrawing = false;
        PlayRecording();
    }

    public void startFunction()
    {
        StartCoroutine(startProcess());
    }
    public void StartRecording()
    {
        string mic = Microphone.devices[0];
        int sampleRate = 44100;
        int lengthSec = 6;

        recordedClip = Microphone.Start(mic,false,lengthSec,sampleRate);
    }

    public void StopRecording()
    {
        if (cursorCopyList.Count > 0)
        {
            for (int i = 0; i < cursorCopyList.Count; i++)
            {
                Destroy(cursorCopyList[i]);
            }
        }
        drawingCursor.transform.position = new Vector3(arrowoffsetx, 2.96f, -3.05f);
        moveForwards = 0;
        audioDrawScript.drawNow = true;
        audioDrawScript.DrawWaveform();
        Microphone.End(null);
    }

    public void PlayRecording()
    {
        audioDrawScript.drawNow = true;
        audioDrawScript.DrawWaveform();
        audioSource.clip = recordedClip;
        audioSource.Play();
    }
}
