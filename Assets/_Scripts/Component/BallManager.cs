using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class BallManager : MonoBehaviour
{
    public Transform m_BallRelease;
    public DraggablePointer m_Pointer;

    public Ball ball;

    public TMP_InputField m_InputField, m_spinInput;

    private bool canBowl;

    private void Awake()
    {
        GameManager.OnPlayBall += StartBowling;
        GameManager.OnBallDead += OnBallOver;
    }

    private void StartBowling()
    {
        if (!canBowl)
        {
            return;
        }

        OnBowl();
    }

    private void Start()
    {
        m_InputField.text = "3";
        m_spinInput.text = "2";

        canBowl = true;
    }
    public void OnBowl()
    {
        Ball temp = Instantiate(ball);

        temp.transform.position = m_BallRelease.position;

        temp.ShootBall((m_Pointer.transform.position), float.Parse(m_spinInput.text), float.Parse(m_InputField.text));

        m_Pointer.gameObject.SetActive(false);

        canBowl = false;
    }

    public void OnBallOver(Ball b)
    {
        m_Pointer.gameObject.SetActive(true);
        canBowl = true;
    }
}
