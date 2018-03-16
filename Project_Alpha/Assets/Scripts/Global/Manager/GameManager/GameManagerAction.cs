using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManagerAction : MonoBehaviour
{
    public GameObject canvas;

    private GlobalVariables globalVariables;
    private int actualLevel = 0;
    private List<AudioSource> allAudioOfTheLevel = new List<AudioSource>();
    private List<bool> allAudioPaused = new List<bool>();//se true era in pausa prima del menu, quindi va fatto ripartire
    public AudioMixerGroup generalAudio;
    [HideInInspector] public GameObject mainCamera;

    private void Awake()
    {
        actualLevel = SceneManager.GetActiveScene().buildIndex;
        FillAudioList();
        FillTheAudioControlList();
        globalVariables = GetComponent<GlobalVariables>();
        canvas.SetActive(false);
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
            canvas.SetActive(false);
            Time.timeScale = 1;
            actualLevel = SceneManager.GetActiveScene().buildIndex;
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
            canvas.SetActive(true);
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
            canvas.SetActive(false);
        }
    }

    public void FillAudioList()
    {
        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
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
        for (int i = 0; i < allAudioOfTheLevel.Count; i++)
        {
            if (allAudioOfTheLevel[i].isPlaying)
            {
                allAudioOfTheLevel[i].Pause();
                allAudioPaused[i] = true;
            }
        }
    }

    public void RemovePauseAllAudio()
    {
        for (int i = 0; i < allAudioOfTheLevel.Count; i++)
        {
            if (allAudioPaused[i] == true)
            {
                allAudioOfTheLevel[i].Play();
                allAudioPaused[i] = false;
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
}
