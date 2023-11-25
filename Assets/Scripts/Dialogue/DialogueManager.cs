using System;
using ORZ;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Text.RegularExpressions;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI DialogueText;
    public GameObject DialogueBox; 
    // public static bool isPaused = false;
    public float textSpeed = 0.1f;
    private string currentSentence;
    private bool controllable = true;

    public bool playingDialogue = false;

    public struct StringComplex
    {
        public string s;
        public bool hasColor;
        public string color;

        public StringComplex(string s, bool hasColor = false, string color = null)
        {
            this.s = s;
            this.hasColor = hasColor;
            this.color = color;
        }

        public override string ToString()
        {
            return $"s: {s}, hasColor: {hasColor}, color: {color}";
        }
    }


    void Start()
    {
        sentences = new Queue<string>();
    }

    void Update()
    {
        if (!controllable) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence();
        }
    }
    

    public IEnumerator StartDialogue(Dialogue dialogue)
    {
        playingDialogue = true;
        sentences.Clear();
        Name.text = dialogue.name;
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        ObjectGetter.globalEvent.GetComponent<LetterBoxController>().BlackSideFadeIn();
        controllable = false;
        yield return new WaitWhile(() => ObjectGetter.globalEvent.GetComponent<LetterBoxController>().T < 0.95f);

        DialogueBox.SetActive(true);
        controllable = true;

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {

        if (currentSentence != null && DialogueText.text.Length < currentSentence.Length)
        {
            StopAllCoroutines();
            DialogueText.text = currentSentence;
            return;
        }

        if (sentences.Count == 0)
        {
            StartCoroutine(EndDialogue());
            return;
        }

        DialogueText.text = "";
        string sentence = sentences.Dequeue();
        currentSentence = sentence;
        StartCoroutine(TypeSentence(sentence));
    }
    
    IEnumerator TypeSentence(string sentence)
    {
        List<StringComplex> complexStrings = ParseStringToComplexList(sentence);

        foreach (StringComplex sc in complexStrings)
        {
            var oldString = DialogueText.text;
            var colorPrefix = string.Empty;
            var colorSuffix = string.Empty;
            var addString = string.Empty;
            if (sc.hasColor)
            {
                colorPrefix = $"<color={sc.color}>";
                colorSuffix = "</color>";
            }
            foreach (var c in sc.s)
            {
                addString += c;
                DialogueText.text = oldString + colorPrefix + addString + colorSuffix;
                yield return new WaitForSeconds(textSpeed);
            }
        }
    }

    public List<StringComplex> ParseStringToComplexList(string input)
    {
        List<StringComplex> resultList = new List<StringComplex>();
        string pattern = @"<color=(.*?)>(.*?)<\/color>";

        MatchCollection matches = Regex.Matches(input, pattern);

        int prevIndex = 0;
        foreach (Match match in matches)
        {
            // If there has more text before color tag, add complexString without color tag
            if (match.Index > prevIndex)
            {
                resultList.Add(new StringComplex(input.Substring(prevIndex, match.Index - prevIndex)));
            }

            // Add the StringComplex with color tag
            string colorValue = match.Groups[1].Value;
            string content = match.Groups[2].Value;
            resultList.Add(new StringComplex(content, true, colorValue));

            prevIndex = match.Index + match.Length;
        }

        // If there has more text after the last color tag, add complexString without color tag
        if (prevIndex < input.Length)
        {
            resultList.Add(new StringComplex(input.Substring(prevIndex)));
        }

        return resultList;
    }

    IEnumerator EndDialogue()
    {
        ObjectGetter.globalEvent.GetComponent<LetterBoxController>().BlackSideFadeOut();
        controllable = false;
        sentences.Clear();
        DialogueBox.SetActive(false);
        yield return new WaitUntil(() => ObjectGetter.globalEvent.GetComponent<LetterBoxController>().T <= 0);
        playingDialogue = false;
    }
}
