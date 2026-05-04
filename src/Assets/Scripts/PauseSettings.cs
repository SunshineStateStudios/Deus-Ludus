using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Rendering;
using DG.Tweening;
using System.Collections;

public class PauseSettings : MonoBehaviour
{
    public GameObject settingsMenu;
    private float MasterVolume = 1f;
    private float MusicVolume = 1f;
    private float SfxVolume = 1f;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundEffectsSlider;
    [SerializeField] private AudioMixer mainMenuMasterSound;

    /// /////////////////////////////////////////
    void Start()
    {
        MasterVolume = PlayerPrefs.GetFloat("MasterVol");
        MusicVolume = PlayerPrefs.GetFloat("MusicVol");
        SfxVolume = PlayerPrefs.GetFloat("SfxVol");
        masterSlider.value = MasterVolume;
        musicSlider.value = MusicVolume;
        soundEffectsSlider.value = SfxVolume;
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
        if (MainMenuHandler.infoBarsEnabled)
        {
            MainMenuHandler.infoBarsEnabled = false;
            InfoBarController.infoBarsEnabled = false;
        } else
        {
            MainMenuHandler.infoBarsEnabled = true;
            InfoBarController.infoBarsEnabled = true;
        }
    }
    public void EnterSettings()
    {
        settingsMenu.SetActive(true);
    }
    public void ExitSettings()
    {
        settingsMenu.SetActive(false);
    }
    }
