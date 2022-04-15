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

    public TMP_InputField m_BallingSpeed, m_spinInput;

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
        m_BallingSpeed.text = "70";
        m_spinInput.text = "2";

        canBowl = true;
    }

    public void OnValueChanged()
    {
        if (string.IsNullOrEmpty(m_BallingSpeed.text))
        {
            m_BallingSpeed.text = "0";
        }

        float speed = float.Parse(m_BallingSpeed.text);

        if (speed > 160)
        {
            speed = 160;
        }
        else if (speed < 70)
        {
            speed = 70;
        }

        Debug.Log(GetInRange(speed));
        m_BallingSpeed.text = speed.ToString();
    }
    public void OnBowl()
    {
        canBowl = false;

        Ball temp = Instantiate(ball);

        temp.transform.position = m_BallRelease.position;

        temp.ShootBall((m_Pointer.transform.position), float.Parse(m_spinInput.text), GetInRange(float.Parse(m_BallingSpeed.text)));

        m_Pointer.gameObject.SetActive(false);

    }

    private float GetInRange(float speed)
    {
        float newRange = 3.5f - 2.25f;
        float oldRange = 160 - 70;
        float newValue = (((speed - 70) * newRange) / oldRange) + 2.25f;
        return newValue;
    }

    public void OnBallOver(Ball b)
    {
        m_Pointer.gameObject.SetActive(true);
        canBowl = true;
    }
}
