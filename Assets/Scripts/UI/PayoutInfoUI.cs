using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PayoutInfoUI : MonoBehaviour
{
    public TextMeshProUGUI text;
    public PayoutLinesContainer payoutLinesContainer;

    private void Start()
    {
        for (int i = 0; i < payoutLinesContainer.payoutLines.Count; i++)
        {
            int[] line = payoutLinesContainer.payoutLines[i].line;
            text.text += "Line " + (i + 1) + ": " + line[0] + "," + line[1] + "," + line[2] + "," + line[3] + "," + line[4] + "," + line[0] + "\n";
        }
    }

    public void Click()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
