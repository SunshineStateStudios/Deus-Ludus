using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NumberCardVisuals : MonoBehaviour
{
    public GameObject canvas;
    public GameObject GodCubePrefab;
    public ParticleSystem particles;
    private WhiteFlash whiteOverlayScript;
    private AudioSource sound;
    private bool canShowVisuals = false;

    IEnumerator CardSound()
    {
        yield return new WaitForSeconds(.3f);
        sound.Play();
        whiteOverlayScript = GameObject.Find("Canvas/WhiteOverlay").GetComponent<WhiteFlash>(); // need to do delay :(
    }

    void Start()
    {
        sound = GetComponent<AudioSource>();

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
        whiteOverlayScript.Flash(new Color(1f,1f,1f,0.5f));

        GameObject godCubeInstance = Instantiate(GodCubePrefab, transform.position + new Vector3(0f,0.5f,-0.2f), Quaternion.identity, transform);
        canShowVisuals = true;
        godCubeInstance.transform.rotation = Quaternion.identity;
    }
}
