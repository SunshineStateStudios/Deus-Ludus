using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    private string CurrentMenu = "Main";

    public void GoToSettings() {
        Debug.Log("settings");
    }

    public void Play()
    {
        if (!CurrentMenu.Equals("Main")) return;
        SceneManager.LoadScene(1);
    }
    public void Quit()
    {
        Debug.Log("Quit");
    }
    public void Credits()
    {
        Debug.Log("Credits");
    }
}
