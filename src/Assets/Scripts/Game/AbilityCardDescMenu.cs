/*

Displays the description for the ability card.

Written by plexinator-9000.

*/

using UnityEngine;
using TMPro;

public class AbilityCardDescMenu : MonoBehaviour
{
    public string description = "";
    public GameObject descriptionMenu;
    public TextMeshProUGUI descriptionLabel;

    void OnMouseEnter() {
        descriptionLabel.SetText(description);
        descriptionMenu.SetActive(true);
    }

    void OnMouseExit() {
        descriptionMenu.SetActive(false);
    }
}
