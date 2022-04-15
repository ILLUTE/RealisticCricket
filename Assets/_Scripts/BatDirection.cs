using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class BatDirection : MonoBehaviour
{
    public TMP_InputField m_BatSpeed;

    public Transform batHit;

    private Vector2 startPos;

    public Vector3 currentDirection;

    private bool canSwing, IsLoft;

    private void Awake()
    {
        GameManager.OnBallDead += OnBallDead;
        GameManager.OnBallReleased += OnBallReleased;
        GameManager.OnBallEnteredHit += OnEnterHit;
        GameManager.OnBallExitedHit += OnExitHit;
        GameManager.OnPlayLofted += Loft;
    }

    private void OnExitHit()
    {
        CanSwing(false);
        batHit.gameObject.SetActive(false);
    }

    private void OnEnterHit()
    {
        CanSwing(true);
    }

    private void OnBallReleased(Ball obj)
    {
        batHit.gameObject.SetActive(true);
    }

    private void OnBallDead(Ball obj)
    {
        currentDirection = Vector3.zero;
        CanSwing(false);
        batHit.gameObject.SetActive(false);
    }

    private void Start()
    {
        m_BatSpeed.text = "40";
        batHit.gameObject.SetActive(false);
    }

    public void Loft(bool isLoft)
    {
        IsLoft = isLoft;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
        }

        if(Input.GetMouseButtonUp(0))
        {
            Calculate(Input.mousePosition);
        }
    }

    private void Calculate(Vector2 endPos)
    {
        if (canSwing)
        {
            Vector2 dir = endPos - startPos;
            dir = dir.normalized;
            currentDirection = new Vector3(dir.x, IsLoft ? .75f : -.5f, dir.y);
            GameManager.Instance.BatSwung(currentDirection, float.Parse(m_BatSpeed.text));
        }
    }

    public void CanSwing(bool _swing)
    {
        canSwing = _swing;
    }
}
