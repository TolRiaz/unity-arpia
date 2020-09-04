using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public bool isPlay;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        isPlay = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlay)
        {
            audioSource.Play();
            isPlay = false;
        }
    }

    public void playBGM()
    {
        if (audioSource.isPlaying)
        {
            return;
        }

        isPlay = true;
    }

    public void stopBGM()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        isPlay = false;
    }
}
