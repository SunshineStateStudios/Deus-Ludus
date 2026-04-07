using UnityEngine;
using TMPro;
using DG.Tweening;

// TODO: make canvasmanager non-functional if player draws or stays
// TODO: add ability/number card prompts

public class CanvasManager : MonoBehaviour
{
    public GameObject inventoryObj;
    public GameObject gameManager;
    public GameObject AbilityCardUIPrefab;
    public GameObject timerLabel;
    public GameObject turnsLabel;
    public GameObject roundsLabel;
    public GameObject statusLabel;
    public GameObject enemyHealthLabel;
    public GameObject playerHealthLabel;
    public GameObject healthBar;

    private bool inventoryPanelHidden = true;
    private bool active = true;
    private GameManager gameManagerScript;
    private Animator canvasAnimator;
    private Animator inventoryAnimator;
    private float timePassed = 0;
    private Tween healthBarTween;
    private Tween healthBarTweenPos;
    private float maxHealthbarHeight = 431.6709f;

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

    public void SetHealth(int ply1HP, int ply2HP) {
        TMP_Text enemyHealthLabelText = enemyHealthLabel.GetComponent<TMP_Text>();
        TMP_Text playerHealthLabelText = playerHealthLabel.GetComponent<TMP_Text>();

        enemyHealthLabelText.text = ply2HP.ToString();
        playerHealthLabelText.text = ply1HP.ToString();

        // Tween
        healthBarTween?.Kill();
        healthBarTweenPos?.Kill();

        float ratio = ply1HP / (float)(ply1HP + ply2HP);
        float targetHeight = ratio * maxHealthbarHeight;

        RectTransform healthBarTransform = healthBar.GetComponent<RectTransform>();
        healthBarTween = healthBarTransform.DOSizeDelta(new Vector2(healthBarTransform.sizeDelta.x, targetHeight), 0.3f).SetEase(Ease.OutQuad);
        healthBarTweenPos = healthBarTransform.DOAnchorPosY(targetHeight/2, 0.3f).SetEase(Ease.OutQuad);
    }

    public void SetActive(bool setting) {
        canvasAnimator.ResetControllerState();
        if (setting) {
            canvasAnimator.Play("Show", 0, 0);
        } else {
            canvasAnimator.Play("Hide", 0, 0);
        }

        active = setting;
    }

    public bool GetActive() {
        return active;
    }

    public void DrawCallback() {
        if (!active) return;
        StartCoroutine(gameManagerScript.EndRound(false));
    }

    public void StayCallback() {
        if (!active) return;
        StartCoroutine(gameManagerScript.EndRound(true));
    }

    public void SetRounds(int rounds, int turns) {
        TMP_Text roundsLabelText = roundsLabel.GetComponent<TMP_Text>();
        TMP_Text turnsLabelText = turnsLabel.GetComponent<TMP_Text>();

        roundsLabelText.text = rounds.ToString();
        turnsLabelText.text = turns.ToString();
    }

    public void SetStatus(string status, Color colour) {
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
        statusLabelText.color = colour;
    }

    public void SetStatus(string status) {
        SetStatus(status, new Color(1f,1f,1f,1f));
    }

    public void ResolvePanel(bool hide) {
        if (!active) return;
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