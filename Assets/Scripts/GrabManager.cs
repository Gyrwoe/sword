using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] private GameObject sword;
    [SerializeField] private XRBaseController right_hand;
    private bool _isActive;

    void Start()
    {
        var select = actionAsset.FindActionMap("XRI RightHand").FindAction("Select");
        select.Enable();
        select.performed += OnSelect;
    }

    void Update()
    {
        if (!_isActive)
        {
            sword.transform.parent = null;
            return;
        }

        sword.transform.SetParent(right_hand.transform);
        sword.transform.localPosition = Vector3.zero;
        sword.transform.localRotation = Quaternion.Euler(0, 0, 45);

    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        _isActive = true;
    }

}