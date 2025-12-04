using UnityEngine;

public class AudioEffectsManager : MonoBehaviour
{
    public static AudioEffectsManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }
}
