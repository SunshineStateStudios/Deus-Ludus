using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class MusicController
{
    public GameObject losingMusic;
    public GameObject winningMusic;
    public GameObject defaultMusic;
    public GameObject sunPause;
    public GameObject moonPause;
    public GameObject defaultPause;
    public int yourHp = 0;
    

    private AudioSource losingAudio;
    private AudioSource winningAudio;
    private AudioSource defaultAudio;
    private AudioSource pauseSunAudio;
    private AudioSource pauseMoonAudio;
    private AudioSource pauseDefaultAudio;

    private GameObject pauseornot;

    public void Initialise()
    {
        //battle music
        losingAudio = losingMusic.GetComponent<AudioSource>();
        winningAudio = winningMusic.GetComponent<AudioSource>();
        defaultAudio = defaultMusic.GetComponent<AudioSource>();

        //pause music
        pauseSunAudio = sunPause.GetComponent<AudioSource>();
        pauseMoonAudio = moonPause.GetComponent<AudioSource>();
        pauseDefaultAudio = defaultPause.GetComponent<AudioSource>();

        losingAudio.Play();
        winningAudio.Play();
        defaultAudio.Play();
        pauseSunAudio.Play();
        pauseMoonAudio.Play();
        pauseDefaultAudio.Play();
    }
    /*public IEnumerator musicFade(AudioSource music, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = music.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            music.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        music.volume = targetVolume;
    }*/
    public void ControlMusic(GameManager gm)
    {
        //PauseController pauseController = pauseornot.GetComponent<PauseController>();
        pauseornot = GameObject.Find("Canvas/Pause");
        yourHp = gm.ply1.Life;
        if (yourHp <= 9)
        {
            if (pauseornot.activeInHierarchy)
            {
                losingAudio.DOFade(0f, 5f);
                winningAudio.DOFade(0f, 5f);
                defaultAudio.DOFade(0f, 5f);
                pauseSunAudio.DOFade(0.5f, 5f);
                pauseMoonAudio.DOFade(0f, 5f);
                pauseDefaultAudio.DOFade(0f, 5f);
            } else
            {
                losingAudio.DOFade(1f, 5f);
                winningAudio.DOFade(0f, 5f);
                defaultAudio.DOFade(0f, 5f);
                pauseSunAudio.DOFade(0f, 5f);
                pauseMoonAudio.DOFade(0f, 5f);
                pauseDefaultAudio.DOFade(0f, 5f);
            }
        } else if (yourHp >= 11)
        {
            if (pauseornot.activeInHierarchy)
            {
                losingAudio.DOFade(0f, 5f);
                winningAudio.DOFade(0f, 5f);
                defaultAudio.DOFade(0f, 5f);
                pauseSunAudio.DOFade(0f, 5f);
                pauseMoonAudio.DOFade(0.5f, 5f);
                pauseDefaultAudio.DOFade(0f, 5f);
            } else
            {
                losingAudio.DOFade(0f, 5f);
                winningAudio.DOFade(1f, 5f);
                defaultAudio.DOFade(0f, 5f);
                pauseSunAudio.DOFade(0f, 5f);
                pauseMoonAudio.DOFade(0f, 5f);
                pauseDefaultAudio.DOFade(0f, 5f);
            }
        } else
        {
            if (pauseornot.activeInHierarchy)
            {
                losingAudio.DOFade(0f, 5f);
                winningAudio.DOFade(0f, 5f);
                defaultAudio.DOFade(0f, 5f);
                pauseSunAudio.DOFade(0f, 5f);
                pauseMoonAudio.DOFade(0.5f, 5f);
                pauseDefaultAudio.DOFade(0f, 5f);
            } else
            {
                losingAudio.DOFade(0f, 5f);
                winningAudio.DOFade(0f, 5f);
                defaultAudio.DOFade(1f, 5f);
                pauseSunAudio.DOFade(0f, 5f);
                pauseMoonAudio.DOFade(0f, 5f);
                pauseDefaultAudio.DOFade(0f, 5f);
            }
        }
    }

    private void StartCoroutine(IEnumerator enumerator)
    {
        throw new NotImplementedException();
    }
}
