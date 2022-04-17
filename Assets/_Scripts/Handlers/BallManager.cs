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

    public void OnBowl()
    {
        canBowl = false;

        Ball temp = Instantiate(ball); // Can just reuse the ball as well, Todo if I get time.

        temp.transform.position = m_BallRelease.position;

        float.TryParse(m_spinInput.text, out float spinAngle);
        float.TryParse(m_BallingSpeed.text, out float ballSpeed);

        ballSpeed = ballSpeed.GetInRange(Constants.MIN_BALLSPEED_V, Constants.MAX_BALLSPEED_V, Constants.MIN_BALLSPEED_R, Constants.MAX_BALLSPEED_R);

        temp.ShootBall(m_Pointer.transform.position, spinAngle, ballSpeed);

        m_Pointer.gameObject.SetActive(false);

    }
    public void OnBallOver(Ball b)
    {
        m_Pointer.gameObject.SetActive(true);
        canBowl = true;
    }
}
