using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Activator : MonoBehaviour
{
    public GameObject arenaDoor;
    public float standardCameraOrtho = 720;

    [SerializeField]private GameObject upPoint;
    [SerializeField]private GameObject downPoint;
    [SerializeField] private GameObject center;

    private bool isPlayerIn;

    private float arenaOrtho;

    private GameObject cmVcam;
    private GameObject player;

    private Transform passageGameObject;
    private Transform pos1;
    private Transform pos2;

    private bool onlyOne = false;
    private bool doTheTranslate = false;

    private float t = 0;

    void Start ()
    {
        cmVcam = GameObject.Find("CM vcam1");
        player = GameObject.Find("PlayerStateMachine");

        arenaOrtho = (upPoint.transform.position.y - downPoint.transform.position.y) - standardCameraOrtho;


    }

	void Update ()
    {
        if(doTheTranslate)
        {
            //step = .1f * Time.deltaTime;

            //passageGameObject.position = Vector3.MoveTowards(passageGameObject.position, pos2.position, .000000001f);

            //t += Time.deltaTime / 9000000000;

            passageGameObject.position = Vector3.Lerp(passageGameObject.position, pos2.position, .2f);

            if (passageGameObject.position.x < pos2.position.x)
            {
                Debug.Log("End");
                doTheTranslate = false;
                //cmVcam.GetComponent<CinemachineVirtualCamera>().m_Follow = pos2;
            }
            
        }

		if(isPlayerIn && !onlyOne)
        {
            onlyOne = true;
            if (cmVcam.GetComponent<ArenaZoom>().detachCameraFromPlayerInArena)
            {
                //cmVcam.GetComponent<CinemachineVirtualCamera>().m_Follow = center.transform;
                ChangeCamera(cmVcam.GetComponent<CinemachineVirtualCamera>().m_Follow, center.transform);

                cmVcam.GetComponent<ArenaZoom>().targetOrtho = arenaOrtho;
            }
            else
            {
                cmVcam.GetComponent<ArenaZoom>().targetOrtho = arenaOrtho;
            }
        }
        /*
        if(arenaDoor.GetComponent<CheckArenaEnemyEnd>().enemyEnd && onlyOne)
        {
            onlyOne = false;
            if (cmVcam.GetComponent<ArenaZoom>().detachCameraFromPlayerInArena)
            {
                cmVcam.GetComponent<CinemachineVirtualCamera>().m_Follow = player.transform;
                //ChangeCamera(cmVcam.GetComponent<CinemachineVirtualCamera>().m_Follow, player.transform);

                cmVcam.GetComponent<ArenaZoom>().targetOrtho = standardCameraOrtho;
            }
            else
            {
                cmVcam.GetComponent<ArenaZoom>().targetOrtho = standardCameraOrtho;
            }
        }*/
	}

    private void ChangeCamera(Transform playerCamera, Transform centerArenaCamera)
    {
        Transform appTrans = cmVcam.transform;
        GameObject _appGameObject = Instantiate<GameObject>(new GameObject("GiovanniSpruzzi"), appTrans.position, Quaternion.identity);
        cmVcam.GetComponent<CinemachineVirtualCamera>().m_Follow = _appGameObject.transform;

        passageGameObject = _appGameObject.transform;
        pos1 = appTrans;
        pos2 = centerArenaCamera;
        doTheTranslate = true;
        //cmVcam.GetComponent<Transform>().position = player.transform.position;

        //StartCoroutine(LerpFromTo(_appGameObject.transform, appTrans, centerArenaCamera, 50f /*cmVcam.GetComponent<ArenaZoom>().changeCameraSpeed*/));

        /*
        float min;
        float max;

        float xDistance = playerCamera.position.x - centerArenaCamera.position.x;
        float yDistance = playerCamera.position.y - centerArenaCamera.position.y;

        if(xDistance < yDistance)
        {
            if(xDistance > 0)
            {
                MinToMaxTranslator(playerCamera, centerArenaCamera, true, true);
            }
            else
            {
                MinToMaxTranslator(playerCamera, centerArenaCamera, true, false);
            }

            min = xDistance;
            max = yDistance;
        }
        else
        {
            if (xDistance > 0)
            {
                MinToMaxTranslator(playerCamera, centerArenaCamera, false, true);
            }
            else
            {
                MinToMaxTranslator(playerCamera, centerArenaCamera, false, false);
            }

            min = yDistance;
            max = xDistance;
        }*/
    }

    private void MinToMaxTranslator(Transform firstCamera, Transform secondCamera, bool isX, bool sum)
    {
        if(isX)
        {
            if(sum)
            {
                for (; firstCamera.position.x < secondCamera.position.x;)
                {
                    firstCamera.position = new Vector2(firstCamera.position.x + (cmVcam.GetComponent<ArenaZoom>().changeCameraSpeed / (firstCamera.position.y / firstCamera.position.x)), firstCamera.position.y + cmVcam.GetComponent<ArenaZoom>().changeCameraSpeed);
                }
            }
            else
            {
                for (; firstCamera.position.x < secondCamera.position.x;)
                {
                    firstCamera.position = new Vector2(firstCamera.position.x - (cmVcam.GetComponent<ArenaZoom>().changeCameraSpeed / (firstCamera.position.y / firstCamera.position.x)), firstCamera.position.y + cmVcam.GetComponent<ArenaZoom>().changeCameraSpeed);
                }
            }
            
        }
        else
        {
            for (; firstCamera.position.x < secondCamera.position.x;)
            {
                firstCamera.position = new Vector2(firstCamera.position.x + cmVcam.GetComponent<ArenaZoom>().changeCameraSpeed, firstCamera.position.y + (cmVcam.GetComponent<ArenaZoom>().changeCameraSpeed / (firstCamera.position.x / firstCamera.position.y)));
            }
        }
    }

    IEnumerator LerpFromTo(Transform appGameobject, Transform pos1, Transform pos2, float duration)
    {
        /*
        for (float t = 0f; appGameobject.position.x < pos2.position.x; t += Time.deltaTime)
        {
            appGameobject.position = Vector3.Lerp(pos1.position, pos2.position, t / duration);
            yield return 50f;
        }
        cmVcam.GetComponent<CinemachineVirtualCamera>().m_Follow = pos2;*/
        for (float step; appGameobject.position.x < pos2.position.x;)
        {
            step = duration * Time.deltaTime;

            appGameobject.position = Vector3.MoveTowards(pos1.position, pos2.position, step);
            yield return 10f;
        }
        cmVcam.GetComponent<CinemachineVirtualCamera>().m_Follow = pos2;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerIn = true;
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerIn = true;
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }
}
