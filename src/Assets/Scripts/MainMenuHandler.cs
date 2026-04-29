using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject optionsMusic;
    public GameObject mainMenuMusic;
    public GameObject checkCredits;
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
}
