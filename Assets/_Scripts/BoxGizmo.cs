using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxGizmo : MonoBehaviour
{
    [SerializeField]
    private Transform m_BoxTransform;

    [SerializeField]
    private Color color;

    private void OnDrawGizmos()
    {
        if(m_BoxTransform)
        {
            Gizmos.color = color;
            Gizmos.DrawWireCube(m_BoxTransform.position, m_BoxTransform.localScale);
        }
    }
}
