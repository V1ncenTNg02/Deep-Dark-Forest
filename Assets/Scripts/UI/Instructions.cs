using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Instructions : MonoBehaviour
{
    public List<Sprite> instructions;
    public List<string> texts;
    private int i = 0;

    public void OnEnable()
    {
        GetComponent<Image>().sprite = instructions[i];
        transform.Find("Text").GetComponent<TextMeshProUGUI>().text = texts[i];
    }

    public void Next()
    {
        i = Math.Abs(i + 1) % instructions.Count;
        GetComponent<Image>().sprite = instructions[i];
        transform.Find("Text").GetComponent<TextMeshProUGUI>().text = texts[i];
    }

    public void Last()
    {
        i = Math.Abs(i - 1) % instructions.Count;
        GetComponent<Image>().sprite = instructions[i];
        transform.Find("Text").GetComponent<TextMeshProUGUI>().text = texts[i];
    }
}
