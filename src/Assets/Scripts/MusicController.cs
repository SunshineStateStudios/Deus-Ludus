using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class MusicController
{
    public GameObject losingMusic;
    public GameObject winningMusic;
    public GameObject defaultMusic;
    public int yourHp = 0;
    

    private AudioSource losingAudio;
    private AudioSource winningAudio;
    private AudioSource defaultAudio;

    public void Initialise()
    {
        losingAudio = losingMusic.GetComponent<AudioSource>();
        winningAudio = winningMusic.GetComponent<AudioSource>();
        defaultAudio = defaultMusic.GetComponent<AudioSource>();
        losingAudio.Play();
        winningAudio.Play();
        defaultAudio.Play();
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
        yourHp = gm.ply1.Life;
        if (yourHp <= 9)
        {
            losingAudio.volume = 1f;
            winningAudio.volume = 0;
            defaultAudio.volume = 0;
            //StartCoroutine(musicFade(losingAudio, 2f, 1f));
            //StartCoroutine(musicFade(winningAudio, 2f, 0f));
            //StartCoroutine(musicFade(defaultAudio, 2f, 0f));
            
        } else if (yourHp >= 11)
        {
            losingAudio.volume = 0;
            winningAudio.volume = 1f;
            defaultAudio.volume = 0;
            //StartCoroutine(musicFade(losingAudio, 2f, 0f));
            //StartCoroutine(musicFade(winningAudio, 2f, 1f));
            //StartCoroutine(musicFade(defaultAudio, 2f, 0f));
        } else
        {
            losingAudio.volume = 0;
            winningAudio.volume = 0;
            defaultAudio.volume = 1f;
            //StartCoroutine(musicFade(losingAudio, 2f, 0f));
            //StartCoroutine(musicFade(winningAudio, 2f, 0f));
            //StartCoroutine(musicFade(defaultAudio, 2f, 1f));

        }
    }

    private void StartCoroutine(IEnumerator enumerator)
    {
        throw new NotImplementedException();
    }
}
