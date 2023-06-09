using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialControl : MonoBehaviour
{
    Vector3 m_startRotation = Vector3.zero;
    MeshRenderer m_meshRenderer = null;

    void Start()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();
    }

    public void StartTurn()
    {
        m_startRotation = transform.localEulerAngles;
        m_meshRenderer.material.SetColor("_Color", Color.red);
    }

    public void StopTurn()
    {
        m_meshRenderer.material.SetColor("_Color", Color.white);
    }

    public void DialUpdate(float angle)
    {
        Vector3 angles = m_startRotation;
        angles.y += angle;
        transform.localEulerAngles = angles;
    }
}
