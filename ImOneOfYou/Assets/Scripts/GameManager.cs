using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    const string glyphs = "azbagoobawagazxail";

    public int minCharAmount;
    public int maxCharAmount;

    public int roundCount;
    public float suspicionPercent;

    //public string exampleString;

    //public TextMeshProUGUI targetText;
    //public TextMeshProUGUI resultText;
    public TextMeshProUGUI susText;
    public TextMeshProUGUI roundText;

    //public TMP_InputField inputText;  

    public AudioDrawer audioDrawerTarget;
    public AudioDrawer audioDrawerInput;

    public AudioSource targetAudio;
    public AudioSource inputAudio;

    public AudioClip selectedAudio;

    public float correctRange;

    public float waveMagnitude;

    public List<AudioClip> clipList = new List<AudioClip>();

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
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
        waveMagnitude = Mathf.Abs(audioDrawerTarget.waveformPercentage - audioDrawerInput.waveformPercentage);
        if (waveMagnitude < correctRange)
        {
            suspicionPercent = suspicionPercent - (correctRange - waveMagnitude);
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
        selectedAudio = clipList[Random.Range(0, clipList.Count)];
        targetAudio.clip = selectedAudio;
        targetAudio.Play();
        audioDrawerTarget.drawNow = true;
        //audioDrawerInput.GetWaveform();
        //audioDrawerInput.DrawWaveform();
    }

    public IEnumerator RandomSounds()
    {
        yield return new WaitForSeconds(1);
        RandomizeTarget();  
    }

}
