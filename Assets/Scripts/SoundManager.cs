using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    public BGM[] bgm;
    public int mapCode;
    public bool isMapChanged;
    public static SoundManager instance;

    public int timer;

    private void Start()
    {
        instance = this;

        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i] = transform.GetChild(i).gameObject.GetComponent<BGM>();
        }

        try
        {
            playMusic(23);
        }
        catch (NullReferenceException)
        {
            Debug.Log("오디오가 비어있음");
        }

        isMapChanged = true;
    }

    public void playMusic(int num)
    {
        bgm[num].playBGM();
    }

    public void refreshSounds()
    {
        CancelInvoke();
        stopAllSounds();
        isMapChanged = true;
    }

    public void stopAllSounds()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].stopBGM();
        }
    }
}
