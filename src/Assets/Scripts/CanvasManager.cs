using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public GameObject inventoryObj;
    public GameObject gameManager;
    public GameObject AbilityCardUIPrefab;

    private bool inventoryPanelHidden = true;
    private GameManager gameManagerScript;
    private Animator canvasAnimator;
    private Animator inventoryAnimator;

    void Start()
    {
        gameManagerScript = gameManager.GetComponent<GameManager>();
        inventoryAnimator = inventoryObj.GetComponent<Animator>();
        canvasAnimator = GetComponent<Animator>();
    }

    public void ResolvePanel(bool hide) {
        inventoryPanelHidden = hide;

        inventoryAnimator.StopPlayback();
        if (inventoryPanelHidden) {
            inventoryAnimator.Play("InventoryClose", 0, 0);
        } else {
            inventoryAnimator.Play("InventoryOpen", 0, 0);

            Transform inventoryList = transform.Find("InventoryPanel/Panel");
            TMP_Text cardsCount = transform.Find("InventoryPanel/CardsCount").GetComponent<TMP_Text>();
            cardsCount.text = gameManagerScript.ply1.AbilityCards.Count.ToString() + "/7";

            foreach (Transform child in inventoryList) {
                Destroy(child.gameObject);
            }

            Debug.Log("oh my gaaaa");

            for (int i = 0; i < gameManagerScript.ply1.AbilityCards.Count; i++) {
                AbilityCard card = gameManagerScript.ply1.AbilityCards[i];
                Debug.Log(i);

                GameObject uiRepresentation = Instantiate(AbilityCardUIPrefab, inventoryList);
                AbilityCardUIScript uiScript = uiRepresentation.GetComponent<AbilityCardUIScript>();

                uiScript.gameManager = gameManagerScript;
                uiScript.index = i;
            }
        }
    }

    public void ResolvePanel() {
        ResolvePanel(!inventoryPanelHidden);
    }
}