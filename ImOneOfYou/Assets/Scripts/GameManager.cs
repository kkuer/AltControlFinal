using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    const string glyphs = "azbagoobawagazxail";
    public int minCharAmount;
    public int maxCharAmount;
    public string exampleString;
    public TextMeshProUGUI targetText;
    public TextMeshProUGUI resultText;
    public TMP_InputField inputText;

    public AudioDrawer Audio1;
    public AudioDrawer Audio2;

    public float correctRange;

    public float waveMagnitude;

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
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    GenerateString();
        //}
        //if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        //{
        //    CheckString(inputText.text);
        //}
    }

    public void GenerateString()
    {
        exampleString = string.Empty;
        int charAmount = Random.Range(minCharAmount, maxCharAmount);
        for (int i = 0; i<charAmount; i++)
        {
            exampleString += glyphs[Random.Range(0, glyphs.Length)];
        }
        targetText.text = exampleString;
    }



    public void CheckString()
    {
        string inputString = inputText.text;
        if (inputString == exampleString)
        {
            resultText.text = "Zoogabazoo you got it let's kiss";
        }
        else
        {
            resultText.text = "GUARDS INCINERATE THIS MAN WITH COOL LASERS";
        }
    }

    public void CompareAudio()
    {
        waveMagnitude = Mathf.Abs(Audio1.waveformPercentage - Audio2.waveformPercentage);
        if (waveMagnitude < correctRange)
        {
            Debug.Log("Success!");
        }
        else
        {
            Debug.Log("Fail!");
        }
    }
}
