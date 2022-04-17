using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SwitchCamera : MonoBehaviour
{
    private bool IsFrontOn = true;

    public void Toggle()
    {
        IsFrontOn = !IsFrontOn;

        GameManager.Instance.SwitchCamera(IsFrontOn);
    }
}
