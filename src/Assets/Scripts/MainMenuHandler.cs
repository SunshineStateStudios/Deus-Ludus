using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Rendering;
using DG.Tweening;
using System.Collections;

public class MainMenuHandler : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject mainMenu;
    public GameObject optionsMusic;
    public GameObject playPrompt;
    public GameObject mainMenuMusic;
    public GameObject checkCredits;
    public GameObject settingsAudioTab;
    public GameObject settingsGeneralTab;
    public GameObject UISoundsObject;
    public static bool infoBarsEnabled = true;
    private float MasterVolume = 1f;
    private float MusicVolume = 1f;
    private float SfxVolume = 1f;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundEffectsSlider;
    [SerializeField] private AudioMixer mainMenuMasterSound;
    private string CurrentMenu = "Main";
    private Animator animator;

    void Start()
    {
        MasterVolume = PlayerPrefs.GetFloat("MasterVol");
        MusicVolume = PlayerPrefs.GetFloat("MusicVol");
        SfxVolume = PlayerPrefs.GetFloat("SfxVol");
        masterSlider.value = MasterVolume;
        musicSlider.value = MusicVolume;
        soundEffectsSlider.value = SfxVolume;
        animator = GetComponent<Animator>();
    }
    public void GoToSettings()
    {
        settingsMenu.SetActive(true);
        mainMenu.SetActive(false);
        
        AudioSource optionsMusicSource = optionsMusic.GetComponent<AudioSource>();
        AudioSource mainMenuMusicSource = mainMenuMusic.GetComponent<AudioSource>();

        optionsMusicSource.DOKill();
        mainMenuMusicSource.DOKill();

        optionsMusicSource.DOFade(1f, 4f);
        mainMenuMusicSource.DOFade(0f, 4f);
    }
    public void ExitSettings()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
        
        AudioSource optionsMusicSource = optionsMusic.GetComponent<AudioSource>();
        AudioSource mainMenuMusicSource = mainMenuMusic.GetComponent<AudioSource>();

        optionsMusicSource.DOKill();
        mainMenuMusicSource.DOKill();

        optionsMusicSource.DOFade(0f, 4f);
        mainMenuMusicSource.DOFade(1f, 4f);
    }

    public void Play()
    {
        if (!CurrentMenu.Equals("Main")) return;
        playPrompt.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    public void Credits()
    {
        checkCredits.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void exitCredits()
    {
        checkCredits.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void exitPrompt() {
        playPrompt.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void acceptPrompt() {
        StartCoroutine(transition());

        GameObject yehButton = playPrompt.transform.Find("Panel/yeh").gameObject;
        GameObject nahButton = playPrompt.transform.Find("Panel/nah").gameObject;

        Destroy(yehButton.GetComponent<GenericUISounds>());
        Destroy(nahButton.GetComponent<GenericUISounds>());

        yehButton.GetComponent<Button>().interactable = false;
        nahButton.GetComponent<Button>().interactable = false;

        animator.Play("GodPromptChosen", 0, 0);
        mainMenuMusic.SetActive(false);
        UISoundsObject.GetComponents<AudioSource>()[6].Play();
    }
    IEnumerator transition() {
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene(1);
    }
    public void checkAudio()
    {
        settingsAudioTab.SetActive(true);
        settingsGeneralTab.SetActive(false);
    }
    public void checkGeneral()
    {
        settingsAudioTab.SetActive(false);
        settingsGeneralTab.SetActive(true);
    }
    public void SetMaster() {
        float MasterVol = masterSlider.value;
        MasterVolume = masterSlider.value;
        mainMenuMasterSound.SetFloat("Master", Mathf.Log10(MasterVol) * 20);
        PlayerPrefs.SetFloat("MasterVol", masterSlider.value);
    }
    public void SetMusic() {
        float MusicVol = musicSlider.value;
        MusicVolume = masterSlider.value;
        mainMenuMasterSound.SetFloat("Music", Mathf.Log10(MusicVol) * 20);
        PlayerPrefs.SetFloat("MusicVol", musicSlider.value);
    }
    public void SetSoundEffects() {
        float EffectsVol = soundEffectsSlider.value;
        SfxVolume = masterSlider.value;
        mainMenuMasterSound.SetFloat("SoundEffects", Mathf.Log10(EffectsVol) * 20);
        PlayerPrefs.SetFloat("SfxVol", soundEffectsSlider.value);
    }
    public void ToggleInfoBars() {
        if (infoBarsEnabled)
        {
            infoBarsEnabled = false;
            InfoBarController.infoBarsEnabled = false;
        } else
        {
            infoBarsEnabled = true;
            InfoBarController.infoBarsEnabled = true;
        }
    }
}
