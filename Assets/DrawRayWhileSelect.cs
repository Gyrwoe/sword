using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.XR.Interaction.Toolkit;

public class DrawRayWhileSelect : MonoBehaviour
{

    void EnableDisable()
    {
        if (gameObject..activeSelf == false)
        {
            gameObject.SetActive(true);
        }
        else if (gameObject..activeSelf == true)
        {
            gameObject.SetActive(false);
        }
    }

}
