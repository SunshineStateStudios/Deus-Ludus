/*

Displays the description for the ability card. Applies for 2D UI elements.

Written by plexinator-9000.

*/

using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class AbilityCardDescMenu2D : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string description = "";
    public GameObject descriptionMenu;
    public TextMeshProUGUI descriptionLabel;

    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionLabel.SetText(description);
        descriptionMenu.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionMenu.SetActive(false);
    }

    void OnDestroy() {
        descriptionMenu.SetActive(false);
    }
}