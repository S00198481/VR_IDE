using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class InputFieldKeyboard : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private InputSystemUIInputModule inputModule;

    void Start()
    {
        inputModule = FindObjectOfType<InputSystemUIInputModule>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        inputModule.ActivateModule();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        inputModule.DeactivateModule();
    }
}
