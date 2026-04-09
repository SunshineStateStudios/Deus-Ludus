using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class AbilityCardUIScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int index;
    public GameManager gameManager;
    public string desc;

    private GameObject nameText;
    private GameObject contentsText;

    void Start()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("oh");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("orrr");
    }

    public void OnDestroy()
    {
        
    }

    public void Draw()
    {
        Debug.Log("AH");
        gameManager.DrawAbilityCard(1, index);
    }
}
