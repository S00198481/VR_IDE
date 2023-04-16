using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

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
        bool keydown = false;
        Event e = Event.current;
        if (e.type.Equals(EventType.KeyDown) && !keydown)
        {
            keydown = true;

            if (e.keyCode == KeyCode.Backspace)
            {
                if (StartingFocusField.text.Length > 0)
                {
                    // Remove the last character from the text
                    StartingFocusField.text = StartingFocusField.text.Substring(0, StartingFocusField.text.Length - 1);
                }
            }
            else
            {
                // Get the output of the key pressed
                string output = GetKeyOutput(e);

                if (output != "")
                {
                    // Add the output to the text
                    StartingFocusField.text += output;
                }
            }           
            //StartingFocusField.text = StartingFocusField.text + GetKeyOutput(Event.current);
            //key = e.keyCode.ToString();
            //StartingFocusField.text = StartingFocusField.text + key;
        }

        if (e.type.Equals(EventType.KeyUp))
            keydown = false;
    }

    string GetKeyOutput(Event e)
    {
        // Check if Shift key is held down
        bool isShiftPressed = (Event.current.modifiers & EventModifiers.Shift) != 0;

        // Check if KeyCode is alphabetical
        if (e.type == EventType.KeyDown && e.keyCode >= KeyCode.A && e.keyCode <= KeyCode.Z)
        {
            // Convert KeyCode to char
            char c = (char)((int)e.keyCode + (isShiftPressed ? 'A' - 'a' : 0));
            return c.ToString();
        }

        // Check if KeyCode is a number
        if (e.type == EventType.KeyDown && e.keyCode >= KeyCode.Alpha0 && e.keyCode <= KeyCode.Alpha9)
        {
            // Convert KeyCode to char
            char c = (char)((int)e.keyCode - KeyCode.Alpha0 + '0');
            return c.ToString();
        }

        // Check if KeyCode is a symbol
        switch (e.keyCode)
        {
            case KeyCode.Space:
                return " ";
            case KeyCode.Comma:
                return isShiftPressed ? "<" : ",";
            case KeyCode.Period:
                return isShiftPressed ? ">" : ".";
            case KeyCode.Semicolon:
                return isShiftPressed ? ":" : ";";
            case KeyCode.Quote:
                return isShiftPressed ? "\"" : "'";
            case KeyCode.Minus:
                return isShiftPressed ? "_" : "-";
            case KeyCode.Equals:
                return isShiftPressed ? "+" : "=";
            case KeyCode.LeftBracket:
                return isShiftPressed ? "{" : "[";
            case KeyCode.RightBracket:
                return isShiftPressed ? "}" : "]";
            case KeyCode.Backslash:
                return isShiftPressed ? "|" : "\\";
            case KeyCode.Slash:
                return isShiftPressed ? "?" : "/";
            default:
                return "";
        }
    }
}
