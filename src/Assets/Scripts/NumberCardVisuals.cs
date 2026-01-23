using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using System;

public class NumberCardVisuals : MonoBehaviour
{
    public GameObject canvas;
    public GameObject GodCubePrefab;
    public ParticleSystem particles;
    private GameObject whiteOverlay;
    private Tween whiteOverlayTween;
    private RawImage whiteOverlayImage;
    private AudioSource sound;
    private bool canShowVisuals = false;

    IEnumerator CardSound()
    {
        yield return new WaitForSeconds(.3f);
        sound.Play();
    }

    void Start()
    {
        whiteOverlay = GameObject.Find("Main Camera/Canvas/WhiteOverlay");
        whiteOverlayImage = whiteOverlay.GetComponent<RawImage>();
        sound = GetComponent<AudioSource>();

        whiteOverlayImage.color = new Color(69f, 76f, 181f, 0f);

        StartCoroutine(CardSound());
    }

    void OnMouseEnter() {
        if (!canShowVisuals) { return; }
        canvas.SetActive(true);
    }

    void OnMouseExit() {
        canvas.SetActive(false);
    }

    public void DoTheParticle()
    {
        particles.Emit(60);

        whiteOverlayTween.Kill();
        whiteOverlayImage.color = new Color(69f, 76f, 181f, 1f);
        whiteOverlayTween = whiteOverlayImage.DOColor(new Color(69/255, 76/255, 181/255,0f), 1.5f);

        GameObject godCubeInstance = Instantiate(GodCubePrefab, transform.position + new Vector3(0f,0.5f,-.2f), Quaternion.identity, transform);
        canShowVisuals = true;
    }
}
