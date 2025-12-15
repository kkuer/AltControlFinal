using UnityEngine;

[CreateAssetMenu(fileName = "AlienNoise", menuName = "Scriptable Objects/AlienNoise")]
public class AlienNoise : ScriptableObject
{
    public AudioClip clip;

    public float referencePitch;
    public float referenceSpeed;

    public Color frequencyColor;
}
