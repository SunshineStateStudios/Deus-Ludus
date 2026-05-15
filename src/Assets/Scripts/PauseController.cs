using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [Header("References")]
    public GameObject pause;
    public GameManager gm;
    public GameObject IntroTextHolder;

    [SerializeField] private CanvasGroup pauseGroup;

    public static bool canPressEscape = true;
    public static bool paused = false;

    private MusicController musicControl;
    private bool debounce;

    void Start()
    {
        if (gm != null)
        {
            musicControl = gm.musicController;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canPressEscape && !debounce)
        {
            StartCoroutine(HandleEscape());
        }
    }

    private IEnumerator HandleEscape()
    {
        debounce = true;
        canPressEscape = false;

        // Prevent MissingReferenceException
        if (IntroTextHolder != null && IntroTextHolder.activeInHierarchy)
        {
            gm.endIntroSequence();
        }
        else
        {
            if (paused)
            {
                Resume();
            }
            else
            {
                StartCoroutine(PauseDeusLudusTheGame());
            }
        }

        // Use realtime because pause menus use Time.timeScale = 0
        yield return new WaitForSecondsRealtime(0.3f);

        debounce = false;
        canPressEscape = true;
    }

    public void Resume()
    {
        StartCoroutine(ResumeDeusLudusTheGame());
    }

    public IEnumerator PauseDeusLudusTheGame()
    {
        if (pause == null || pauseGroup == null)
            yield break;

        pause.SetActive(true);

        pauseGroup.DOFade(1f, 0.2f).SetUpdate(true);

        yield return new WaitForSecondsRealtime(0.2f);

        paused = true;

        if (musicControl != null)
        {
            musicControl.ControlMusic(gm);
        }

        Time.timeScale = 0f;
    }

    public IEnumerator ResumeDeusLudusTheGame()
    {
        Time.timeScale = 1f;

        if (pauseGroup != null)
        {
            pauseGroup.DOFade(0f, 0.2f).SetUpdate(true);
        }

        yield return new WaitForSecondsRealtime(0.2f);

        if (pause != null)
        {
            pause.SetActive(false);
        }

        paused = false;

        if (musicControl != null)
        {
            musicControl.ControlMusic(gm);
        }
    }

    public void Quit()
    {
        paused = false;
        Time.timeScale = 1f;

        SceneManager.LoadScene(0);
    }
}