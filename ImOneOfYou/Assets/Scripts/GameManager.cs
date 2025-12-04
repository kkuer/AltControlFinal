using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    const string glyphs = "azbagoobawagazxail";
    public int minCharAmount;
    public int maxCharAmount;
    public string exampleString;
    public TextMeshProUGUI targetText;
    public TMP_InputField inputText;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GenerateString();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            CheckString(inputText.text);
        }
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

    public void CheckString(string inputString)
    {
        if (inputString == exampleString)
        {
            Debug.Log("Zoogabazoo you got it let's kiss");
        }
        else
        {
            Debug.Log("GUARDS INCINERATE THIS MAN WITH COOL LASERS");
        }
    }
}
