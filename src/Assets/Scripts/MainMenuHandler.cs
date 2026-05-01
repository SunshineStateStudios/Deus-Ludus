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
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundEffectsSlider;
    [SerializeField] private AudioMixer mainMenuMasterSound;
    private string CurrentMenu = "Main";

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
        mainMenuMasterSound.SetFloat("Master", Mathf.Log10(MasterVol) * 20);
    }
    public void SetMusic() {
        float MusicVol = musicSlider.value;
        mainMenuMasterSound.SetFloat("Music", Mathf.Log10(MusicVol) * 20);
    }
    public void SetSoundEffects() {
        float EffectsVol = soundEffectsSlider.value;
        mainMenuMasterSound.SetFloat("SoundEffects", Mathf.Log10(EffectsVol) * 20);
    }
}
