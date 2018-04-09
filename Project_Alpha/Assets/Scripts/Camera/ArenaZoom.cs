using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaZoom : MonoBehaviour
{
    [HideInInspector]public float variable = 0f;
    [HideInInspector]public float zoomSpeed = 1f;
    [HideInInspector]public float targetOrtho;
    public float smoothSpeed = 2.0f;
    public float changeCameraSpeed = 50f;
    public bool detachCameraFromPlayerInArena = false;
    [HideInInspector]public float minOrtho = 1.0f;
    [HideInInspector]public float maxOrtho = 20.0f;

    void Start()
    {
        targetOrtho = gameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize;
    }

    void Update()
    {

        if(gameObject.GetComponent<CinemachineVirtualCamera>().m_Follow == null)
        {
            gameObject.GetComponent<CinemachineVirtualCamera>().m_Follow = GameObject.Find("PlayerStateMachine").transform;
        }

        float scroll = variable;
        if (scroll != 0.0f)
        {
            targetOrtho -= scroll * zoomSpeed;
            targetOrtho = Mathf.Clamp(targetOrtho, minOrtho, maxOrtho);
        }
        gameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);
    }
}