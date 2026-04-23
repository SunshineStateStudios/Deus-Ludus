using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    public GameObject pause;
    public float fadeTime = 10f;
    [SerializeField] private CanvasGroup pauseFade;
    public bool paused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                PauseDeusLudusTheGame();
            } else
            {
                ResumeDeusLudusTheGame();
            }
        }
    }
    public void ResumeDeusLudusTheGame()
    {
        pause.gameObject.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
        //StartCoroutine(DoFadeOut_Resume());
    }
    public void PauseDeusLudusTheGame()
    {
        pause.gameObject.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
        //StartCoroutine(DoFadeIn_Pause());
    }
    /*IEnumerator DoFadeOut_Resume()
    {
        float timePassed = 0;
        while(pauseFade.alpha > 0)
        {
            timePassed += Time.deltaTime;
            pauseFade.alpha = Mathf.Clamp01(1.0f - (timePassed / fadeTime));
            if (pauseFade.alpha == 0)
            {
                Debug.Log("resumed");
                pause.gameObject.SetActive(false);
            }
            yield return null;
        }

        yield return null;
    }
    IEnumerator DoFadeIn_Pause()
    {
        float timePassed = 0;
        while(pauseFade.alpha < 1)
        {
            timePassed += Time.deltaTime;
            pauseFade.alpha = Mathf.Clamp01(0f + (timePassed / fadeTime));
            if (pauseFade.alpha == 1)
            {
                Debug.Log("paused");
                Time.timeScale = 0f;
            }
            yield return null;
        }

        yield return null;
    }*/
}
