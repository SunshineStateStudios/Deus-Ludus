/*

Script that controls the buttons in the main menu.
Written by plexinator-9000.

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{

    // Main Panel //

    public void MainMenu_StartButton() {
        Transform playPanel = transform.Find("PlayPanel");
        Transform mainPanel = transform.Find("MainPanel");

        playPanel.gameObject.SetActive(true);
        mainPanel.gameObject.SetActive(false);
    }

    public void MainMenu_CreditsButton() {
        Transform creditsPanel = transform.Find("CreditsPanel");
        Transform mainPanel = transform.Find("MainPanel");

        creditsPanel.gameObject.SetActive(true);
        mainPanel.gameObject.SetActive(false);
    }

    public void MainMenu_QuitButton() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    // Credits Panel //

    public void CreditsMenu_BackButton() {
        Transform creditsPanel = transform.Find("CreditsPanel");
        Transform mainPanel = transform.Find("MainPanel");

        creditsPanel.gameObject.SetActive(false);
        mainPanel.gameObject.SetActive(true);
    }

    // Play Panel //

    public void PlayMenu_BackButton() {
        Transform playPanel = transform.Find("PlayPanel");
        Transform mainPanel = transform.Find("MainPanel");

        playPanel.gameObject.SetActive(false);
        mainPanel.gameObject.SetActive(true);
    }

    public void PlayMenu_PlayButton() {
        Transform playPanel = transform.Find("PlayPanel");
        Transform mainPanel = transform.Find("MainPanel");

        playPanel.gameObject.SetActive(true);
        mainPanel.gameObject.SetActive(false);

        // switch scenes
        SceneManager.LoadScene(1);
    }
}
