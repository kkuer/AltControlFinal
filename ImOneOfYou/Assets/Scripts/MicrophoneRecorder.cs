using System.Collections;
using UnityEngine;

public class MicrophoneRecorder : MonoBehaviour
{
    private AudioClip recordedClip;
    [SerializeField] AudioSource audioSource;
    public AudioDrawer audioDrawScript;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            PlayRecording();
        }
    }

    public IEnumerator startProcess()
    {
        StartRecording();
        yield return new WaitForSeconds(6);
        StopRecording();
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
