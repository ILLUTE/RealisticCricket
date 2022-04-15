using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private Transform m_Transform;

    [SerializeField]
    private Rigidbody m_Rigidbody;

    [Header("Variables")]
    private Vector3 moveDirection;
    private Vector3 startPos,endPos;
    private float ballSpeed, maxSpeed = 3.5f, pitchBounce;
    private bool _isReleased, _isCollided, IsDead = false;
    private float toReach,distanceToCover;

    public void ShootBall(Vector3 Pos, float spinAngle, float speed,float _pitchBounceCoefficient = 0.65f)
    {
        // Set Values
        pitchBounce = _pitchBounceCoefficient;
        ballSpeed = maxSpeed = speed;
        endPos = Pos;
        toReach = Time.time;
        startPos = m_Transform.position;
        transform.rotation = Quaternion.Euler(0, spinAngle, 0);
        distanceToCover = Vector3.Distance(startPos, Pos); //  Can use (A - B).magnitude
        _isReleased = true; // Should Ball be in a state?

        m_Rigidbody.useGravity = false;
        m_Rigidbody.velocity = Vector3.zero;

        moveDirection = endPos - transform.position;

        GameManager.Instance.BallReleased(this);

    }

    private void Update()
    {
        if (_isCollided & !IsDead) // Ball State will reduce code from here.. 
        {
            if (m_Rigidbody.velocity.magnitude <= 0)
            {
                IsDead = true;
                GameManager.Instance.EndCurrentBall(this);
            }
        }
        if (_isReleased)
        {
            float timeTaken = Time.time - toReach;
            float distanceCovered = timeTaken * (ballSpeed / 1.5f);
            float frac = distanceCovered / distanceToCover;

            m_Transform.position = Vector3.Lerp(startPos, endPos, frac);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("OnBatBox"))
        {
            return;
        }
        _isReleased = false;
        _isCollided = true;
        m_Rigidbody.useGravity = true;
        moveDirection = Vector3.Reflect(transform.forward, collision.contacts[0].normal);
        m_Transform.forward = moveDirection;
        m_Rigidbody.velocity = new Vector3(moveDirection.x, pitchBounce, moveDirection.z) * ballSpeed;
        ballSpeed = Mathf.Clamp(ballSpeed - 0.2f, 0, maxSpeed);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("OnBatBox"))
        {
            Debug.Log("Why");
            return;
        }
        ballSpeed = Mathf.Clamp(ballSpeed - 0.2f, 0, maxSpeed);
        m_Rigidbody.velocity = m_Rigidbody.velocity.normalized * ballSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("OnBatBox"))
        {
            GameManager.OnBatSwing += OnBatSwing;
            GameManager.Instance.BallEnterBatsmanTriggerZone();
        }
    }

    private void OnBatSwing(Vector3 direction,float force)
    {
        _isCollided = false;
        m_Rigidbody.velocity = Vector3.zero;
        m_Transform.forward = direction;
        m_Rigidbody.velocity = direction * force;
        _isCollided = true;

        BallExitTrigger();
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("OnBatBox"))
        {
            BallExitTrigger();
        }
    }

    private void BallExitTrigger()
    {
        GameManager.Instance.BallExitBastmanTriggerZone();
        GameManager.OnBatSwing -= OnBatSwing;
    }
}
