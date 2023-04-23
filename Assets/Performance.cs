using Newtonsoft.Json;
using OVRSimpleJSON;
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
    public delegate void ApiResponseCallback(bool success, string msg);

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

            if ((source == null) || (target == null)) accuracy = 0.0;
            if ((source.Length == 0) || (target.Length == 0)) accuracy = 0.0;
            

            int stepsToSame = ComputeLevenshteinDistance(source, target);
            accuracy = (1.0 - ((double)stepsToSame / (double)Math.Max(source.Length, target.Length))) * 100;
            if (source == target)
            {
                accuracy = 100.0;
            }
            accuracyText.text = "Typing Accuracy: " + accuracy.ToString("0.00") + "%";
            
            //if (accuracy.ToString("0.00") == "0.00")
            //{
            //    accuracyText.text = "Typing Accuracy: " + "100%";
            //}
            //else
            //{
            //    accuracyText.text = "Typing Accuracy: " + accuracy.ToString("0.00") + "%";
            //}  
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
        //testResult test = new testResult
        //{
        //    sampleText = readText.text,
        //    userText = inputText.text,
        //    accuracy = accuracy.ToString("0.00"),
        //    timeTaken = timer.currentTime.ToString("0.00")
        //};
        testResult test = new testResult(readText.text, inputText.text, accuracy.ToString("0.00"), timer.currentTime.ToString("0.00"));

        //string jsonData =  JsonUtility.ToJson(test);
        string jsonData = test.JSONString();
        StartCoroutine(PostData(jsonData, RequestCallback));
    }

    void RequestCallback(bool success, string msg)
    {
        if (success)
        {
            uploadResultText.text = "Test Attempt Recorded Successfully!";
            StartCoroutine(WaitAndClear());
        }
        else
        {
            uploadResultText.text = "Fail - "+msg;
        }
    }

    IEnumerator WaitAndClear()
    {
        yield return new WaitForSeconds(5);
        uploadResultText.text = "";
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
            callback(false, (request.responseCode.ToString()+' '+request.result.ToString()+' '+request.error)); // Call the callback with a failure flag
        }
        else
        {
            callback(true, jsonData); // Call the callback with a success flag
        }
    }

    public class testResult
    {
        public string sampleText { get; set; }
        public string userText { get; set; }
        public string timeTaken { get; set; }
        public string accuracy { get; set; }

        public testResult()
        {
            this.sampleText = "val 1";
            this.userText = "val 2";
            this.timeTaken = "val 3";
            this.accuracy = "val 4";
        }

        public testResult(string sampleText, string usertext, string timeTaken, string accuracy)
        {
            this.sampleText = sampleText;
            this.userText  = usertext;
            this.timeTaken = timeTaken;
            this.accuracy = accuracy;
        }

        public string JSONString()
        {
            //return JsonUtility.ToJson(this);
            return JsonConvert.SerializeObject(this);
            //return string.Format("{\r\n  \"sampleText\": {0},\r\n  \"userText\": {1},\r\n  \"timeTaken\": {2},\r\n  \"accuracy\": {3}\r\n}", sampleText, userText, timeTaken, accuracy);
        }
    }
}
