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
    public GameObject pauseornot;

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

    public void ControlMusic(GameManager gm)
    {
        yourHp = gm.ply1.Life;
        if (yourHp <= 9)
        {
            if (pauseornot.activeInHierarchy)
            {
                losingAudio.DOFade(0f, 3f).SetUpdate(true);
                winningAudio.DOFade(0f, 3f).SetUpdate(true);
                defaultAudio.DOFade(0f, 3f).SetUpdate(true);
                pauseSunAudio.DOFade(1f, 3f).SetUpdate(true);
                pauseMoonAudio.DOFade(0f, 3f).SetUpdate(true);
                pauseDefaultAudio.DOFade(0f, 3f).SetUpdate(true);
            } else
            {
                losingAudio.DOFade(1f, 5f).SetUpdate(true);
                winningAudio.DOFade(0f, 5f).SetUpdate(true);
                defaultAudio.DOFade(0f, 5f).SetUpdate(true);
                pauseSunAudio.DOFade(0f, 5f).SetUpdate(true);
                pauseMoonAudio.DOFade(0f, 5f).SetUpdate(true);
                pauseDefaultAudio.DOFade(0f, 5f).SetUpdate(true);
            }
        } else if (yourHp >= 11)
        {
            if (pauseornot.activeInHierarchy)
            {
                losingAudio.DOFade(0f, 3f).SetUpdate(true);
                winningAudio.DOFade(0f, 3f).SetUpdate(true);
                defaultAudio.DOFade(0f, 3f).SetUpdate(true);
                pauseSunAudio.DOFade(0f, 3f).SetUpdate(true);
                pauseMoonAudio.DOFade(1f, 3f).SetUpdate(true);
                pauseDefaultAudio.DOFade(0f, 3f).SetUpdate(true);
            } else
            {
                losingAudio.DOFade(0f, 5f).SetUpdate(true);
                winningAudio.DOFade(1f, 5f).SetUpdate(true);
                defaultAudio.DOFade(0f, 5f).SetUpdate(true);
                pauseSunAudio.DOFade(0f, 5f).SetUpdate(true);
                pauseMoonAudio.DOFade(0f, 5f).SetUpdate(true);
                pauseDefaultAudio.DOFade(0f, 5f).SetUpdate(true);
            }
        } else
        {
            if (pauseornot.activeInHierarchy)
            {
                losingAudio.DOFade(0f, 3f).SetUpdate(true);
                winningAudio.DOFade(0f, 3f).SetUpdate(true);
                defaultAudio.DOFade(0f, 3f).SetUpdate(true);
                pauseSunAudio.DOFade(0f, 3f).SetUpdate(true);
                pauseMoonAudio.DOFade(0f, 3f).SetUpdate(true);
                pauseDefaultAudio.DOFade(1f, 3f).SetUpdate(true);
            } else
            {
                losingAudio.DOFade(0f, 5f).SetUpdate(true);
                winningAudio.DOFade(0f, 5f).SetUpdate(true);
                defaultAudio.DOFade(1f, 5f).SetUpdate(true);
                pauseSunAudio.DOFade(0f, 5f).SetUpdate(true);
                pauseMoonAudio.DOFade(0f, 5f).SetUpdate(true);
                pauseDefaultAudio.DOFade(0f, 5f).SetUpdate(true);
            }
        }
    }
    public void FinaleMusic(GameManager gm)
    {
        losingAudio.DOFade(0.5f, 1f).SetUpdate(true);
        winningAudio.DOFade(0.5f, 1f).SetUpdate(true);
        defaultAudio.DOFade(0.5f, 1f).SetUpdate(true);
        pauseSunAudio.DOFade(0f, 1f).SetUpdate(true);
        pauseMoonAudio.DOFade(0f, 1f).SetUpdate(true);
        pauseDefaultAudio.DOFade(0f, 1f).SetUpdate(true);
    }

    private void StartCoroutine(IEnumerator enumerator)
    {
        throw new NotImplementedException();
    }
}
