using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    private string CurrentMenu = "Main";

    public void GoToSettings() {
        
    }

    public void Play()
    {
        if (!CurrentMenu.Equals("Main")) return;
        SceneManager.LoadScene(1);
    }
}
