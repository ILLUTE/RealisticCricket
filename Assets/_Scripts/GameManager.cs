using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }

            return instance;
        }
    }

    public static event Action OnPlayBall;
    public static event Action<Ball> OnBallReleased;
    public static event Action OnBallEnteredHit;
    public static event Action OnBallExitedHit;
    public static event Action<Vector3,float> OnBatSwing;
    public static event Action<Ball> OnBallDead;
    public static event Action<bool> OnPlayLofted;

    public static event Action<int> UpdateScore;

    private bool IsLofted;

    private void Start()
    {
        Physics.gravity = new Vector3(0, -5, 0);
    }
    public void StartBowling()
    {
        OnPlayBall?.Invoke();
    }

    public void BallReleased(Ball b)
    {
        OnBallReleased?.Invoke(b);
    }

    public void BallEnterBatsmanTriggerZone()
    {
        OnBallEnteredHit?.Invoke();
    }

    public void BallExitBastmanTriggerZone()
    {
        OnBallExitedHit?.Invoke();
    }

    public void BatSwung(Vector3 direction, float force)
    {
        OnBatSwing?.Invoke(direction, force);
    }

    public void EndCurrentBall(Ball b)
    {
        OnBallDead?.Invoke(b);
        Destroy(b.gameObject);
    }
    public bool PlayLoftedShot()
    {
        IsLofted = !IsLofted;
        OnPlayLofted?.Invoke(IsLofted);
        return IsLofted;
    }

    public void ScoreUpdate(int _score)
    {
        UpdateScore?.Invoke(_score);
    }
}
