using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CustomControls : MonoBehaviour
{
    public OVRTrackedKeyboard trackedKeyboard;
    public InputField StartingFocusField;

    void OnEnable()
    {
        StartCoroutine(SelectInputField());
    }

    IEnumerator SelectInputField()
    {
        yield return new WaitForEndOfFrame();
        StartingFocusField.ActivateInputField();
    }

    void Start()
    {
        trackedKeyboard.TrackingEnabled = true;
        trackedKeyboard.ConnectionRequired = true;
        trackedKeyboard.RemoteKeyboard = true;
        StartingFocusField.lineType = InputField.LineType.MultiLineNewline;
        StartingFocusField.Select();
        StartingFocusField.ActivateInputField();
    }

    void Update()
    {

    }
}
