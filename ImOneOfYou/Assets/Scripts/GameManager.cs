using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    const string glyphs = "azbagoobawagazxail";

    public int minCharAmount;
    public int maxCharAmount;

    public int roundCount;
    public float suspicionPercent;

    public float speedHigh;
    public float speedLow;
    public float pitchHigh;
    public float pitchLow;

    public float pitchToMatch;
    public float speedToMatch;

    public AlienNoise alienSO;


    public AudioEffectsManager alienAudSource;
    //public string exampleString;

    //public TextMeshProUGUI targetText;
    //public TextMeshProUGUI resultText;
    public TextMeshProUGUI susText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI timerText;

    //public TMP_InputField inputText;  

    public AudioDrawer audioDrawerTarget;
    public AudioDrawer audioDrawerInput;

    public MicrophoneRecorder microphoneRecorderScript;

    public AudioSource targetAudio;
    public AudioSource inputAudio;

    public AudioClip selectedAudio;

    public float correctWaveRange;

    public float waveMagnitudeComp;
    public float waveSpeedComp;
    public float wavePitchComp;

    public List<AlienNoise> clipList = new List<AlienNoise>();
    public List<AlienNoise> usedClips = new List<AlienNoise>();

    public AudioEffectsManager audMan;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        timerText.enabled = false;
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        RandomizeTarget();
    }

    public IEnumerator startRecordin()
    {
        yield return new WaitForSeconds(targetAudio.clip.length + 1);
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
        //microphoneRecorderScript.StopRecording();
        timerText.enabled = false;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CompareAudio();
        }
        susText.text = $"Suspicion: {Mathf.RoundToInt(suspicionPercent)}%";
        roundText.text = $"Successful Social Interactions: {roundCount}. Good for you.";
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
        waveMagnitudeComp = Mathf.Abs(audioDrawerTarget.waveformPercentage - audioDrawerInput.waveformPercentage);
        wavePitchComp = Mathf.Abs(pitchToMatch - audMan.currentPitch);
        waveSpeedComp = Mathf.Abs(speedToMatch - audMan.currentSpeed);

        float howGood = (waveMagnitudeComp + wavePitchComp + waveSpeedComp) / 3;

        if (howGood < correctWaveRange)
        {
            suspicionPercent = suspicionPercent - (correctWaveRange - howGood);
            roundCount++;
            if (suspicionPercent < 0)
            {
                suspicionPercent = 0;
            }
            StartCoroutine(RandomSounds());
            Debug.Log("Success!");
        }
        else
        {
            StartCoroutine(RandomSounds());
            suspicionPercent = suspicionPercent + 20;
            Debug.Log("Fail!");
            if (suspicionPercent >= 100)
            {
                SceneManager.LoadScene(2);
            }
            else
            {
                roundCount++;
            }
        }
    }

    public void RandomizeTarget()
    {
        var availableClips = clipList.Except(usedClips).ToList();
        if (availableClips.Count == 0) { usedClips.Clear(); availableClips = clipList.ToList(); }

        AlienNoise newNoise = availableClips[Random.Range(0, clipList.Count)];


        usedClips.Add(newNoise);

        pitchToMatch = Random.Range(newNoise.pitchLowSO, newNoise.pitchHighSO);
        speedToMatch = Random.Range(newNoise.speedLowSO, newNoise.speedHighSO);

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

    public IEnumerator RandomSounds()
    {
        yield return new WaitForSeconds(1);
        RandomizeTarget();  
    }

}
