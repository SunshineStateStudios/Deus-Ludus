using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NumberCardVisuals : MonoBehaviour
{
    public GameObject canvas;
    public GameObject dashLabel;
    public GameObject damageLabel;
    public GameObject healthLabel;
    public GameObject GodFigurePrefab;
    public CanvasManager canvasManager;
    public ParticleSystem particles;
    private AudioSource sound;
    private bool canShowVisuals = false;

    IEnumerator CardSound()
    {
        yield return new WaitForSeconds(.3f);
        sound.Play();
    }

    void Start()
    {
        sound = GetComponent<AudioSource>();
        sound.volume = 0.25f;
        StartCoroutine(CardSound());
    }

    void OnMouseEnter() {
        if (!canShowVisuals) { return; }
        dashLabel.SetActive(true);
        healthLabel.SetActive(true);
        damageLabel.SetActive(true);
    }

    void OnMouseExit() {
        dashLabel.SetActive(false);
        healthLabel.SetActive(false);
        damageLabel.SetActive(false);
    }

    public void DoTheParticle()
    {
        particles.Emit(60);
        canvas.SetActive(true);
        //canvasManager.WhiteFlash();

        GameObject godFigureInstance = Instantiate(GodFigurePrefab, transform.position + new Vector3(0f,0.5f,0.1f), Quaternion.identity, transform);
        canShowVisuals = true;
        godFigureInstance.transform.rotation = Quaternion.identity;
    }
}
