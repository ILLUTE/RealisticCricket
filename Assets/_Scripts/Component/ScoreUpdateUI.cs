using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using TMPro;

public class ScoreUpdateUI : MonoBehaviour
{
    public RectTransform m_AnimAnchor;
    public TextMeshProUGUI m_ScoreText;

    private Queue<int> queue = new Queue<int>();
    private bool IsAnimating;



    private void Awake()
    {
        GameManager.UpdateScore += UpdateScore;
    }

    private void UpdateScore(int _score)
    {
        UpdateQueue(_score);
    }

    private void UpdateQueue(int score)
    {
        queue.Enqueue(score);
        CheckQueue();
    }

    private void CheckQueue()
    {
        if (queue.Count > 0 && !IsAnimating)
        {
            SetSequence();
        }
    }

    private void SetSequence()
    {
        IsAnimating = true;

        m_ScoreText.text = queue.Peek().ToString();

        Sequence m_sequence = DOTween.Sequence();

        m_sequence.Append(m_AnimAnchor.DOAnchorPosY(0, 0.2f)).AppendInterval(1f).Append(m_AnimAnchor.DOAnchorPosY(300, .2f));

        m_sequence.OnComplete(() =>
        {
            IsAnimating = false;
            queue.Dequeue();
            CheckQueue();
        });
    } }
