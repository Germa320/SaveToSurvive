using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicHandler : MonoBehaviour
{
    public AudioClip clipToLoop;
    bool isPlayingLoop = false;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!audioSource.isPlaying && !isPlayingLoop)
        {
            audioSource.loop = true;
            audioSource.clip = clipToLoop;
            audioSource.Play();
            isPlayingLoop = true;
        }
    }
}
