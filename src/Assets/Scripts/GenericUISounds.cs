using UnityEngine;
using UnityEngine.EventSystems;

public class GenericUISounds : MonoBehaviour,
    IPointerEnterHandler,
    IPointerClickHandler
{
    public GameObject UISoundsObject;
    public bool UseAltConfirmSound = false;
    public bool PlayConfirmSound = true;

    void PlaySound(int index) {
        UISoundsObject.GetComponents<AudioSource>()[index].Play();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlaySound(2);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!PlayConfirmSound) return;
        int chosenIndex = 0;
        if (UseAltConfirmSound) chosenIndex = 1;

        PlaySound(chosenIndex);
    }
}