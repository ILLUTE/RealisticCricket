using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookGizmo : MonoBehaviour
{
    [SerializeField]
    private Transform m_Transform;

    [SerializeField]
    private Color m_Color;

    [SerializeField]
    [Range(0.01f, 5)]
    private float m_Radius;

    private void OnDrawGizmos()
    {
        if(m_Transform)
        {
            Gizmos.color = m_Color;

            Gizmos.DrawWireSphere(m_Transform.position, m_Radius);
        }
    }
}
