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
    private Vector3 startPos, endPos, lastPos, lastDir;
    private float ballSpeed = 3.5f, _bounceCoefficient;
    private bool _isReleased, _isCollided, _ballDone, IsDead = false;
    private float toReach,distanceToCover;

    private float totalBounces = 0;

    private Vector3 releasePoint;

    public void ShootBall(Vector3 Pos, float spinAngle, float speed, float bounceCoefficient = 0.5f)
    {
        releasePoint = m_Transform.position;
        // Set Values
        transform.rotation = Quaternion.Euler(0, spinAngle, 0);
        startPos = m_Transform.position;
        distanceToCover = Vector3.Distance(startPos, Pos); //  Can use (A - B).magnitude
        _bounceCoefficient = bounceCoefficient;
        toReach = Time.time;
        ballSpeed = speed;
        endPos = Pos;
        _isReleased = true; // Should Ball be in a state machine instead -- would be easier to maintain.?

        m_Rigidbody.useGravity = false;
        m_Rigidbody.velocity = Vector3.zero;

        // Updating All Elements that the ball has released.
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

        lastDir = m_Transform.position - lastPos; // Just keep telling me the direction of ball
        lastPos = m_Transform.position;
        Debug.DrawRay(m_Transform.position, m_Transform.forward, Color.black);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("OnBatBox"))
        {
            return;
        }

        if (collision.collider.CompareTag("Boundary"))
        {
            int score;

            if (totalBounces == 0)
            {
                score = 6;
            }
            else
            {
                score = 4;
            }
            GameManager.Instance.ScoreUpdate(score);
            m_Rigidbody.velocity = Vector3.zero;
        }
        else
        {
            _isReleased = false;
            m_Rigidbody.useGravity = true;
            m_Rigidbody.angularVelocity = Vector3.zero;
            moveDirection = Vector3.Reflect(_ballDone ? lastDir.normalized : m_Transform.forward, collision.GetContact(0).normal);

            moveDirection.Normalize();

            // Just need to calculate if bouncing for first time.
            float radians = Mathf.Tan(releasePoint.y /( m_Transform.position.z - releasePoint.z)); // --> basically lesser bounce in case pitched up to look a little better.
            float bounce = _bounceCoefficient - (_ballDone ? 0 : (radians - 0.6f));

            bounce = Mathf.Clamp(bounce, 0.4f, _bounceCoefficient); // Giving a minimum bounce..

            m_Rigidbody.AddForce(new Vector3(moveDirection.x, bounce, moveDirection.z) * ballSpeed, ForceMode.Impulse);
            totalBounces++;

            if (_ballDone)
            {
                ReduceBallForce();
            }
        }

        Debug.Log(collision.gameObject.name);
    }

    private void ReduceBallForce()
    {
        // Reducing balls movement based on bounces
        _bounceCoefficient -= totalBounces * .15f;
        m_Rigidbody.mass = totalBounces * 0.5f;
        ballSpeed -= totalBounces * .1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("OnBatBox"))
        {
            GameManager.OnBatSwing += OnBatSwing;
            GameManager.Instance.BallEnterBatsmanTriggerZone();
        }
    }

    private void OnBatSwing(Vector3 direction, float force)
    {
        ballSpeed = force;
        _isCollided = false;
        _isReleased = false;
        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.AddForce(direction * force, ForceMode.Impulse);
        m_Rigidbody.useGravity = true;
        totalBounces = 0;
        _ballDone = true; // have to add this early on in case ball is a yorker, Game States might help with this, to reduce redundant code.
        Invoke("BallExitTrigger", 0.1f);// Doing this so that ball isn't considered dead, because add force will happen next frame and velocity will be 0;[Currently Ball is killed when Vel = 0, need some other dead condition?]

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
        _ballDone = true;
        _isCollided = true;
        GameManager.Instance.BallExitBastmanTriggerZone();
        GameManager.OnBatSwing -= OnBatSwing;
    }
}
