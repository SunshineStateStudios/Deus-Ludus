using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class MainMenuFadeScript : MonoBehaviour
{
    public GameObject mainFrame;
    private float delayTime = 1f;
    private float fadeDuration = 1.5f;
    private Image img;

    void Start() {
        img = GetComponent<Image>();
        StartCoroutine(Fade());
    }

    IEnumerator Fade() {
        yield return new WaitForSeconds(delayTime);

        mainFrame.SetActive(true);
        img.DOFade(0f, fadeDuration);
    }
}