using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class VRKeyboardInput : MonoBehaviour
{
    private EventSystem eventSystem;
    private Selectable selectable;

    void Start()
    {
        eventSystem = FindObjectOfType<EventSystem>();
        selectable = GetComponent<Selectable>();
    }

    void OnEnable()
    {
        // Select and activate the input field when the keyboard is opened
        selectable.Select();
        eventSystem.SetSelectedGameObject(selectable.gameObject);
    }

    void OnDisable()
    {
        // Deselect the input field when the keyboard is closed
        eventSystem.SetSelectedGameObject(null);
    }
}
