using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;
using static UnityEngine.GraphicsBuffer;


public class Performance : MonoBehaviour
{
    [Header("Text Input Components")]
    public InputField readText;
    public InputField inputText;
    public Timer timer;

    [Header("Output Component")]
    public TextMeshProUGUI accuracyText;
    public TextMeshProUGUI uploadResultText;

    public double accuracy;

    public string apiEndpoint;
    public delegate void ApiResponseCallback(bool success);

    // Start is called before the first frame update
    void Start()
    {
        accuracy = 0;
        inputText.text = "";
    }

    private void OnEnable()
    {
        Start();
    }


    public void ToggleTest()
    {
        this.enabled = !this.enabled;
    }

    // Update is called once per frame
    void Update()
    {
        if ((readText.text != "") || (inputText.text != ""))
        {
            string target = inputText.text;
            string source = readText.text.Substring(0, target.Length);

            if ((source == null) || (target == null)) accuracy = 0.0f;
            if ((source.Length == 0) || (target.Length == 0)) accuracy = 0.0f;
            if (source == target) accuracy = 1.0f;

            int stepsToSame = ComputeLevenshteinDistance(source, target);
            accuracy = (1.0 - ((double)stepsToSame / (double)Math.Max(source.Length, target.Length))) * 100;
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

    public void SubmitText()
    {
        testResult test = new testResult();

        test.sampleText = readText.text;
        test.userText   = inputText.text;
        test.accuracy = accuracy.ToString("0.00");
        //test.timeTaken  = 

        string jsonData =  JsonUtility.ToJson(test);
        StartCoroutine(PostData(jsonData, RequestCallback));
    }

    void RequestCallback(bool success)
    {
        if (success)
        {
            uploadResultText.text = "Test Attempt Recorded Successfully!";
        }
        else
        {
            uploadResultText.text = "Test Attempt Upload Failed!";
        }
    }

    IEnumerator PostData(string jsonData, ApiResponseCallback callback)
    {
        // Create a new UnityWebRequest object
        UnityWebRequest request = new UnityWebRequest(apiEndpoint, "POST");

        // Set the content type of the request to application/json
        request.SetRequestHeader("Content-Type", "application/json");

        // Convert the JSON data to a byte array and set it as the request body
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);

        // Send the request
        yield return request.SendWebRequest();

        // Check for errors
        if (request.result != UnityWebRequest.Result.Success)
        {
            callback(false); // Call the callback with a failure flag
        }
        else
        {
            callback(true); // Call the callback with a success flag
        }
    }

    [Serializable]
    private class testResult
    {
        public string sampleText { get; set; }
        public string userText { get; set; }
        public string timeTaken { get; set; }
        public string accuracy { get; set; }
    }
}
