using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class AbilityCardUIScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int index;
    private GameManager gameManager;
    private GameObject descriptionPanel;
    private GameObject nameText;
    private GameObject contentsText;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        descriptionPanel = GameObject.Find("Canvas/InventoryContainer/Inventory/InventoryPanel/DescriptionPanel");

        nameText = descriptionPanel.transform.Find("NameText").gameObject;
        contentsText = descriptionPanel.transform.Find("ContentsText").gameObject;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Player ply = gameManager.ply1;
        AbilityCard card = ply.AbilityCards[index];

        TMP_Text nameTextComponent = nameText.GetComponent<TMP_Text>();
        TMP_Text contentsTextComponent = contentsText.GetComponent<TMP_Text>();

        nameTextComponent.text = card.name;
        contentsTextComponent.text = card.description;
        descriptionPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionPanel.SetActive(false);
    }

    public void OnDestroy()
    {
        descriptionPanel.SetActive(false);
    }

    public void Draw()
    {
        gameManager.DrawAbilityCard(1, index);
    }
}
