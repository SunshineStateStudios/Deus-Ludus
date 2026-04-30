using System.Collections;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class NumberCardVisuals : MonoBehaviour
{
    public GameObject[] Male_GodPrefabs; // indexes of the arrays correspond to models
    public GameObject[] Female_GodPrefabs;
    public Material[] PossibleMaterials; // refer to Male_GodPrefabs
    public GameObject particlesEmitter;
    public int cardSuit;
    public Transform GodPos;
    private ParticleSystem particles;
    private CanvasManager canvasManager;
    private AudioSource sound;
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
        if (transform.parent.gameObject.name.Equals("EnemyNumberCards")) GodPos.Rotate(0f,-180f,0f);

        Material chosenMaterial = PossibleMaterials[4];

        if (cardSuit >= 0 && cardSuit < PossibleMaterials.Length)
        {
            Material mat = PossibleMaterials[cardSuit];
            if (mat != null)
            {
                chosenMaterial = mat;
            }
        }

        transform.Find("Container/Card").GetComponent<MeshRenderer>().material = chosenMaterial;

        particles = particlesEmitter.GetComponent<ParticleSystem>();

        sound = GetComponent<AudioSource>();
        sound.volume = 0.25f;
        StartCoroutine(CardSound());
    }

    public void DoTheParticle()
    {
        particles.Emit(60);

        if (canvasManager == null) {
            canvasManager = GameObject.Find("Canvas").GetComponent<CanvasManager>();
        }
        canvasManager.WhiteFlash();

        /*GameObject chosenModel;
        if (Random.value >= 0.5) {
            chosenModel = Male_GodPrefabs[cardSuit-1];
        } else {
            chosenModel = Female_GodPrefabs[cardSuit-1];
        }

        GameObject godFigureInstance = Instantiate(chosenModel, GodPos.transform);
        ChosenGodModel = godFigureInstance;*/
        //GameObject suitInstance = Instantiate(suitPrefab, transform.position + new Vector3(0f,0.5f,0.1f), Quaternion.identity, transform);
        /*suitInstance.transform.rotation = Quaternion.identity;
        suitInstance.transform.Rotate(90, 0, 0);
        suitInstance.transform.position = new Vector3(0f, 0f, 0f);*/
    }
}
