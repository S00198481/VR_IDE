using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Component")]
    public TextMeshProUGUI timerUi;

    [Header("Timer Settings")]
    public float currentTime;
    public bool countDown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if countdown is true, remove time, if false, add time
        currentTime = countDown ? currentTime -= Time.deltaTime : currentTime += Time.deltaTime;
        timerUi.text = "Current Time: "+currentTime.ToString("0.00");
    }
}
