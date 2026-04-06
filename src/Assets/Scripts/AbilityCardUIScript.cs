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
    //private GameObject descriptionPanel;

    void Start()
    {
        //descriptionPanel = GameObject.Find("Canvas/InventoryContainer/Inventory/InventoryPanel/DescriptionPanel");

        TMP_Text nameTextComponent = transform.Find("NameLabel").GetComponent<TMP_Text>();
        nameTextComponent.text = gameManager.ply1.AbilityCards[index].name;

        //nameText = descriptionPanel.transform.Find("NameText").gameObject;
        //contentsText = descriptionPanel.transform.Find("ContentsText").gameObject;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Player ply = gameManager.ply1;
        AbilityCard card = ply.AbilityCards[index];

        TMP_Text nameTextComponent = nameText.GetComponent<TMP_Text>();
        TMP_Text contentsTextComponent = contentsText.GetComponent<TMP_Text>();

        nameTextComponent.text = card.name;
        contentsTextComponent.text = card.description;
        //descriptionPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        /*if (descriptionPanel == null) return;
        descriptionPanel.SetActive(false);*/
    }

    public void OnDestroy()
    {
        /*if (descriptionPanel == null) return;
        descriptionPanel.SetActive(false);*/
    }

    public void Draw()
    {
        gameManager.DrawAbilityCard(1, index);
    }
}
