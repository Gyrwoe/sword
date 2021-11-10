using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HideRay : MonoBehaviour
{
    public ActionBasedController rightTPController;
    public ActionBasedController leftTPController;
    private XRInteractorLineVisual rightTeleportRay;
    private XRInteractorLineVisual leftTeleportRay;
    // Start is called before the first frame update
    void Start()
    {
        rightTeleportRay = rightTPController.GetComponent(typeof(XRInteractorLineVisual)) as XRInteractorLineVisual;
        leftTeleportRay = leftTPController.GetComponent(typeof(XRInteractorLineVisual)) as XRInteractorLineVisual;
    }

    // Update is called once per frame
    void Update()
    {
        if (rightTeleportRay)
        {
            rightTeleportRay.enabled = CheckIfActivated(rightTPController);
        }
        if (leftTeleportRay)
        {
            leftTeleportRay.enabled = CheckIfActivated(leftTPController);
        }
    }

    public bool CheckIfActivated(XRBaseController ray)
    {
        return ray.activateInteractionState.active;
    }
}
