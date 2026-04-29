using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    public GameObject settingsMenu;
    private string CurrentMenu = "Main";

    public void GoToSettings()
    {
        settingsMenu.SetActive(true); 
        Debug.Log("settings");
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
        Debug.Log("Credits");
    }
}
