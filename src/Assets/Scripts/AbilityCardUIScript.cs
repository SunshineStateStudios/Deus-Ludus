using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class AbilityCardUIScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int index;
    public GameManager gameManager;
    public string cardDesc;
    public Sprite cardSprite;
    public string cardName;
    public GameObject InventoryDescriptionPanel;

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(CallbackPrompt);
    }

    void CallbackPrompt()
    {
        gameManager.DrawAbilityCard(1, index);
    }

    void ShowPanel(bool show) {
        if (show) {
            InventoryDescriptionPanel.SetActive(true);

            TMP_Text NameTxt = InventoryDescriptionPanel.transform.Find("Name").gameObject.GetComponent<TMP_Text>();
            TMP_Text DescriptionTxt = InventoryDescriptionPanel.transform.Find("Description").gameObject.GetComponent<TMP_Text>();
            Image Icon = InventoryDescriptionPanel.transform.Find("Icon").gameObject.GetComponent<Image>();

            NameTxt.text = cardName;
            DescriptionTxt.text = cardDesc;
            Icon.sprite = cardSprite;
            return;
        }

        InventoryDescriptionPanel.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowPanel(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ShowPanel(false);
    }
}