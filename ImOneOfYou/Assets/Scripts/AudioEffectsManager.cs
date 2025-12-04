using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioEffectsManager : MonoBehaviour
{
    public static AudioEffectsManager Instance { get; private set; }

    private AudioSource source;

    [SerializeField] AudioMixer mixer;

    public AudioClip currentAudioClip;
    public List<AudioClip> clipList = new List<AudioClip>();

    //TESTING INPUTS
    [SerializeField] private float currentSpeed;
    [SerializeField] private float currentPitch;


    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }

        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) //SPEED DOWN
        {
            currentSpeed -= 0.1f;
            AdjustClipSpeed(currentSpeed);
        }

        if (Input.GetKeyDown(KeyCode.I)) //SPEED UP
        {
            currentSpeed += 0.1f;
            AdjustClipSpeed(currentSpeed);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            currentPitch -= 0.1f;
            AdjustClipPitch(currentPitch);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            currentPitch += 0.1f;
            AdjustClipPitch(currentPitch);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (source.clip != null)
            {
                source.Play();
                Debug.Log("Sound played");
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            ResetClipSpeed();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            ResetClipPitch();
        }
    }

    public void AdjustClipPitch(float value) //keep this between min 0.5 and 2.0 max
    {
        mixer.SetFloat("Pitch", value);
    }

    public void ResetClipPitch()
    {
        mixer.SetFloat("Pitch", 1f);
    }

    public void AdjustClipSpeed(float speed) //use percentage decimals (0.8 = 80% speed, 1.2 = 120% speed, etc)
    {
        float currentPitch;
        if (mixer.GetFloat("Pitch", out currentPitch))
        {
            source.pitch = speed;
            mixer.SetFloat("Pitch", currentPitch / speed);
        }
    }

    public void ResetClipSpeed()
    {
        source.pitch = 1;
    }
}
