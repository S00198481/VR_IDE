using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardToggle : MonoBehaviour
{
    [Header("Keyboard")]
    public OVRTrackedKeyboard trackedKeyboard;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleKeyboardActive()
    {
        trackedKeyboard.ShowUntracked = !trackedKeyboard.ShowUntracked;
        trackedKeyboard.TrackingEnabled = !trackedKeyboard.TrackingEnabled;
    }
}
