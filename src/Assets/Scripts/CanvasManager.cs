using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public GameObject inventoryObj;
    public GameObject gameManager;
    public GameObject AbilityCardUIPrefab;
    public GameObject timerLabel;
    public GameObject turnsLabel;
    public GameObject roundsLabel;
    public GameObject statusLabel;

    private bool inventoryPanelHidden = true;
    private GameManager gameManagerScript;
    private Animator canvasAnimator;
    private Animator inventoryAnimator;
    private float timePassed = 0;

    void Start()
    {
        gameManagerScript = gameManager.GetComponent<GameManager>();
        inventoryAnimator = inventoryObj.GetComponent<Animator>();
        canvasAnimator = GetComponent<Animator>();
    }

    void Update() {
        timePassed += Time.deltaTime;

        int minutes = (int) timePassed/60;
        int seconds = (int) timePassed%60;

        TMP_Text timerLabelText = timerLabel.GetComponent<TMP_Text>();

        if (seconds < 10) {
            timerLabelText.text = minutes.ToString() + ":0" + seconds.ToString();
        } else {
            timerLabelText.text = minutes.ToString() + ":" + seconds.ToString();
        }
    }

    public void SetRounds(int rounds, int turns) {
        TMP_Text roundsLabelText = roundsLabel.GetComponent<TMP_Text>();
        TMP_Text turnsLabelText = turnsLabel.GetComponent<TMP_Text>();

        roundsLabelText.text = rounds.ToString();
        turnsLabelText.text = turns.ToString();
    }

    public void SetStatus(string status) {
        /*
            Waiting for opponent
            It's the opponent's turn!
            It's your turn!
            It's not your turn yet!
            WHAT IS TAKING YOU SO LONG!
            You #*$%ing lost!
            KILL YOURSELF NOW.
        */
        
        TMP_Text statusLabelText = statusLabel.GetComponent<TMP_Text>();
        statusLabelText.text = status;
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