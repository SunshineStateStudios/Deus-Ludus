using System.Collections;
using UnityEngine;
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
    public GameObject playerNumberCards;
    public GameObject enemyNumberCards;
    public AudioClip attackGodCubeClip;
    public int BlackjackThreshold = 21;

    // Attributes for the drawn/stayed overlay
    public GameObject notificationUI;

    private AudioSource decidedSound;
    private AudioSource attackGodCubeSound;
    // Misc
    private GameObject inventoryButton;
    private GameObject inventoryPanel;
    private GameObject stayButton;
    private GameObject drawButton;
    private GameObject notificationUIText;
    private GameObject progressText;
    private GameObject healthPanel;
    
    private GameObject YouDefense;
    private GameObject YouAttack;
    private GameObject OppDefense;
    private GameObject OppAttack;

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
        healthPanel = GameObject.Find("Canvas/HealthPanel");
        YouDefense = GameObject.Find("Canvas/YouDefense");
        YouAttack = GameObject.Find("Canvas/YouAttack");
        OppDefense = GameObject.Find("Canvas/OppDefense");
        OppAttack = GameObject.Find("Canvas/OppAttack");

        ply1 = new Player();
        ply2 = new Player();
        deck = new Deck();

        TMP_Text YouDefenseTxt = YouDefense.GetComponent<TMP_Text>();
        TMP_Text YouAttackTxt = YouAttack.GetComponent<TMP_Text>();
        TMP_Text OppDefenseTxt = OppDefense.GetComponent<TMP_Text>();
        TMP_Text OppAttackTxt = OppAttack.GetComponent<TMP_Text>();
        YouDefenseTxt.text = ply1.TotalHealth(false).ToString();
        YouAttackTxt.text = ply1.TotalDamage(false).ToString();
        OppDefenseTxt.text = ply2.TotalHealth().ToString();
        OppAttackTxt.text = ply2.TotalDamage().ToString();


        StartCoroutine(StartNewRound());
        StartCoroutine(ShowUIButtons());
    }
    

    void RebuildAbilityCardPool()
    {
        abilityCardList = new List<AbilityCard>();
        abilityCardList.Add(new Test1());
        abilityCardList.Add(new Test2());
        abilityCardList.Add(new Test3());
        abilityCardList.Add(new Test4());
        abilityCardList.Add(new Test5());
        abilityCardList.Add(new Test6());
        abilityCardList.Add(new Test7());
        abilityCardList.Add(new Test8());

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

    AbilityCard GivePlayerAbilityCard(Player ply)
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
        RectTransform inventoryButtonRect = inventoryButton.GetComponent<RectTransform>();
        RectTransform stayButtonRect = stayButton.GetComponent<RectTransform>();
        RectTransform drawButtonRect = drawButton.GetComponent<RectTransform>();
        RectTransform textRect = progressText.GetComponent<RectTransform>();
        RectTransform healthPanelRect = healthPanel.GetComponent<RectTransform>();

        inventoryButtonRect.anchoredPosition = new Vector2(888f, 57f);
        stayButtonRect.anchoredPosition = new Vector2(-888f, 158f);
        drawButtonRect.anchoredPosition = new Vector2(-888f, 57f);
        healthPanelRect.anchoredPosition = new Vector2(-171f, -187.3714f);

        yield return new WaitForSeconds(3.5f);

        inventoryButtonRect.DOAnchorPos(new Vector2(-60f, 57f), 0.75f).SetEase(Ease.OutSine);
        stayButtonRect.DOAnchorPos(new Vector2(127f, 245f), 0.75f).SetEase(Ease.OutSine);
        drawButtonRect.DOAnchorPos(new Vector2(127f, 111f), 0.75f).SetEase(Ease.OutSine);
        textRect.DOAnchorPos(new Vector2(0f, -57f), 0.75f).SetEase(Ease.OutSine);
        healthPanelRect.DOAnchorPos(new Vector2(30f, -187.3714f), 0.75f).SetEase(Ease.OutSine);
    }

    IEnumerator StartNewRound()
    {
        ply1.NumberCards.Clear();
        ply2.NumberCards.Clear();

        RebuildAbilityCardPool();
        GivePlayerAbilityCard(ply1);
        GivePlayerAbilityCard(ply2);

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
        progressTxtObj.text = "card total: <b><color=#8391F1><b>" + ply1.BlackjackTotal(false).ToString() + "</color>\nthreshold: <b><color=#8391F1><b>" + BlackjackThreshold.ToString() + "</color></b>";
    }

    public IEnumerator EndRound(bool didStay)
    {
        TMP_Text progressTxtObj = progressText.GetComponent<TMP_Text>();
        //
        TMP_Text YouDefenseTxt = YouDefense.GetComponent<TMP_Text>();
        TMP_Text YouAttackTxt = YouAttack.GetComponent<TMP_Text>();
        TMP_Text OppDefenseTxt = OppDefense.GetComponent<TMP_Text>();
        TMP_Text OppAttackTxt = OppAttack.GetComponent<TMP_Text>();
        //

        progressTxtObj.text = "card total: <b><color=#8391F1><b>" + ply1.BlackjackTotal(false).ToString() + "</color>\nthreshold: <b><color=#8391F1><b>" + BlackjackThreshold.ToString() + "</color></b>";

        progressText.SetActive(false);
        TMP_Text notificationTxt = notificationUIText.GetComponent<TMP_Text>();
        
        if (didStay)
        {
            notificationTxt.text = "STAYED";
        } else
        {
            notificationTxt.text = "DRAWN";
        }

        decidedSound.Play();
        Animator notificationUIAnimator = notificationUI.GetComponent<Animator>();
        notificationUIAnimator.Play("Notification_Popup", 0, 0);
        notificationTxt.color = new Color(0, 49f/255f, 188f/255f, 1f);

        inventoryButton.SetActive(false);
        stayButton.SetActive(false);
        drawButton.SetActive(false);
        inventoryPanel.SetActive(false);

        yield return new WaitForSeconds(1f);

        if (!didStay)
        {
            DrawNumberCard(ply1);
        }

        yield return new WaitForSeconds(1f);

        YouDefenseTxt.text = ply1.TotalHealth(false).ToString();
        YouAttackTxt.text = ply1.TotalDamage(false).ToString();
        OppDefenseTxt.text = ply2.TotalHealth().ToString();
        OppAttackTxt.text = ply2.TotalDamage().ToString();

        progressText.SetActive(true);
        progressTxtObj.text = "It's the opponent's turn!";
        phase = GamePhase.AITurn;

        yield return new WaitForSeconds(1f);

        bool draw = false;

        if (!ply2.IsBust())
        {
            int BlackjackTotal = ply2.BlackjackTotal(false);
            int difference = BlackjackThreshold - BlackjackTotal;

            if (difference > 0)
            {
                switch (difference)
                {
                    case 5:
                        draw = Random.Range(1, 20) == 1;
                        break;

                    case 4:
                        draw = Random.Range(1, 40) == 1;
                        break;

                    case 3:
                        draw = Random.Range(1, 60) == 1;
                        break;

                    case 2:
                        draw = Random.Range(1, 80) == 1;
                        break;

                    case 1:
                        draw = false;
                        break;

                    default:
                        draw = true;
                        break;
                }
            }
        }

        decidedSound.Play();
        notificationTxt.color = new Color(241f/255f, 246f/255f, 86f/255f, 1f);
        notificationUIAnimator.Play("Notification_Popup", 0, 0);

        if (draw)
        {
            notificationTxt.text = "DRAWN";
        } else
        {
            notificationTxt.text = "STAYED";
        }

        yield return new WaitForSeconds(1f);

        if (draw) { DrawNumberCard(ply2); yield return new WaitForSeconds(0.5f); }

        if (!draw && didStay)
        {
            StartCoroutine(CombatSection());
        } else
        {
            phase = GamePhase.PlayerTurn;
            inventoryButton.SetActive(true);
            stayButton.SetActive(true);
            drawButton.SetActive(true);
            inventoryPanel.SetActive(true);
            progressTxtObj.text = "card total: <b><color=#8391F1><b>" + ply1.BlackjackTotal(false).ToString() + "</color>\nthreshold: <b><color=#8391F1><b>" + BlackjackThreshold.ToString() + "</color></b>";
        }
    }

    void MakeGodCubesPlayAnimtion(string animationName, Transform transform)
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

        decidedSound.Play();
        Animator notificationUIAnimator = notificationUI.GetComponent<Animator>();
        notificationUIAnimator.Play("Notification_Popup", 0, 0);
        
        if (whoWon == ply1)
        {
            notificationTxt.color = new Color(0, 49f/255f, 188f/255f, 1f);
            notificationTxt.text = "YOU WON\n<color=#FFFFFF>Since you're closest to 21.</color>";
        } else
        {
            notificationTxt.color = new Color(241f/255f, 246f/255f, 86f/255f, 1f);
            notificationTxt.text = "ENEMY WON\n<color=#FFFFFF>Since they're closest to 21.</color>";
        }

        yield return new WaitForSeconds(2.3f);

        StartCoroutine(Fight(whoWon));

        yield return new WaitForSeconds(2.5f);

        StartCoroutine(Fight(whoLost));

        /*Cleanup();
        StartCoroutine(StartNewRound());
        StartCoroutine(ShowUIButtons());
        inventoryButton.SetActive(true);
        stayButton.SetActive(true);
        drawButton.SetActive(true);
        inventoryPanel.SetActive(true);*/
    }

    void RepositionCards(Transform parent)
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
            Transform card = parent.GetChild(i);

            Vector3 targetPos = startingPos + new Vector3(
                startX + (i * spacing),
                0f,
                0f
            );

            card.DOMove(targetPos, 0.35f)
                .SetEase(Ease.OutQuad);
        }
    }

    void DrawNumberCard(Player ply)
    {
        NumberCard card = deck.Draw();
        ply.NumberCards.Add(card);

        GameObject parent = (ply == ply2)
            ? enemyNumberCards
            : playerNumberCards;

        GameObject cardRepresentation =
            Instantiate(numberCard, parent.transform);
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

        valueText.text = card.Value.ToString();
        damageText.text = card.Damage.ToString();
        healthText.text = card.Health.ToString();

        // Hide enemy first card
        if (ply == ply2 && ply2.NumberCards.Count == 1)
        {
            valueText.text = "?";
            damageText.text = "?";
            healthText.text = "?";
        }

        // ----- Layout -----
        RepositionCards(parent.transform);
    }

    void DestroyAllSpawnedCardObjects()
    {
        foreach (Transform child in playerNumberCards.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in enemyNumberCards.transform)
        {
            Destroy(child.gameObject);
        }
    }

    Player DetermineBlackjackWinner()
    {
        if (ply1.IsBust() && ply2.IsBust())
        {
            if (ply1.BlackjackTotal(false) == ply2.BlackjackTotal(false))
            {
                return null;
            } else if (ply1.BlackjackTotal(false) < ply2.BlackjackTotal(false))
            {
                return ply1;
            } else
            {
                return ply2;
            }
        }

        if (ply1.IsBust()) return ply2;
        if (ply2.IsBust()) return ply1;

        int p1Diff = 21 - ply1.BlackjackTotal(false);
        int p2Diff = 21 - ply2.BlackjackTotal(false);

        if (p1Diff < p2Diff) return ply1;
        if (p2Diff < p1Diff) return ply2;

        return null;
    }

    void ResolveCombat(Player attacker, Player defender, float counterMultiplier = 1f)
    {
        int attackPower = attacker.TotalDamage(false);
        int defencePower = defender.TotalHealth(false);

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
        ply1.AbilityCards.Clear();

        ply2.NumberCards.Clear();
        ply2.AbilityCards.Clear();

        DestroyAllSpawnedCardObjects();
    }
}
