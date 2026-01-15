using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;

public class NumberCardVisuals : MonoBehaviour
{
    public ParticleSystem particles;
    private GameObject whiteOverlay;
    private Tween whiteOverlayTween;
    private RawImage whiteOverlayImage;
    private AudioSource sound;

    IEnumerator SlowTween()
    {
        yield return new WaitForSeconds(1.2f);
        whiteOverlayTween = whiteOverlayImage.DOColor(new Color(69/255, 76/255, 181/255,0.35f), .9f);
    }

    IEnumerator CardSound()
    {
        yield return new WaitForSeconds(.3f);
        sound.Play();
    }

    void Start()
    {
        whiteOverlay = GameObject.Find("Canvas/WhiteOverlay");
        whiteOverlayImage = whiteOverlay.GetComponent<RawImage>();
        sound = GetComponent<AudioSource>();

        whiteOverlayImage.color = new Color(69/255, 76/255, 181/255, 0f);

        StartCoroutine(CardSound());
        StartCoroutine(SlowTween());
    }

    public void DoTheParticle()
    {
        particles.Emit(60);

        whiteOverlayTween.Kill();
        whiteOverlayImage.color = new Color(69/255, 76/255, 181/255,1f);
        whiteOverlayTween = whiteOverlayImage.DOColor(new Color(69/255, 76/255, 181/255,0f), 1.5f);
    }
}
