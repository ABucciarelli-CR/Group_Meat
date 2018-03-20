using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManagerAction : MonoBehaviour
{
    public GameObject canvas;
    public EventSystem eventSystem;
    public List<GameObject> menuParts;

    private GlobalVariables globalVariables;
    private int actualLevel = 0;
    public List<AudioSource> allAudioOfTheLevel = new List<AudioSource>();
    public List<bool> allAudioPaused = new List<bool>();//se true era in pausa prima del menu, quindi va fatto ripartire
    public AudioMixerGroup generalAudio;
    [HideInInspector] public GameObject mainCamera;

    private void Awake()
    {
        StartCoroutine(WaitForStartingMenu());
        actualLevel = SceneManager.GetActiveScene().buildIndex;
        FillAudioList();
        FillTheAudioControlList();
        globalVariables = GetComponent<GlobalVariables>();
    }

    private void Update()
    {
        if(mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
        if(actualLevel != SceneManager.GetActiveScene().buildIndex)
        {
            ClearAllAudio();
            //canvas.SetActive(false);
            foreach (GameObject go in menuParts)
            {
                go.SetActive(false);
            }
            Time.timeScale = 1;
            actualLevel = SceneManager.GetActiveScene().buildIndex;
        }

        foreach(AudioSource audSource in allAudioOfTheLevel)
        {
            if(audSource == null)
            {
                ClearAllAudio();
                break;
            }
        }
    }

    public void RestartTheLevel(bool isRestart)
    {
        if (isRestart)
        {
            //globalVariables.closeDoor = false;
            //globalVariables.enemyDead = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            ClearAllAudio();
        }
    }

    public void PauseLevel(bool pause)
    {
        if(pause && SceneManager.GetActiveScene().buildIndex != 0)
        {
            mainCamera.GetComponent<Cinemachine.CinemachineBrain>().m_UpdateMethod = mainCamera.GetComponent<Cinemachine.CinemachineBrain>().updateFixed;
            //generalAudio.audioMixer.SetFloat("BGVolume", -80f);
            PauseAllAudio();
            Time.timeScale = 0;
            //canvas.SetActive(true);
            foreach (GameObject go in menuParts)
            {
                go.SetActive(true);
            }
            eventSystem.firstSelectedGameObject = menuParts[1];
        }
    }

    public void ExitPauseLevel(bool pause)
    {
        if (pause)
        {
            //generalAudio.audioMixer.SetFloat("BGVolume", 0f);
            RemovePauseAllAudio();
            StartCoroutine(WaitForCamera());
            Time.timeScale = 1;
            //canvas.SetActive(false);
            foreach (GameObject go in menuParts)
            {
                go.SetActive(false);
            }
        }
    }

    public void FillAudioList()
    {
        //foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        GameObject[] allObjInScene = FindObjectsOfType<GameObject>() as GameObject[];
        foreach (GameObject go in allObjInScene)
        {
            if (go.GetComponent<AudioSource>() != null)
            {
                if(!go.CompareTag("Untagged"))
                {
                    allAudioOfTheLevel.Add(go.GetComponent<AudioSource>());
                }
            }
        }
    }

    public void FillTheAudioControlList()
    {
        for(int i=0; i<allAudioOfTheLevel.Count; i++)
        {
            allAudioPaused.Add(false);
        }
    }

    public void PauseAllAudio()
    {
        if(allAudioOfTheLevel != null)
        {
            for (int i = 0; i < allAudioOfTheLevel.Count; i++)
            {
                if (allAudioOfTheLevel[i].isPlaying)
                {
                    allAudioOfTheLevel[i].Pause();
                    //allAudioOfTheLevel[i].enabled = false;
                    allAudioPaused[i] = true;
                }
            }
        }
    }

    public void RemovePauseAllAudio()
    {
        if (allAudioOfTheLevel != null)
        {
            for (int i = 0; i < allAudioOfTheLevel.Count; i++)
            {
                if (allAudioPaused[i] == true)
                {
                    allAudioOfTheLevel[i].UnPause();
                    //allAudioOfTheLevel[i].enabled = true;
                    allAudioPaused[i] = false;
                }
            }
        }
    }

    public void ClearAllAudio()
    {
        allAudioOfTheLevel.Clear();
        allAudioPaused.Clear();
        FillAudioList();
        FillTheAudioControlList();
    }

    IEnumerator WaitForCamera()
    {
        yield return new WaitForSeconds(.1f);
        mainCamera.GetComponent<Cinemachine.CinemachineBrain>().m_UpdateMethod = mainCamera.GetComponent<Cinemachine.CinemachineBrain>().updateSmart;
    }

    IEnumerator WaitForStartingMenu()
    {
        yield return new WaitForSeconds(.01f);
        //canvas.SetActive(false);
        foreach (GameObject go in menuParts)
        {
            go.SetActive(false);
        }
    }
}
