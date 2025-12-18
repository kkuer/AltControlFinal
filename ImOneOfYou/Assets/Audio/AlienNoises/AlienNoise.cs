using UnityEngine;

[CreateAssetMenu(fileName = "AlienNoise", menuName = "Scriptable Objects/AlienNoise")]
public class AlienNoise : ScriptableObject
{
    public AudioClip clip;

    public float referencePitch;
    public float referenceSpeed;

    public float speedHighSO = 1.5f;
    public float speedLowSO = 0.2f;
    public float pitchHighSO = 2;
    public float pitchLowSO = 0.5f;

    public Color frequencyColor;
}
