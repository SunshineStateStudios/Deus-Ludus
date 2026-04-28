using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NumberCardVisuals : MonoBehaviour
{
    public GameObject canvas;
    public GameObject dashLabel;
    public GameObject damageLabel;
    public GameObject healthLabel;
    public GameObject[] Male_GodPrefabs; // indexes of the arrays correspond to models
    public GameObject[] Female_GodPrefabs;
    public int cardSuit;
    public Transform GodPos;
    public CanvasManager canvasManager;
    public ParticleSystem particles;
    private AudioSource sound;
    private bool canShowVisuals = false;
    private GameObject ChosenGodModel;

    IEnumerator CardSound()
    {
        yield return new WaitForSeconds(.3f);
        sound.Play();
    }

    public GameObject GetChosenGodModel() {
        return ChosenGodModel;
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

        if (canvasManager == null) {
            canvasManager = GameObject.Find("Canvas").GetComponent<CanvasManager>();
        }
        canvasManager.WhiteFlash();

        GameObject chosenModel;
        if (Random.value >= 0.5) {
            chosenModel = Male_GodPrefabs[cardSuit-1];
        } else {
            chosenModel = Female_GodPrefabs[cardSuit-1];
        }

        GameObject godFigureInstance = Instantiate(chosenModel, transform.position + new Vector3(0f,0.5f,0.1f), Quaternion.identity, transform);
        ChosenGodModel = godFigureInstance;
        //GameObject suitInstance = Instantiate(suitPrefab, transform.position + new Vector3(0f,0.5f,0.1f), Quaternion.identity, transform);
        canShowVisuals = true;
        
        godFigureInstance.transform.rotation = Quaternion.identity;
        godFigureInstance.transform.position = GodPos.transform.position;
        godFigureInstance.transform.position += new Vector3(0.8f, 0f, 0f);
        /*suitInstance.transform.rotation = Quaternion.identity;
        suitInstance.transform.Rotate(90, 0, 0);
        suitInstance.transform.position = new Vector3(0f, 0f, 0f);*/
    }
}
