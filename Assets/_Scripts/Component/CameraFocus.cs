using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFocus : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera m_Camera;

    public bool IsMainCamera;


    private void Awake()
    {
        m_Camera = this.GetComponent<CinemachineVirtualCamera>();

        GameManager.CameraSwitch += CameraSwitch;

    }

    private void CameraSwitch(bool _IsFrontOn)
    {
        // There will be a way to better check this Todo
        if(IsMainCamera)
        {
            m_Camera.Priority = _IsFrontOn ? 10 : 1;
        }
        else
        {
            m_Camera.Priority = _IsFrontOn ? 1 : 10;
        }
    }
}
