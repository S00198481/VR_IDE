using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;
using static UnityEngine.GraphicsBuffer;

public class Performance : MonoBehaviour
{
    [Header("Text Input Components")]
    public InputField readText;
    public InputField inputText;

    [Header("Output Component")]
    public TextMeshProUGUI accuracyText;

    public float accuracy;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if ((readText.text != "") || (inputText.text != ""))
        {
            string source = readText.text;
            string target = inputText.text;
            if ((source == null) || (target == null)) accuracy = 0.0f;
            if ((source.Length == 0) || (target.Length == 0)) accuracy = 0.0f;
            if (source == target) accuracy = 1.0f;

            int stepsToSame = ComputeLevenshteinDistance(source, target);
            accuracy = (100.0f - ((float)stepsToSame / (float)Math.Max(source.Length, target.Length)));
            accuracyText.text = "Typing Accuracy: " + accuracy.ToString("0.00") + "%";
        }
    }

    private int ComputeLevenshteinDistance(string source, string target)
    {
        if ((source == null) || (target == null)) return 0;
        if ((source.Length == 0) || (target.Length == 0)) return 0;
        if (source == target) return source.Length;

        int sourceWordCount = source.Length;
        int targetWordCount = target.Length;

        // Step 1
        if (sourceWordCount == 0)
            return targetWordCount;

        if (targetWordCount == 0)
            return sourceWordCount;

        int[,] distance = new int[sourceWordCount + 1, targetWordCount + 1];

        // Step 2
        for (int i = 0; i <= sourceWordCount; distance[i, 0] = i++) ;
        for (int j = 0; j <= targetWordCount; distance[0, j] = j++) ;

        for (int i = 1; i <= sourceWordCount; i++)
        {
            for (int j = 1; j <= targetWordCount; j++)
            {
                // Step 3
                int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;

                // Step 4
                distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
            }
        }
        return distance[sourceWordCount, targetWordCount];
    }
}
