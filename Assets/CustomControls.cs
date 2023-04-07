using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CustomControls : MonoBehaviour
{
    public OVRTrackedKeyboard trackedKeyboard;
    public InputField StartingFocusField;
    public OVRInput.RawButton keyboardButton;


    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(StartingFocusField.gameObject, null);
        StartingFocusField.OnPointerClick(new PointerEventData(EventSystem.current));
        StartingFocusField.ActivateInputField();
        StartCoroutine(SelectInputField());
    }

    IEnumerator SelectInputField()
    {
        yield return new WaitForEndOfFrame();
        StartingFocusField.ActivateInputField();
    }

    //private void Start()
    //{
    //    trackedKeyboard.TrackingEnabled = true;
    //    trackedKeyboard.ConnectionRequired = true;
    //    trackedKeyboard.RemoteKeyboard = true;
    //    EventSystem.current.SetSelectedGameObject(StartingFocusField.gameObject, null);
    //    StartingFocusField.OnPointerClick(new PointerEventData(EventSystem.current));
    //    StartingFocusField.ActivateInputField();
    //    StartingFocusField.onEndEdit.AddListener(OnEndEdit);
    //    StartingFocusField.shouldHideMobileInput = true;
    //}

    private void Update()
    {
        // Check if an external keyboard is connected
        if (trackedKeyboard.ConnectionRequired)
        {
            // Enable the input field for external keyboard input
            StartingFocusField.interactable = true;
            StartingFocusField.ActivateInputField();
            StartingFocusField.shouldHideMobileInput = true;
        }
        else
        {
            // Disable the input field to prevent the onboard virtual keyboard from appearing
            StartingFocusField.interactable = false;
            StartingFocusField.DeactivateInputField();
        }

    }

    void OnGUI()
    {
        //string key = "";
        //bool keydown = false;
        //Event e = Event.current;
        //if (e.type.Equals(EventType.KeyDown) && !keydown)
        //{
        //    keydown = true;
        //    key = e.keyCode.ToString();
        //    StartingFocusField.text = StartingFocusField.text + key;
        //}

        //if (e.type.Equals(EventType.KeyUp))
        //    keydown = false;
    }
}
