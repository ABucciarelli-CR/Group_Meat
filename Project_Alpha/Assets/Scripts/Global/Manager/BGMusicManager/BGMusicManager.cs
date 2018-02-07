using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusicManager : MonoBehaviour
{
    public List<AudioClip> music;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    void Start ()
    {
        audioSource.clip = music[0];
        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        
	}
}
