using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset actionAsset;
    private InputAction _thumbstick;
    void Start()
    {
        var activate = actionAsset.FindActionMap("XRI RightHand").FindAction("Teleportation Mode Activate");
        activate.Enable();
        activate.performed += OnTeleportActivate;

        var cancel = actionAsset.FindActionMap("XRI RightHand").FindAction("Teleportation Mode Cancel");
        cancel.Enable();
        cancel.performed += OnTeleportCancel;

        _thumbstick = actionAsset.FindActionMap("XRI RightHand").FindAction("Move");
        _thumbstick.Enable();
    }

    void Update()
    {
        
    }

    private void OnTeleportActivate(InputAction.CallbackContext context)
    {

    }

    private void OnTeleportCancel(InputAction.CallbackContext context)
    {

    }
}