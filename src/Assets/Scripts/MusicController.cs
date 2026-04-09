using Unity.VisualScripting;
using UnityEngine;

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

    public void ControlMusic(GameManager gm)
    {
        yourHp = gm.ply1.Life;
        if (yourHp >= 9)
        {
            losingAudio.volume = 1f;
            winningAudio.volume = 0f;
            defaultAudio.volume = 0f;
        } else if (yourHp <= 11)
        {
            losingAudio.volume = 0f;
            winningAudio.volume = 1f;
            defaultAudio.volume = 0f;
        } else
        {
            losingAudio.volume = 0f;
            winningAudio.volume = 0f;
            defaultAudio.volume = 1f;
        }
    }
}
