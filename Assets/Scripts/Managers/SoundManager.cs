using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource soundAudioSource;
    public static SoundManager manager;
    public AudioClip alarmSound;
    public AudioClip touchSound;
    public AudioClip buttonSound;


    private void Awake()
    {
        if (manager == null)
        {
            manager = this;
        }
    }
    public void PlaySound()
    {
        soundAudioSource.loop = true;
        soundAudioSource.Play();

    }

    public void ButtonSound()
    {
        soundAudioSource.PlayOneShot(RefrenceManager.instance.buttonClickSound);
    }

    public void PlaySoundTouch()
    {
        soundAudioSource.PlayOneShot(RefrenceManager.instance.attachSound);
    }

    public void QuestionCompleteSound()
    {
        soundAudioSource.PlayOneShot(RefrenceManager.instance.questionCompleteSound);
    }


   

    /// <summary>
    /// it stop the backgroud music
    /// </summary>
    public void StopMusic()
    {

        soundAudioSource.Stop();

    }



}

