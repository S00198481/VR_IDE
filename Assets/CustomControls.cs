using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomControls : MonoBehaviour
{
    public OVRTrackedKeyboard trackedKeyboard;
    public InputField StartingFocusField;



    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(StartingFocusField.gameObject, null);
        StartingFocusField.OnPointerClick(new PointerEventData(EventSystem.current));
        StartingFocusField.ActivateInputField();
        //StartCoroutine(SelectInputField());
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
        EventSystem.current.SetSelectedGameObject(StartingFocusField.gameObject, null);
        StartingFocusField.OnPointerClick(new PointerEventData(EventSystem.current));
        StartingFocusField.ActivateInputField();
        //StartingFocusField.lineType = InputField.LineType.MultiLineNewline;
        //StartingFocusField.Select();
        //StartingFocusField.ActivateInputField();
    }

    void OnGUI()
    {
        string key = "";
        bool keydown = false;
        Event e = Event.current;
        if (e.type.Equals(EventType.KeyDown) && !keydown)
        {
            keydown = true;
            key = e.keyCode.ToString();
            StartingFocusField.text = StartingFocusField.text + key;
        }

        if (e.type.Equals(EventType.KeyUp))
            keydown = false;
    }
}
