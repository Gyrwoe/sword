using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HideRay : MonoBehaviour
{
    public XRController rightTPController;
    private XRInteractorLineVisual rightTeleportRay;
    public InputHelpers.Button teleportActivationButton;
    public float activationThreshold = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        rightTeleportRay = rightTPController.GetComponent(typeof(XRInteractorLineVisual)) as XRInteractorLineVisual;
    }

    // Update is called once per frame
    void Update()
    {
        if (rightTeleportRay)
        {
            rightTeleportRay.enabled = CheckIfActivated(rightTPController);
        }
    }

    public bool CheckIfActivated(XRController ray)
    {
        InputHelpers.IsPressed(ray.inputDevice, teleportActivationButton, out bool isActivated, activationThreshold);
        return isActivated;
    }
}
