using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SuspicionMeterUI : MonoBehaviour
{
    private Slider s;
    public TMP_Text amount;
    public Image fill;

    void Start()
    {
        s = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.suspicionPercent <= 30)
        {
            fill.color = Color.green;
        }
        else if (GameManager.Instance.suspicionPercent > 30 && GameManager.Instance.suspicionPercent <= 70)
        {
            fill.color = Color.yellow;
        }
        else
        {
            fill.color = Color.red;
        }

        s.value = GameManager.Instance.suspicionPercent / 100f;
        amount.text = GameManager.Instance.suspicionPercent.ToString("0") + "%";
    }
}
