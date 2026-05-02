using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class MainMenuHandler : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject optionsMusic;
    public GameObject mainMenuMusic;
    public GameObject checkCredits;
    public GameObject settingsAudioTab;
    public GameObject settingsGeneralTab;
    public static bool infoBarsEnabled = true;
    private float MasterVolume = 1f;
    private float MusicVolume = 1f;
    private float SfxVolume = 1f;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundEffectsSlider;
    [SerializeField] private AudioMixer mainMenuMasterSound;
    private string CurrentMenu = "Main";

    void Start()
    {
        MasterVolume = PlayerPrefs.GetFloat("MasterVol");
        MusicVolume = PlayerPrefs.GetFloat("MusicVol");
        SfxVolume = PlayerPrefs.GetFloat("SfxVol");
        masterSlider.value = MasterVolume;
        musicSlider.value = MusicVolume;
        soundEffectsSlider.value = SfxVolume;
    }
    public void GoToSettings()
    {
        settingsMenu.SetActive(true); 
        optionsMusic.SetActive(true);
        mainMenuMusic.SetActive(false);
    }
    public void ExitSettings()
    {
        settingsMenu.SetActive(false); 
        optionsMusic.SetActive(false);
        mainMenuMusic.SetActive(true);
    }

    public void Play()
    {
        if (!CurrentMenu.Equals("Main")) return;
        SceneManager.LoadScene(1);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Credits()
    {
        checkCredits.SetActive(true);
    }
    public void exitCredits()
    {
        checkCredits.SetActive(false);
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
