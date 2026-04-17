using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;

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
    public GameObject playerADLabel;
    public GameObject enemyADLabel;
    public GameObject playerTotalLabel;
    public GameObject enemyTotalLabel;
    public GameObject healthBar;
    public GameObject AbilityCardPromptPanel;
    public GameObject NumberCardPromptPanel;
    public GameObject NumberCardButtonPrompt;
    public GameObject InventoryDescriptionPanel;

    private bool inventoryPanelHidden = true;
    private bool active = true;
    private GameManager gameManagerScript;
    private Animator canvasAnimator;
    private Animator inventoryAnimator;
    private float timePassed = 0;
    private Tween healthBarTween;
    private Tween healthBarTweenPos;
    private Tween whiteFlashTween;
    private float maxHealthbarHeight = 431.6709f;
    private Animator plyTotalLabelAnimator;
    private bool alreadyFlourished = false;

    void Start()
    {
        gameManagerScript = gameManager.GetComponent<GameManager>();
        inventoryAnimator = inventoryObj.GetComponent<Animator>();
        canvasAnimator = GetComponent<Animator>();
        plyTotalLabelAnimator = playerTotalLabel.GetComponent<Animator>();
    }

    public void ShowDrawButton(bool visibility)
    {
        GameObject drawButtonObject = transform.Find("OptionsPanel/DrawButton").gameObject;
        drawButtonObject.SetActive(visibility);
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

    void FlourishTotal() {
        AudioSource flourishSound = GetComponents<AudioSource>()[0];
        flourishSound.Play();

        plyTotalLabelAnimator.Play("Flourish", 0, 0);
    }

    public void WhiteFlash() {
        whiteFlashTween?.Kill();
        
        Image flashImg = transform.Find("White").gameObject.GetComponent<Image>();

        flashImg.color = new Color(1f,1f,1f,0.5f);
        whiteFlashTween = flashImg.DOFade(0f, 1f);
    }

    IEnumerator UpdateText(Player ply1, Player ply2, bool hideFirstCard) {
        yield return new WaitForSeconds(2f);

        TMP_Text plyTotalText = playerTotalLabel.GetComponent<TMP_Text>();
        TMP_Text enemyTotalText = enemyTotalLabel.GetComponent<TMP_Text>();
        TMP_Text ply1ADLabel = playerADLabel.GetComponent<TMP_Text>();
        TMP_Text ply2ADLabel = enemyADLabel.GetComponent<TMP_Text>();

        int total = ply1.BlackjackTotal(gameManagerScript.BlackjackThreshold, false);
        if (total > gameManagerScript.BlackjackThreshold) {
            if (!alreadyFlourished) {
                alreadyFlourished = true;
                FlourishTotal();
            }
        } else {
            if (alreadyFlourished) alreadyFlourished = false;
        }

        int ply1AttackTotal = ply1.TotalDamage(false);
        int ply1HealthTotal = ply1.TotalHealth(gameManagerScript.BlackjackThreshold, false);
        int ply2AttackTotal = ply2.TotalDamage(true);
        int ply2HealthTotal = ply2.TotalHealth(gameManagerScript.BlackjackThreshold, true);

        ply1ADLabel.text = "A: <color=#d62d2dff>" + ply1AttackTotal.ToString() + "</color> / D: <color=#2d6ed6ff>" + ply1HealthTotal.ToString() + "</color>";
        ply2ADLabel.text = "A: <color=#d62d2dff>? + " + ply2AttackTotal.ToString() + "</color> / D: <color=#2d6ed6ff>? + " + ply2HealthTotal.ToString() + "</color>";

        plyTotalText.text = "Total: " + total.ToString() + "/" + gameManagerScript.BlackjackThreshold.ToString();
        
        if (hideFirstCard) {
            enemyTotalText.text = "Total: ? + " + ply2.BlackjackTotal(gameManagerScript.BlackjackThreshold, true).ToString() + "/" + gameManagerScript.BlackjackThreshold.ToString();
        } else {
            enemyTotalText.text = "Total: " + ply2.BlackjackTotal(gameManagerScript.BlackjackThreshold, false).ToString() + "/" + gameManagerScript.BlackjackThreshold.ToString();
        }
    }

    public void CalculateText(Player ply1, Player ply2, bool hideFirstCard = true) { // Updating the text requires a delay
        StartCoroutine(UpdateText(ply1, ply2, hideFirstCard));
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

    public void RecountAbilityCardCount() {
        int count = 0;
        for (int i = 0; i < gameManagerScript.ply1.AbilityCards.Count; i++) {
            AbilityCard card = gameManagerScript.ply1.AbilityCards[i];
            if (card.Drawn) continue;
            count++;
        }

        Transform inventoryList = transform.Find("InventoryPanel/Panel");
        TMP_Text cardsCount = transform.Find("InventoryPanel/CardsCount").GetComponent<TMP_Text>();

        cardsCount.text = count.ToString() + "/5";
    }

    public void RebuildInventoryPanel() {
        RecountAbilityCardCount();

        Transform inventoryList = transform.Find("InventoryPanel/Panel");

        foreach (Transform child in inventoryList) {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < gameManagerScript.ply1.AbilityCards.Count; i++) {
            AbilityCard card = gameManagerScript.ply1.AbilityCards[i];
            if (card.Drawn) continue;

            GameObject uiRepresentation = Instantiate(AbilityCardUIPrefab, inventoryList);
            uiRepresentation.name = i.ToString();
                
            TMP_Text nameLabel = uiRepresentation.transform.Find("Name").gameObject.GetComponent<TMP_Text>();
            nameLabel.text = card.name;

            Image img = uiRepresentation.transform.Find("Image").gameObject.GetComponent<Image>();
            img.sprite = card.icon;
            img.preserveAspect = true;

            AbilityCardUIScript uiScript = uiRepresentation.GetComponent<AbilityCardUIScript>();

            uiScript.gameManager = gameManagerScript;
            uiScript.index = i;
            uiScript.cardDesc = card.description;
            uiScript.cardName = card.name;
            uiScript.cardSprite = card.icon;
            uiScript.InventoryDescriptionPanel = InventoryDescriptionPanel;
        }
    }

    public void HidePrompt() {
        NumberCardPromptPanel.SetActive(false);
        AbilityCardPromptPanel.SetActive(false);
    }

    public void ShowPrompt(string type, Player ply, PromptAbilityCard card) {
        if (type.Equals("number")) {
            NumberCardPromptPanel.SetActive(true);

            Transform promptContents = NumberCardPromptPanel.transform.Find("Contents");
            TMP_Text titleText = NumberCardPromptPanel.transform.Find("Text (TMP)").gameObject.GetComponent<TMP_Text>();

            titleText.text = "Choose a number card... (" + card.name + ")";

            for (int i = 0; i < ply.NumberCards.Count; i++) {
                NumberCard numbCard = ply.NumberCards[i];
                GameObject cardRepresentation = Instantiate(NumberCardButtonPrompt, promptContents);

                TMP_Text valueLabel = cardRepresentation.transform.Find("ValueLabel").gameObject.GetComponent<TMP_Text>();
                TMP_Text damageLabel = cardRepresentation.transform.Find("DamageLabel").gameObject.GetComponent<TMP_Text>();
                TMP_Text healthLabel = cardRepresentation.transform.Find("HealthLabel").gameObject.GetComponent<TMP_Text>();

                valueLabel.text = numbCard.Value.ToString();
                damageLabel.text = numbCard.Damage.ToString();
                healthLabel.text = numbCard.Health.ToString();

                NumberCardPromptScript promptScript = cardRepresentation.GetComponent<NumberCardPromptScript>();
                promptScript.cardIndex = i;
                promptScript.card = card;
                promptScript.gameManager = gameManagerScript;
            }

            return;
        }

        AbilityCardPromptPanel.SetActive(true);

        Transform promptContentsAbility = AbilityCardPromptPanel.transform.Find("Contents");
        TMP_Text titleTextAbility = AbilityCardPromptPanel.transform.Find("Text (TMP)").gameObject.GetComponent<TMP_Text>();

        titleTextAbility.text = "Choose an ability card... (" + card.name + ")";

        for (int i = 0; i < ply.AbilityCards.Count; i++) {
            AbilityCard abilityCard = ply.AbilityCards[i];
            if (abilityCard.Drawn) continue;

            GameObject cardRepresentation = Instantiate(AbilityCardUIPrefab, promptContentsAbility);
            Destroy(cardRepresentation.GetComponent<AbilityCardUIScript>());

            TMP_Text nameLabel = cardRepresentation.transform.Find("Name").gameObject.GetComponent<TMP_Text>();
            nameLabel.text = abilityCard.name.ToString();

            cardRepresentation.AddComponent<AbilityCardPromptScript>();

            AbilityCardPromptScript promptScript = cardRepresentation.GetComponent<AbilityCardPromptScript>();
            promptScript.cardIndex = i;
            promptScript.card = card;
            promptScript.gameManager = gameManagerScript;
        }
    }

    public bool IsInventoryPanelHidden() {
        return inventoryPanelHidden;
    }

    public void ResolvePanel(bool hide) {
        if (!active) return;
        inventoryPanelHidden = hide;

        inventoryAnimator.StopPlayback();
        if (inventoryPanelHidden) {
            inventoryAnimator.Play("InventoryClose", 0, 0);
        } else {
            inventoryAnimator.Play("InventoryOpen", 0, 0);
            RebuildInventoryPanel();
        }
    }

    public void ResolvePanel() {
        ResolvePanel(!inventoryPanelHidden);
    }
}