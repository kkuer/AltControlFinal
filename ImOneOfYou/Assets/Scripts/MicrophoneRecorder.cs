using UnityEngine;

public class MicrophoneRecorder : MonoBehaviour
{
    private AudioClip recordedClip;
    [SerializeField] AudioSource audioSource;

    public void StartRecording()
    {
        string mic = Microphone.devices[0];
        int sampleRate = 44100;
        int lengthSec = 6;

        recordedClip = Microphone.Start(mic,false,lengthSec,sampleRate);
    }

    public void StopRecording()
    {
        Microphone.End(null);
    }

    public void PlayRecording()
    {
        audioSource.clip = recordedClip;
        audioSource.Play();
    }
}
