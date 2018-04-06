using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaZoom : MonoBehaviour
{
    public float variable = 0f;
    public float zoomSpeed = 1f;
    public float targetOrtho;
    public float smoothSpeed = 2.0f;
    public float minOrtho = 1.0f;
    public float maxOrtho = 20.0f;

    void Start()
    {
        targetOrtho = gameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize;
    }

    void Update()
    {
        float scroll = variable;
        if (scroll != 0.0f)
        {
            targetOrtho -= scroll * zoomSpeed;
            targetOrtho = Mathf.Clamp(targetOrtho, minOrtho, maxOrtho);
        }
        gameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);
    }
}