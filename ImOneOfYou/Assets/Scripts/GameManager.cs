using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;
//using UnityEditor.U2D;
//using UnityEditor.Build;
using UnityEngine.UI;
using Unity.Collections;

public class GameManager : MonoBehaviour
{
    const string glyphs = "azbagoobawagazxail";

    public int minCharAmount;
    public int maxCharAmount;

    public int roundCount;
    public float suspicionPercent;
    public float suspicionCoefficient;

    public float speedHigh;
    public float speedLow;
    public float pitchHigh;
    public float pitchLow;

    public float pitchToMatch;
    public float speedToMatch;

    public AlienNoise alienSO;

    public Image refColor;
    public Image yourColor;

    public SpriteRenderer alienPicture;

    public List<float> dataListOriginal = new List<float>();
    public List<float> dataListRecording = new List<float>();
    public float dataListOriginalSum;
    public float dataListRecordingSum;
    public float suspicionMaxReduction;

    public List<Sprite> sprites = new List<Sprite>();
    public List<Color> colorFreqs = new List<Color>();

    public Color currentColor;


    public AudioEffectsManager alienAudSource;
    //public string exampleString;

    //public TextMeshProUGUI targetText;
    //public TextMeshProUGUI resultText;
    public TextMeshProUGUI susText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI timerText;

    public enum Recording { Recording, Not};
    public Recording recordStatus = Recording.Recording;

    //public TMP_InputField inputText;  

    public AudioDrawer audioDrawerTarget;
    public AudioDrawer audioDrawerInput;

    public MicrophoneRecorder microphoneRecorderScript;

    public AudioSource targetAudio;
    public AudioSource inputAudio;

    public AudioClip selectedAudio;
    public AudioClip recordedAudio;

    public float correctWaveRange;

    public float waveMagnitudeComp;
    public float waveSpeedComp;
    public float wavePitchComp;

    public List<AlienNoise> clipList = new List<AlienNoise>();
    public List<AlienNoise> usedClips = new List<AlienNoise>();

    public RecordData dataScriptOriginal;
    public RecordData dataScriptRecording;


    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        Debug.Log("AWAKE");
        recordStatus = Recording.Recording;
    }

    private void Start()
    {
        RandomizeTarget();
        currentColor = colorFreqs[0];
        ChangeColorFreq();
        timerText.enabled = false;
        Debug.Log("Start");
    }

    public IEnumerator startRecordin()
    {
        dataScriptRecording.ClearList();
        yield return new WaitForSeconds(targetAudio.clip.length);
        timerText.enabled = true;
        timerText.text = "3";
        yield return new WaitForSeconds(1);
        timerText.text = "2";
        yield return new WaitForSeconds(1);
        timerText.text = "1";
        yield return new WaitForSeconds(1);
        timerText.text = "GO!";
        microphoneRecorderScript.startFunction();
        yield return new WaitForSeconds(targetAudio.clip.length);
        microphoneRecorderScript.StopRecording();
        recordStatus = Recording.Not;
        timerText.enabled = false;
    }

    public IEnumerator secondRecordin()
    {
        dataScriptRecording.ClearList();
        timerText.enabled = true;
        timerText.text = "3";
        yield return new WaitForSeconds(1);
        timerText.text = "2";
        yield return new WaitForSeconds(1);
        timerText.text = "1";
        yield return new WaitForSeconds(1);
        timerText.text = "GO!";
        microphoneRecorderScript.startFunction();
        yield return new WaitForSeconds(targetAudio.clip.length);
        microphoneRecorderScript.StopRecording();
        timerText.enabled = false;
        microphoneRecorderScript.audVisMan.BeatToggleOff();
    }

    public void Update()
    {
        if (recordStatus == Recording.Not)
        {
            suspicionPercent = suspicionPercent + (suspicionCoefficient * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CompareAudio();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeColorFreq();
        }
        susText.text = $"Suspicion: {Mathf.RoundToInt(suspicionPercent)}%";
        roundText.text = $"Successful Social Interactions: {roundCount}. Good for you.";
        
        if (suspicionPercent >100)
        {
            SceneManager.LoadScene(2);
        }

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    GenerateString();
        //}
        //if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        //{
        //    CheckString(inputText.text);
        //}
    }

    //public void GenerateString()
    //{
    //    exampleString = string.Empty;
    //    int charAmount = Random.Range(minCharAmount, maxCharAmount);
    //    for (int i = 0; i<charAmount; i++)
    //    {
    //        exampleString += glyphs[Random.Range(0, glyphs.Length)];
    //    }
    //    targetText.text = exampleString;
    //}



    //public void CheckString()
    //{
    //    string inputString = inputText.text;
    //    if (inputString == exampleString)
    //    {
    //        resultText.text = "Zoogabazoo you got it let's kiss";
    //    }
    //    else
    //    {
    //        resultText.text = "GUARDS INCINERATE THIS MAN WITH COOL LASERS";
    //    }
    //}

    public void CompareAudio()
    {
        var numSamplesRecorded = inputAudio.clip.samples;
        var samplesRecorded = new NativeArray<float>(numSamplesRecorded, Allocator.Temp);

        inputAudio.clip.GetData(samplesRecorded, 0);
        for (int i = 0; i < samplesRecorded.Length; i++)
        {
            dataListRecording.Add(samplesRecorded[i]);
            dataListRecordingSum = dataListRecordingSum + samplesRecorded[i];
        }
        var numSamplesTarget = targetAudio.clip.samples;
        var samplesTarget = new NativeArray<float>(numSamplesTarget, Allocator.Temp);

        targetAudio.clip.GetData(samplesTarget, 0);
        for (int i = 0; i < samplesTarget.Length; i++)
        {
            dataListOriginal.Add(samplesTarget[i]);
            dataListOriginalSum = dataListOriginalSum + samplesTarget[i];
        }
        if ((Mathf.Abs(suspicionMaxReduction - Mathf.Abs(dataListRecordingSum - dataListOriginalSum))) > 20)
        {
            suspicionPercent = suspicionPercent - Random.Range(5, 10);
        }
        else
        {
            suspicionPercent = suspicionPercent - (Mathf.Abs(suspicionMaxReduction - Mathf.Abs(dataListRecordingSum - dataListOriginalSum)));
        }
        if (suspicionPercent < 0)
        {
            suspicionPercent = 0;
        }

        StartCoroutine(RandomSounds());
        //dataScriptOriginal.ClearList();
        //dataScriptRecording.ClearList();
    }

    public void RandomizeTarget()
    {
        var availableClips = clipList.Except(usedClips).ToList();
        if (availableClips.Count == 0)
        {
            usedClips.Clear();
            availableClips = clipList.ToList();
        }

        AlienNoise newNoise = availableClips[Random.Range(0, availableClips.Count)];

        alienPicture.sprite = sprites[Random.Range(0, sprites.Count)];
        //Color c = new Color(Random.Range(100, 200), Random.Range(100, 200), Random.Range(100, 200), 255);
        //alienPicture.color = c;

        usedClips.Add(newNoise);

        pitchToMatch = Random.Range(newNoise.pitchLowSO, newNoise.pitchHighSO);
        speedToMatch = Random.Range(newNoise.speedLowSO, newNoise.speedHighSO);

        alienAudSource.AdjustClipSpeed(speedToMatch);
        alienAudSource.AdjustClipPitch(pitchToMatch);

        refColor.color = newNoise.frequencyColor;

        selectedAudio = newNoise.clip;
        alienSO = newNoise;

        targetAudio.clip = selectedAudio;

        targetAudio.GetComponent<AudioEffectsManager>().currentSpeed = speedToMatch;
        targetAudio.GetComponent<AudioEffectsManager>().currentPitch = pitchToMatch;

        targetAudio.Play();

        audioDrawerTarget.drawNow = true;
        StartCoroutine(startRecordin());
        //audioDrawerInput.GetWaveform();
        //audioDrawerInput.DrawWaveform();
    }

    public void ChangeColorFreq()
    {
        StartCoroutine(secondRecordin());
        //bool found = false;

        //foreach (Color c in colorFreqs)
        //{
        //    if (!found && c != null && c == currentColor)
        //    {
        //        int pos = colorFreqs.IndexOf(c);
        //        found = true;
        //        Debug.Log(pos);
        //        if (pos >= colorFreqs.Count - 1)
        //        {
        //            currentColor = colorFreqs[0];
        //        }
        //        else
        //        {
        //            currentColor = colorFreqs[pos + 1];
        //        }
        //    }
        //}

        //yourColor.color = currentColor;
    }

    public IEnumerator RandomSounds()
    {
        yield return new WaitForSeconds(1);
        RandomizeTarget();  
    }

}
