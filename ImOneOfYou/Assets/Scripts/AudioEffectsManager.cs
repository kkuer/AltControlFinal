using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

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

    [SerializeField] private Keyboard k;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }

        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        k = Keyboard.current;
    }

    private void Update()
    {
        if (k.uKey.wasPressedThisFrame) //SPEED DOWN
        {
            if (currentSpeed <= 0.5f)
            {

            }
            else
            {
                currentSpeed -= 0.1f;
            }

            AdjustClipSpeed(currentSpeed);
        }

        if (k.iKey.wasPressedThisFrame) //SPEED UP
        {
            if (currentSpeed >= 2.0f)
            {

            }
            else
            {
                currentSpeed += 0.1f;
            }

            AdjustClipSpeed(currentSpeed);
        }

        if (k.oKey.wasPressedThisFrame)
        {
            if (currentPitch <= 0.5f)
            {

            }
            else
            {
                currentPitch -= 0.1f;
            }
            AdjustClipPitch(currentPitch);
        }

        if (k.pKey.wasPressedThisFrame)
        {
            if (currentPitch >= 2.0f)
            {

            }
            else
            {
                currentPitch += 0.1f;
            }
            
            AdjustClipPitch(currentPitch);
        }

        if (k.spaceKey.wasPressedThisFrame)
        {
            if (source.clip != null)
            {
                source.Play();
                Debug.Log("Sound played");
            }
        }

        if (k.wKey.wasPressedThisFrame)
        {
            ResetClipSpeed();
        }

        if (k.sKey.wasPressedThisFrame)
        {
            ResetClipPitch();
        }
    }

    public void AdjustClipPitch(float value) //keep this between min 0.5 and 2.0 max
    {
        mixer.SetFloat("Pitch", 1f * value);
    }

    public void ResetClipPitch()
    {
        currentPitch = 1f;
        mixer.SetFloat("Pitch", 1f);
    }

    public void AdjustClipSpeed(float speed) //use percentage decimals (0.8 = 80% speed, 1.2 = 120% speed, etc)
    {
        float currentPitch;
        if (mixer.GetFloat("Pitch", out currentPitch))
        {
            source.pitch = speed;
            mixer.SetFloat("Pitch", 1f / speed);
        }
    }

    public void ResetClipSpeed()
    {
        currentSpeed = 1f;
        source.pitch = 1;
    }
}
