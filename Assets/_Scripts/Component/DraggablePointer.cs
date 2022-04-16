using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggablePointer : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    private Transform m_Transfom;

    private Camera m_Camera;

    public Vector3 maxOffset;
    public Vector3 minOffset;

    private void Start()
    {
        m_Camera = Camera.main;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        Ray m_Ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        Vector3 m_Pos = m_Transfom.position;
        Vector3 cam_Pos = -m_Camera.transform.forward;
        float t = Vector3.Dot(m_Pos - m_Ray.origin, cam_Pos) / Vector3.Dot(m_Ray.direction, cam_Pos);
        Vector3 P = m_Ray.origin + m_Ray.direction * t;

        m_Transfom.position = new Vector3(Mathf.Clamp(P.x, minOffset.x, maxOffset.x), m_Pos.y, Mathf.Clamp(P.z, minOffset.z, maxOffset.z));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }
}
