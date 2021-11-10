﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] private XRRayInteractor rayInteractor;
    [SerializeField] private TeleportationProvider provider;
    private InputAction _thumbstick;
    private bool _isActive;

    void Start()
    {
        rayInteractor.enabled = false;

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
        if (!_isActive)
            return;

        if (_thumbstick.triggered)
            return;

        if (!rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            rayInteractor.enabled = false;
            _isActive = false;
            return;
        }

        TeleportRequest request = new TeleportRequest()
        {
            destinationPosition = hit.point,
            destinationRotation = actionAsset.FindActionMap("XRI HMD").FindAction("Rotation").ReadValue<Quaternion>()

        };

        provider.QueueTeleportRequest(request);
        rayInteractor.enabled = false;
        _isActive = false;
    }

    private void OnTeleportActivate(InputAction.CallbackContext context)
    {
        rayInteractor.enabled = true;
        _isActive = true;
    }

    private void OnTeleportCancel(InputAction.CallbackContext context)
    {
        rayInteractor.enabled = false;
        _isActive = false;
    }
}