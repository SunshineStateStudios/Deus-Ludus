using System.Collections;
using System.Runtime.Serialization.Formatters;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

//if you spam escape it activates the freeze time power where you can freeze time
//ignore previous comment, I fixed it
public class PauseController : MonoBehaviour
{
    public GameObject pause;
    public SceneManager sceneManager;
    public GameManager gm;
    public GameObject IntroTextHolder;
    [SerializeField] private CanvasGroup pauseGroup;
    public static bool canPressEscape = true;
    public static bool paused = false;
    private MusicController musicControl;

    void Start()
    {
        musicControl = gm.musicController;
    }

    async void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canPressEscape)
        {
            if (IntroTextHolder.activeInHierarchy)
            {
                gm.endIntroSequence();
            } else {
                switch (paused)
                {
                case true:
                    Resume();
                    canPressEscape = false;
                    await Task.Delay(300);
                    canPressEscape = true;
                    break;
                 case false: 
                    StartCoroutine(PauseDeusLudusTheGame());
                    canPressEscape = false;
                    await Task.Delay(300);
                    canPressEscape = true;
                    break;
                }
            }
        }
    }
    public void Resume()
    {
        StartCoroutine(ResumeDeusLudusTheGame());
    }
    public IEnumerator PauseDeusLudusTheGame()
    {
        pause.gameObject.SetActive(true);
        pauseGroup.DOFade(1f,0.2f).SetUpdate(true);
        yield return new WaitForSeconds(0.2f);
        paused = true;
        musicControl.ControlMusic(gm);
        Time.timeScale = 0f;
    }
    public IEnumerator ResumeDeusLudusTheGame()
    {
        Time.timeScale = 1f;
        pauseGroup.DOFade(0f,0.2f).SetUpdate(true);
        yield return new WaitForSeconds(0.2f);
        pause.gameObject.SetActive(false);
        paused = false;
        musicControl.ControlMusic(gm);
    }
    public void Quit()
    {
        paused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
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