using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public Player ply1;
    public Player ply2;
    public Deck deck;
    public GamePhase phase;

    public GameObject numberCard;
    public GameObject abilityCard;
    public GameObject playerAbilityCards;
    public GameObject enemyAbilityCards;
    public GameObject playerNumberCards;
    public GameObject enemyNumberCards;
    public GameObject canvasObject;
    public AudioClip attackGodCubeClip;
    public GameObject inventoryPanelScroll;
    public GameObject abilityCardUIPrefab;
    public int BlackjackThreshold = 21;

    // Attributes for the drawn/stayed overlay
    public GameObject notificationUI;
    public GameObject YouDefense;
    public GameObject YouAttack;
    public GameObject OppDefense;
    public GameObject OppAttack;
    public GameObject TempYouHp;
    public GameObject TempOppHp;
    public GameObject cardPrompt;

    private AudioSource decidedSound;
    private AudioSource attackGodCubeSound;
    // Misc
    private GameObject inventoryButton;
    private GameObject inventoryPanel;
    private GameObject stayButton;
    private GameObject drawButton;
    private GameObject notificationUIText;
    private GameObject progressText;

    private Animator canvasAnimator;
    private CanvasManager canvasManager;
    private bool inventoryPanelHidden = false;
    private List<AbilityCard> abilityCardList;

    void Start()
    {
        AudioSource[] sources = GetComponents<AudioSource>();
        decidedSound = sources[0];
        attackGodCubeSound = sources[1];

        inventoryButton = GameObject.Find("Canvas/InventoryButton");
        inventoryPanel = GameObject.Find("Canvas/InventoryPanel");
        stayButton = GameObject.Find("Canvas/StayButton");
        drawButton = GameObject.Find("Canvas/DrawNumberCardButton");
        notificationUIText = GameObject.Find("Canvas/Notification/Label");
        progressText = GameObject.Find("Canvas/ProgressText");
        canvasAnimator = canvasObject.GetComponent<Animator>();
        canvasManager = canvasObject.GetComponent<CanvasManager>();
        YouDefense = GameObject.Find("Canvas/YouDefense");
        YouAttack = GameObject.Find("Canvas/YouAttack");
        OppDefense = GameObject.Find("Canvas/OppDefense");
        OppAttack = GameObject.Find("Canvas/OppAttack");
        TempYouHp = GameObject.Find("Canvas/TempYouHP");
        TempOppHp = GameObject.Find("Canvas/TempOppHP");

        ply1 = new Player();
        ply2 = new Player();
        deck = new Deck();

        StartCoroutine(StartNewRound());
        StartCoroutine(ShowUIButtons());
    }
    

    public void UpdateProgressText()
    {
        TMP_Text progressTxtObj = progressText.GetComponent<TMP_Text>();
        progressTxtObj.text = "card total: <b><color=#8391F1><b>" + ply1.BlackjackTotal(BlackjackThreshold, false).ToString() + "</color>\nthreshold: <b><color=#8391F1><b>" + BlackjackThreshold.ToString() + "</color></b>";
    }

    public void UpdateDrawNumberCardText()
    {
        GameObject rawimg = drawButton.transform.Find("RawImage").gameObject;
        TextFade rawimgScript = drawButton.GetComponent<TextFade>();
        RawImage rawimgComponent = rawimg.GetComponent<RawImage>();
            
        rawimgScript.Contents = "Draw a number card and end your turn.";
        rawimgComponent.color = new Color(1f,1f,1f,1f);
        canvasManager.SetCanDrawNumberCard(false);

        if (ply1.NumberCards.Count > 5)
        {
            rawimgScript.Contents = "You have more than 6 number cards drawn! You cannot draw anymore.";
            canvasManager.SetCanDrawNumberCard(true);
            rawimgComponent.color = new Color(1,0,0,1f);
        } else if (ply1.BlackjackTotal(BlackjackThreshold, false) > BlackjackThreshold)
        {
            rawimgScript.Contents = "Busted (went over " + BlackjackThreshold.ToString() + ")! Your cards' combative capibilities will be severely tarnished.";
            rawimgComponent.color = new Color(120f/255f,0,0,1f);
        }
    }

    void RebuildAbilityCardPool()
    {
        abilityCardList = new List<AbilityCard>();

        abilityCardList.Add(new AbilityDeath());
        abilityCardList.Add(new AbilityDevil());
        abilityCardList.Add(new AbilityEmperor());
        abilityCardList.Add(new AbilityEmpress());
        abilityCardList.Add(new AbilityHierophant());
        abilityCardList.Add(new AbilityJudgement());
        abilityCardList.Add(new AbilityJustice());
        abilityCardList.Add(new AbilityLovers());
        abilityCardList.Add(new AbilityMoon());
        abilityCardList.Add(new AbilityStar());
        abilityCardList.Add(new AbilityStrength());
        abilityCardList.Add(new AbilitySun());
        abilityCardList.Add(new AbilityTemperance());
        abilityCardList.Add(new AbilityWheelOfFortune());
        abilityCardList.Add(new AbilityWorld());
        abilityCardList.Add(new AbilityTower());
        abilityCardList.Add(new AbilityChariot());
        abilityCardList.Add(new AbilityHighPriestess());
        abilityCardList.Add(new AbilityMagician());
        abilityCardList.Add(new AbilityHermit());
        abilityCardList.Add(new AbilityHangedMan());

        // Scramble the list!
        int n = abilityCardList.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i+1);
            AbilityCard card = abilityCardList[i];
            abilityCardList[i] = abilityCardList[j];
            abilityCardList[j] = card;
        }
    }

    public AbilityCard GivePlayerAbilityCard(Player ply)
    {
        if (abilityCardList.Count > 0) {
            AbilityCard chosenCard = abilityCardList.ElementAt(0);
            ply.AbilityCards.Add(chosenCard);
            abilityCardList.Remove(chosenCard);

            return chosenCard;
        }

        return null;
    }

    IEnumerator ShowUIButtons()
    {
        yield return new WaitForSeconds(3.5f);
        canvasAnimator.Play("In", 0, 0);
    }

    IEnumerator StartNewRound()
    {
        TMP_Text YouHpTxt = TempYouHp.GetComponent<TMP_Text>();
        TMP_Text OppHpTxt = TempOppHp.GetComponent<TMP_Text>();
        YouHpTxt.text = ply1.Life.ToString();
        OppHpTxt.text = ply2.Life.ToString();
        
        ply1.NumberCards.Clear();
        ply2.NumberCards.Clear();

        RebuildAbilityCardPool();
        GivePlayerAbilityCard(ply1);
        GivePlayerAbilityCard(ply1);
        GivePlayerAbilityCard(ply1);
        GivePlayerAbilityCard(ply1);
        GivePlayerAbilityCard(ply1);
        GivePlayerAbilityCard(ply1);
        GivePlayerAbilityCard(ply1);
        GivePlayerAbilityCard(ply1);
        GivePlayerAbilityCard(ply1);
        GivePlayerAbilityCard(ply1);
        GivePlayerAbilityCard(ply1);
        GivePlayerAbilityCard(ply1);
        GivePlayerAbilityCard(ply1);

        GivePlayerAbilityCard(ply2);
        GivePlayerAbilityCard(ply2);
        GivePlayerAbilityCard(ply2);
        GivePlayerAbilityCard(ply2);

        RebuildInventoryPanel();

        for (int i = 0; i < 2; i++)
        {
            DrawNumberCard(ply1);
            yield return new WaitForSeconds(0.5f);
        }

        for (int i = 0; i < 2; i++)
        {
            DrawNumberCard(ply2);
            yield return new WaitForSeconds(0.5f);
        }

        phase = GamePhase.PlayerTurn;

        TMP_Text progressTxtObj = progressText.GetComponent<TMP_Text>();
        progressTxtObj.text = "card total: <b><color=#8391F1><b>" + ply1.BlackjackTotal(BlackjackThreshold, false).ToString() + "</color>\nthreshold: <b><color=#8391F1><b>" + BlackjackThreshold.ToString() + "</color></b>";

        if (ply1.BlackjackTotal(BlackjackThreshold, false) >= BlackjackThreshold)
        {
            drawButton.SetActive(false);
        } else
        {
            GameObject rawimg = drawButton.transform.Find("RawImage").gameObject;
            RawImage rawimgComponent = rawimg.GetComponent<RawImage>();
            rawimgComponent.color = new Color(1f,1f,1f,1f);
        }

        UpdateDrawNumberCardText();
        canvasManager.SetFunctionality(true);
    }

    void ShowNotification(string text, Color colour)
    {
        TMP_Text notificationTxt = notificationUIText.GetComponent<TMP_Text>();
        notificationTxt.text = text;
        decidedSound.Play();
        Animator notificationUIAnimator = notificationUI.GetComponent<Animator>();
        notificationUIAnimator.Play("Notification_Popup", 0, 0);
        notificationTxt.color = colour;
    }

    public IEnumerator EndRound(bool didStay)
    {
        TMP_Text progressTxtObj = progressText.GetComponent<TMP_Text>(); 

        progressTxtObj.text = "card total: <b><color=#8391F1><b>" + ply1.BlackjackTotal(BlackjackThreshold, false).ToString() + "</color>\nthreshold: <b><color=#8391F1><b>" + BlackjackThreshold.ToString() + "</color></b>";

        progressText.SetActive(false);
        
        string notificationText = "DRAWN";
        if (didStay)
        {
            notificationText = "STAYED";
        }

        ShowNotification(notificationText, new Color(0, 49f/255f, 188f/255f, 1f));
        canvasAnimator.Play("Out", 0, 0);
        canvasManager.SetFunctionality(false);

        yield return new WaitForSeconds(1f);

        if (!didStay)
        {
            DrawNumberCard(ply1);
        }

        yield return new WaitForSeconds(1f);

        progressText.SetActive(true);
        progressTxtObj.text = "It's the opponent's turn!";
        phase = GamePhase.AITurn;

        yield return new WaitForSeconds(1f);

        bool draw = false;

        for (int i = ply2.AbilityCards.Count-1; i >= 0; i--)
        {
            AbilityCard card = ply2.AbilityCards[i];
            if (card.Drawn) continue;
            if (!card.AIShouldDraw(this, ply2)) continue;
            yield return new WaitForSeconds(0.3f);
            DrawAbilityCard(ply2, i);
        }

        if (ply2.BlackjackTotal(BlackjackThreshold, false) < BlackjackThreshold && ply2.NumberCards.Count < 6)
        {
            int BlackjackTotal = ply2.BlackjackTotal(BlackjackThreshold, false);
            int difference = BlackjackThreshold - BlackjackTotal;

            if (difference > 0)
            {
                if (difference > 5)
                {
                    draw = true;
                } else
                {
                    switch (difference)
                    {
                        case 5:
                            draw = Random.Range(1, 5) == 1;
                            break;

                        case 4:
                            draw = Random.Range(1, 15) == 1;
                            break;

                        case 3:
                            draw = Random.Range(1, 30) == 1;
                            break;

                        case 2:
                            draw = Random.Range(1, 60) == 1;
                            break;

                        default:
                            draw = true;
                            break;
                    }
                }
            }
        }

        notificationText = "STAYED";

        if (draw)
        {
            notificationText = "DRAWN";
        }

        ShowNotification(notificationText, new Color(241f/255f, 246f/255f, 86f/255f, 1f));

        yield return new WaitForSeconds(1f);

        if (draw) { DrawNumberCard(ply2); yield return new WaitForSeconds(0.5f); }

        if (!draw && didStay)
        {
            StartCoroutine(CombatSection());
        } else
        {
            phase = GamePhase.PlayerTurn;
            canvasAnimator.Play("In", 0, 0);
            canvasManager.SetFunctionality(true);

            UpdateDrawNumberCardText();
            UpdateProgressText();
        }
    }

    void MakeGodCubesPlayAnimation(string animationName, Transform transform)
    {
        foreach (Transform child in transform)
        {
            Transform childTransform = child.Find("GodCube(Clone)");
            Transform cubeTransform = child.Find("GodCube(Clone)/Cube");
            childTransform.transform.rotation = Quaternion.identity;

            GameObject cube = cubeTransform.gameObject;
            Animator cubeAnimator = cube.GetComponent<Animator>();

            cubeAnimator.Play(animationName, 0, 0);
        }
    }

    IEnumerator Fight(Player attacker)
    {

        GameObject physicalAttackerCards = playerNumberCards;
        GameObject physicalDefenderCards = enemyNumberCards;

        GameObject hiddenCard = enemyNumberCards.transform.Find("0").gameObject;
        NumberCard enemyFirstCard = ply2.NumberCards[0];

        TMP_Text valueText =
            hiddenCard.transform.Find("Card/Canvas/ValueLabel")
            .GetComponent<TMP_Text>();

        TMP_Text damageText =
            hiddenCard.transform.Find("Card/Canvas/DamageLabel")
            .GetComponent<TMP_Text>();

        TMP_Text healthText =
            hiddenCard.transform.Find("Card/Canvas/HealthLabel")
            .GetComponent<TMP_Text>();

        valueText.text = enemyFirstCard.Value.ToString();
        damageText.text = enemyFirstCard.Damage.ToString();
        healthText.text = enemyFirstCard.Health.ToString();

        Player defender = ply2;

        if (attacker == ply2)
        {
            physicalAttackerCards = enemyNumberCards;
            physicalDefenderCards = playerNumberCards;
            defender = ply1;
        }

        int laneCount = Mathf.Min(
            attacker.NumberCards.Count,
            defender.NumberCards.Count
        );

        for (int i = 0; i < laneCount; i++)
        {
            Transform attackerCard = physicalAttackerCards.transform.GetChild(i);
            Transform defenderCard = physicalDefenderCards.transform.GetChild(i);

            Transform attackerCube = attackerCard.Find("GodCube(Clone)");
            Transform defenderCube = defenderCard.Find("GodCube(Clone)");

            if (attackerCube == null || defenderCube == null)
                continue;

            Animator attackerAnimator = attackerCube.Find("Cube").GetComponent<Animator>();
            Animator defenderAnimator = defenderCube.Find("Cube").GetComponent<Animator>();

            // Face correct direction
            if (attacker == ply2)
                attackerCube.rotation = Quaternion.Euler(0f, 180f, 0f);
            else
                attackerCube.rotation = Quaternion.identity;

            attackerAnimator.Play("Attack", 0, 0);

            attackGodCubeSound.PlayOneShot(attackGodCubeClip, 1f);

            yield return new WaitForSeconds(0.6f);

            defenderAnimator.Play("Defend", 0, 0);

            yield return new WaitForSeconds(0.6f);
        }
    }

    IEnumerator CombatSection()
    {
        phase = GamePhase.Combat;

        TMP_Text progressTxtObj = progressText.GetComponent<TMP_Text>();

        progressTxtObj.text = "They're gonna fight!";
        TMP_Text notificationTxt = notificationUIText.GetComponent<TMP_Text>();

        Player whoWon = DetermineBlackjackWinner();
        Player whoLost = ply1;

        ////////reveal opp card

        if (whoWon == ply1) whoLost = ply2;

        yield return new WaitForSeconds(1f);
        TMP_Text OppDefenseTxt = OppDefense.GetComponent<TMP_Text>();
        TMP_Text OppAttackTxt = OppAttack.GetComponent<TMP_Text>();
        OppDefenseTxt.text = ply2.TotalHealth(BlackjackThreshold, false).ToString();
        OppAttackTxt.text = ply2.TotalDamage(false).ToString();
        int YouDef = ply1.TotalHealth(BlackjackThreshold, false);
        int YouAtk = ply1.TotalDamage(false);
        int OppDef = ply2.TotalHealth(BlackjackThreshold, false);
        int OppAtk = ply2.TotalDamage(false);
        TMP_Text YouHpTxt = TempYouHp.GetComponent<TMP_Text>();
        TMP_Text OppHpTxt = TempOppHp.GetComponent<TMP_Text>();

        decidedSound.Play();
        Animator notificationUIAnimator = notificationUI.GetComponent<Animator>();
        notificationUIAnimator.Play("Notification_Popup", 0, 0);

        if (whoWon == ply1)
        {
            ShowNotification("YOU WON\n<color=#FFFFFF>Since you're closest to the Threshold.</color>", new Color(0, 49f/255f, 188f/255f, 1f));
            if (OppDef - YouAtk >= 0)
            {
                
            } else
            {
                ply2.Life += OppDef - YouAtk; // formular for Ryzer Hp if you win
                ply1.Life -= OppDef - YouAtk; // formular for Your Hp if you win
            }
            YouHpTxt.text = ply1.Life.ToString();
            OppHpTxt.text = ply2.Life.ToString();

            StartCoroutine(Fight(whoWon));

            yield return new WaitForSeconds(2.5f);

            StartCoroutine(Fight(whoLost));
        } else
        {
            ShowNotification("ENEMY WON\n<color=#FFFFFF>Since they're closest to the Threshold.</color>",new Color(241f/255f, 246f/255f, 86f/255f, 1f));
        }

        yield return new WaitForSeconds(2f);

        // remove expired ability cards
        // ply
        for (int i = 0; i < ply1.AbilityCards.Count; i++)
        {
            AbilityCard card = ply1.AbilityCards[i];
            if (!card.Drawn) continue;
            card.triesPassed += 1;

            if (card.triesPassed >= card.triesDecayTime)
            {
                card.Remove(this, ply1, ply2);
                ply1.AbilityCards.RemoveAt(i);

                Destroy(playerAbilityCards.transform.Find(i.ToString()).gameObject);

                for (int x = i + 1; x < ply1.AbilityCards.Count; x++)
                {
                    Transform physicalCard = playerAbilityCards.transform.Find(x.ToString());
                    if (physicalCard != null)
                    {
                        physicalCard.gameObject.name = (x - 1).ToString();
                    }
                }
            }
        }
        // opponent
        for (int i = 0; i < ply2.AbilityCards.Count; i++)
        {
            AbilityCard card = ply2.AbilityCards[i];
            if (!card.Drawn) continue;
            card.triesPassed += 1;

            if (card.triesPassed >= card.triesDecayTime)
            {
                Destroy(playerAbilityCards.transform.Find(i.ToString()).gameObject);
                card.Remove(this, ply2, ply1);
                ply2.AbilityCards.RemoveAt(i);

                for (int x = i + 1; x < ply2.AbilityCards.Count; x++)
                {
                    Transform physicalCard = playerAbilityCards.transform.Find(x.ToString());
                    if (physicalCard != null)
                    {
                        physicalCard.gameObject.name = (x - 1).ToString();
                    }
                }
            }
        }

        RepositionCards(playerAbilityCards.transform, true);
        RepositionCards(enemyAbilityCards.transform, true);

        Cleanup();
        BlackjackThreshold = 21;
        StartCoroutine(StartNewRound());
        StartCoroutine(ShowUIButtons());
    }

    public void RepositionCards(Transform parent, bool isAbilityCard = false)
    {
        int cardCount = parent.childCount;
        float normalSpacing = 1.2f;
        float compressedSpacing = 0.9f;
        float spacing = cardCount > 3 ? compressedSpacing : normalSpacing;

        float totalWidth = (cardCount - 1) * spacing;
        float startX = -totalWidth / 2f;

        Vector3 startingPos = new Vector3(-2.27f, 0.033f, -16.04f);

        if (parent == enemyNumberCards.transform) startingPos = new Vector3(-2.27f, 0.033f, -14.5f);

        for (int i = 0; i < cardCount; i++)
        {
            Transform card = parent.Find(i.ToString());
            if (!card) continue;

            Vector3 targetPos = startingPos + new Vector3(
                startX + (i * spacing),
                0f,
                0f
            );

            Ease ease = Ease.InOutBack;
            float timeToTween = 1.15f;

            if (isAbilityCard)
            {
                ease = Ease.OutBack;
                timeToTween = 0.75f;

                if (parent == enemyAbilityCards.transform)
                {
                    targetPos += new Vector3(-0.8f,0.2f,-5.5f);
                } else {
                    targetPos += new Vector3(-0.8f,0.2f,-9f);
                }
            }

            card.DOMove(targetPos, timeToTween)
                .SetEase(ease);
        }
    }

    public void AnswerNumberCardPrompt(Player owner, int index, PromptAbilityCard card)
    {
        if (owner.NumberCards[index] == null) return;

        GameObject ScrollViewContentsObj = cardPrompt.transform.Find("Scroll View/Viewport/Content").gameObject;
        foreach (Transform child in ScrollViewContentsObj.transform)
        {
            Destroy(child.gameObject);
        }

        canvasAnimator.Play("In", 0, 0);
        canvasManager.SetFunctionality(true);
        cardPrompt.SetActive(false);

        if (inventoryPanelHidden) {
            canvasManager.ResolvePanel();
            inventoryPanelHidden = false;
        }

        Player opponent = ply2;
        if (owner == ply2) opponent = ply1;

        card.PromptChosen(this, owner, opponent, index);
    }


    public void AnswerNumberCardPrompt(int owner, int index, PromptAbilityCard card)
    {
        Player ply = ply1;
        if (owner == 2) ply = ply2;

        AnswerNumberCardPrompt(ply, index, card);
    }

    public void AnswerAbilityCardPrompt(Player ply, int index, PromptAbilityCardAlt card) {
        if (ply.AbilityCards[index] == null) return;

        GameObject ScrollViewContentsObj = cardPrompt.transform.Find("Scroll View/Viewport/Content").gameObject;
        foreach (Transform child in ScrollViewContentsObj.transform)
        {
            Destroy(child.gameObject);
        }

        canvasAnimator.Play("In", 0, 0);
        canvasManager.SetFunctionality(true);
        cardPrompt.SetActive(false);

        if (inventoryPanelHidden) {
            canvasManager.ResolvePanel();
            inventoryPanelHidden = false;
        }

        Player opponent = ply2;
        if (ply == ply2) opponent = ply1;

        card.PromptChosen(this, ply, opponent, index);
    }

    public void AnswerAbilityCardPrompt(int ply, int index, PromptAbilityCardAlt card) {
        Player plyChosen = ply1;
        if (ply == 2) plyChosen = ply2;

        AnswerAbilityCardPrompt(plyChosen, index, card);
    }

    public void PromptForAbilityCard(Player owner, string title, string reason, PromptAbilityCardAlt card) {
        if (owner == ply2) {
            AnswerAbilityCardPrompt(ply2, card.AICardDecision(owner), card);
            return;
        }

        if (!canvasManager.GetPanelHidden()) inventoryPanelHidden = true;
        canvasManager.ResolvePanel();

        canvasAnimator.Play("Out", 0, 0);
        canvasManager.SetFunctionality(false);
        cardPrompt.SetActive(true);

        Animator inventoryAnimator = GameObject.Find("Canvas/InventoryContainer/Inventory").GetComponent<Animator>();
        inventoryAnimator.Play("In", 0, 0);

        GameObject TitleLabelObj = cardPrompt.transform.Find("TitleLabel").gameObject;
        GameObject ReasonLabelObj = cardPrompt.transform.Find("ReasonLabel").gameObject;
        GameObject ScrollViewContentsObj = cardPrompt.transform.Find("Scroll View/Viewport/Content").gameObject;

        TMP_Text TitleLabel = TitleLabelObj.GetComponent<TMP_Text>();
        TMP_Text ReasonLabel = ReasonLabelObj.GetComponent<TMP_Text>();

        TitleLabel.text = title;
        ReasonLabel.text = reason;

        int plyIndex = 1;
        if (owner == ply2) plyIndex = 2;

        for (int i = 0; i < owner.AbilityCards.Count; i++)
        {
            AbilityCard abilityCard = owner.AbilityCards[i];
            if (abilityCard == card) continue;

            GameObject cardRepresentation = Instantiate(abilityCardUIPrefab, ScrollViewContentsObj.transform);
            Destroy(cardRepresentation.GetComponent<AbilityCardUIScript>());
            cardRepresentation.AddComponent<AbilityCardPromptScript>();

            AbilityCardPromptScript cardPromptScript = cardRepresentation.GetComponent<AbilityCardPromptScript>();
            cardPromptScript.cardIndex = i;
            cardPromptScript.plyNumber = plyIndex;
            cardPromptScript.card = card;

            TMP_Text cardText = cardRepresentation.transform.Find("NameLabel").GetComponent<TMP_Text>();
            cardText.text = abilityCard.name;
        }
    }

    public void PromptForNumberCard(Player owner, string title, string reason, PromptAbilityCard card)
    {
        if (owner == ply2) {
            AnswerNumberCardPrompt(2, card.AICardDecision(owner), card);
            return;
        }

        if (!canvasManager.GetPanelHidden()) inventoryPanelHidden = true;
        canvasManager.ResolvePanel();

        canvasAnimator.Play("Out", 0, 0);
        canvasManager.SetFunctionality(false);
        cardPrompt.SetActive(true);

        Animator inventoryAnimator = GameObject.Find("Canvas/InventoryContainer/Inventory").GetComponent<Animator>();
        inventoryAnimator.Play("In", 0, 0);

        GameObject TitleLabelObj = cardPrompt.transform.Find("TitleLabel").gameObject;
        GameObject ReasonLabelObj = cardPrompt.transform.Find("ReasonLabel").gameObject;
        GameObject ScrollViewContentsObj = cardPrompt.transform.Find("Scroll View/Viewport/Content").gameObject;

        TMP_Text TitleLabel = TitleLabelObj.GetComponent<TMP_Text>();
        TMP_Text ReasonLabel = ReasonLabelObj.GetComponent<TMP_Text>();

        TitleLabel.text = title;
        ReasonLabel.text = reason;

        int plyIndex = 1;
        if (owner == ply2) plyIndex = 2;

        for (int i = 0; i < owner.NumberCards.Count; i++)
        {
            NumberCard nmbCard = owner.NumberCards[i];
            GameObject cardRepresentation = Instantiate(abilityCardUIPrefab, ScrollViewContentsObj.transform);
            Destroy(cardRepresentation.GetComponent<AbilityCardUIScript>());
            cardRepresentation.AddComponent<CardPromptScript>();

            CardPromptScript cardPrompt = cardRepresentation.GetComponent<CardPromptScript>();
            cardPrompt.cardIndex = i;
            cardPrompt.plyNumber = plyIndex;
            cardPrompt.card = card;

            TMP_Text cardText = cardRepresentation.transform.Find("NameLabel").GetComponent<TMP_Text>();

            if (nmbCard.Value == 12) {
                cardText.text = "A";
            } else {
                cardText.text = nmbCard.Value.ToString();
            }
        }
    }

    public GameObject InstantiateNumberCard(Transform parent)
    {
        return Instantiate(numberCard, parent);
    }

    public void UpdateAttackDefendText()
    {
        TMP_Text YouDefenseTxt = YouDefense.GetComponent<TMP_Text>();
        TMP_Text YouAttackTxt = YouAttack.GetComponent<TMP_Text>();
        TMP_Text OppDefenseTxt = OppDefense.GetComponent<TMP_Text>();
        TMP_Text OppAttackTxt = OppAttack.GetComponent<TMP_Text>();
        YouDefenseTxt.text = ply1.TotalHealth(BlackjackThreshold, false).ToString();
        YouAttackTxt.text = ply1.TotalDamage(false).ToString();
        OppDefenseTxt.text = ply2.TotalHealth(BlackjackThreshold).ToString() + "?";
        OppAttackTxt.text = ply2.TotalDamage().ToString() + "?";
    }

    public void RemoveAbilityCard(Player ply, int index) {
        ply.AbilityCards.RemoveAt(index);
        RebuildInventoryPanel();
    }

    public void RemoveNumberCard(Player ply, int index)
    {
        GameObject cardsObject = playerNumberCards;
        if (ply == ply2) cardsObject = enemyNumberCards;

        Transform cardToRemove = cardsObject.transform.Find(index.ToString());
        if (cardToRemove != null)
        {
            Destroy(cardToRemove.gameObject);
        }

        for (int i = index + 1; i < ply.NumberCards.Count; i++)
        {
            Transform card = cardsObject.transform.Find(i.ToString());
            if (card != null)
            {
                card.gameObject.name = (i - 1).ToString();
            }
        }

        ply.NumberCards.RemoveAt(index);
        RepositionCards(cardsObject.transform, false);
    }

    public void RemoveNumberCard(int ply, int index)
    {
        Player chosenPly = ply1;

        if (ply == 2) chosenPly = ply2;
        RemoveNumberCard(chosenPly, index);
    }

    public void DrawTwice(int ply)
    {
        Player chosenPly = ply1;
        if (ply == 2) chosenPly = ply2;
        StartCoroutine(DrawTwice(chosenPly));
    }

    public IEnumerator DrawTwice(Player ply)
    {
        DrawNumberCard(ply);
        yield return new WaitForSeconds(0.5f);
        DrawNumberCard(ply);
    }

    public void DrawNumberCard(int ply)
    {
        Player chosenPly = ply1;
        if (ply == 2) chosenPly = ply2;

        DrawNumberCard(chosenPly);
    }

    void DrawNumberCard(Player ply)
    {
        NumberCard card = deck.Draw();
        ply.NumberCards.Add(card);

        UpdateAttackDefendText();

        GameObject parent = (ply == ply2)
            ? enemyNumberCards
            : playerNumberCards;

        GameObject cardRepresentation = InstantiateNumberCard(parent.transform);
        cardRepresentation.name = (ply.NumberCards.Count-1).ToString();

        // ----- Update card UI -----

        TMP_Text valueText =
            cardRepresentation.transform.Find("Card/Canvas/ValueLabel")
            .GetComponent<TMP_Text>();

        TMP_Text damageText =
            cardRepresentation.transform.Find("Card/Canvas/DamageLabel")
            .GetComponent<TMP_Text>();

        TMP_Text healthText =
            cardRepresentation.transform.Find("Card/Canvas/HealthLabel")
            .GetComponent<TMP_Text>();

        // Hide enemy first card
        if (ply == ply2 && ply2.NumberCards.Count == 1)
        {
            valueText.text = "?";
            damageText.text = "?";
            healthText.text = "?";
        } else
        {
            if (card.Value == 12)
            {
                valueText.text = "A";
                damageText.text = "1/11";
                healthText.text = "11/1";
            } else
            {
                valueText.text = card.Value.ToString();
                damageText.text = card.Damage.ToString();
                healthText.text = card.Health.ToString();
            }
        }
        if (ply1.BlackjackTotal(BlackjackThreshold, false) >= BlackjackThreshold)
        {
            drawButton.SetActive(false);
        }

        // ----- Layout -----
        RepositionCards(parent.transform);
    }

    void RebuildInventoryPanel()
    {
        foreach (Transform child in inventoryPanelScroll.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < ply1.AbilityCards.Count; i++)
        {   
            AbilityCard card = ply1.AbilityCards[i];
            if (card.Drawn) continue;

            GameObject cardRepresentation = Instantiate(abilityCardUIPrefab, inventoryPanelScroll.transform);
            AbilityCardUIScript cardRepresentationScript = cardRepresentation.GetComponent<AbilityCardUIScript>();
            TMP_Text cardRepresentationLabel = cardRepresentation.transform.Find("NameLabel").gameObject.GetComponent<TMP_Text>();

            cardRepresentationScript.index = i;
            cardRepresentationLabel.text = card.name;
        }
    }


    public void DrawAbilityCard(Player ply, int index)
    {
        ply.AbilityCards[index].Drawn = true;

        Player oppPly = ply2;
        if (ply == ply2) oppPly = ply1;

        ply.AbilityCards[index].Apply(this, ply, oppPly);
        RebuildInventoryPanel();

        // Summon ability card visually
        Transform parent = playerAbilityCards.transform;
        if (ply == ply2) parent = enemyAbilityCards.transform;

        GameObject cardRepresentation = Instantiate(abilityCard, parent);
        cardRepresentation.name = index.ToString();
        TMP_Text cardText = cardRepresentation.transform.Find("Card/Canvas/NameLabel").GetComponent<TMP_Text>();
        cardText.text = ply.AbilityCards[index].name;

        RepositionCards(parent, true);
    }

    public void DrawAbilityCard(int ply, int index)
    {
        Player player = ply1;
        if (ply == 2) player = ply2;

        DrawAbilityCard(player, index);
    }

    void DestroyAllSpawnedCardObjects()
    {
        foreach (Transform child in playerNumberCards.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in playerAbilityCards.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in enemyNumberCards.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in enemyAbilityCards.transform)
        {
            Destroy(child.gameObject);
        }
    }

    Player DetermineBlackjackWinner()
    {
        int ply1Total = ply1.BlackjackTotal(BlackjackThreshold, false);
        int ply2Total = ply2.BlackjackTotal(BlackjackThreshold, false);

        bool ply1Bust = ply1Total > BlackjackThreshold;
        bool ply2Bust = ply2Total > BlackjackThreshold;

        if (ply1Bust && ply2Bust)
        {
            if (ply1Total < ply2Total)
            {
                return ply1;
            } else if (ply2Total < ply1Total)
            {
                return ply2;
            } else
            {
                return null;
            }
        }

        if (ply1Bust) return ply2;
        if (ply2Bust) return ply1;

        if (ply1Total > ply2Total) return ply1;
        if (ply2Total > ply1Total) return ply2;

        return null;
    }

    void ResolveCombat(Player attacker, Player defender, float counterMultiplier = 1f)
    {
        int attackPower = attacker.TotalDamage(false);
        int defencePower = defender.TotalHealth(BlackjackThreshold, false);

        if (attackPower > defencePower) defender.Life -= 1;
    }

    void CombatPhase(Player first, Player second)
    {
        ResolveCombat(first, second);
        ResolveCombat(second, first, 0.5f);
    }

    void Cleanup()
    {
        ply1.NumberCards.Clear();
        ply2.NumberCards.Clear();

        DestroyAllSpawnedCardObjects();
    }
}
