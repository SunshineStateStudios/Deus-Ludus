using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;
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
    public GameObject Camera;
    public int BlackjackThreshold = 21;

    // Attributes for the drawn/stayed overlay
    public GameObject notificationUI;

    private AudioSource decidedSound;
    private Vector3 oldCameraPos;
    // Misc
    private GameObject inventoryButton;
    private GameObject inventoryPanel;
    private GameObject stayButton;
    private GameObject drawButton;
    private GameObject notificationUIText;
    private GameObject progressText;

    void Start()
    {
        oldCameraPos = Camera.transform.position;
        decidedSound = GetComponent<AudioSource>();

        inventoryButton = GameObject.Find("Canvas/InventoryButton");
        inventoryPanel = GameObject.Find("Canvas/InventoryPanel");
        stayButton = GameObject.Find("Canvas/StayButton");
        drawButton = GameObject.Find("Canvas/DrawNumberCardButton");
        notificationUIText = GameObject.Find("Canvas/Notification/Label");
        progressText = GameObject.Find("Canvas/ProgressText");

        ply1 = new Player();
        ply2 = new Player();
        deck = new Deck();

        StartCoroutine(StartNewRound());
        StartCoroutine(ShowUIButtons());
    }

    IEnumerator ShowUIButtons()
    {
        RectTransform inventoryButtonRect = inventoryButton.GetComponent<RectTransform>();
        RectTransform stayButtonRect = stayButton.GetComponent<RectTransform>();
        RectTransform drawButtonRect = drawButton.GetComponent<RectTransform>();
        RectTransform textRect = progressText.GetComponent<RectTransform>();

        inventoryButtonRect.anchoredPosition = new Vector2(888f, 57f);
        stayButtonRect.anchoredPosition = new Vector2(-888f, 158f);
        drawButtonRect.anchoredPosition = new Vector2(-888f, 57f);

        yield return new WaitForSeconds(3.5f);

        inventoryButtonRect.DOAnchorPos(new Vector2(-60f, 57f), 0.75f).SetEase(Ease.OutSine);
        stayButtonRect.DOAnchorPos(new Vector2(52f, 158f), 0.75f).SetEase(Ease.OutSine);
        drawButtonRect.DOAnchorPos(new Vector2(52f, 57f), 0.75f).SetEase(Ease.OutSine);
        textRect.DOAnchorPos(new Vector2(0f, -57f), 0.75f).SetEase(Ease.OutSine);
    }

    IEnumerator StartNewRound()
    {
        ply1.NumberCards.Clear();
        ply2.NumberCards.Clear();

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
        progressTxtObj.text = "card total: <b><color=#8391F1><b>" + ply1.BlackjackTotal.ToString() + "</color>\nthreshold: <b><color=#8391F1><b>" + BlackjackThreshold.ToString() + "</color></b>";
    }

    public IEnumerator EndRound(bool didStay)
    {
        TMP_Text progressTxtObj = progressText.GetComponent<TMP_Text>();

        progressTxtObj.text = "card total: <b><color=#8391F1><b>" + ply1.BlackjackTotal.ToString() + "</color>\nthreshold: <b><color=#8391F1><b>" + BlackjackThreshold.ToString() + "</color></b>";

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

        yield return new WaitForSeconds(1.5f);

        if (!didStay)
        {
            DrawNumberCard(ply1);
        }

        yield return new WaitForSeconds(1f);

        progressText.SetActive(true);
        progressTxtObj.text = "It's the opponent's turn!";
        phase = GamePhase.AITurn;

        yield return new WaitForSeconds(1.5f);

        bool draw = false;

        if (!ply2.IsBust)
        {
            int BlackjackTotal = ply2.BlackjackTotal;
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

        yield return new WaitForSeconds(1.5f);

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
            progressTxtObj.text = "card total: <b><color=#8391F1><b>" + ply1.BlackjackTotal.ToString() + "</color>\nthreshold: <b><color=#8391F1><b>" + BlackjackThreshold.ToString() + "</color></b>";
        }
    }

    IEnumerator Fight(Player attacker)
    {
        GameObject physicalAttackerCards = playerNumberCards;
        GameObject physicalDefenderCards = enemyNumberCards;

        Player defender = ply2;

        if (attacker == ply2)
        {
            physicalAttackerCards = enemyNumberCards;
            physicalDefenderCards = playerNumberCards;
        }

        int attackerTotalAttackPoints = 0;
        int defenderTotalDefendPoints = 0;

        foreach (NumberCard card in attacker.NumberCards)
        {
            attackerTotalAttackPoints += card.Damage;
        }

        foreach (NumberCard card in defender.NumberCards)
        {
            defenderTotalDefendPoints += card.Health;
        }

        // Playing combat animations

        foreach (Transform child in physicalAttackerCards.transform)
        {
            Transform childTransform = child.Find("GodCube(Clone)");
            Transform cubeTransform = child.Find("GodCube(Clone)/Cube");
            childTransform.transform.rotation = Quaternion.identity;

            GameObject cube = cubeTransform.gameObject;
            Animator cubeAnimator = cube.GetComponent<Animator>();

            cubeAnimator.Play("Attack", 0, 0);
        }

        yield return new WaitForSeconds(0.7f);

        foreach (Transform child in physicalDefenderCards.transform)
        {
            Transform childTransform = child.Find("GodCube(Clone)");
            Transform cubeTransform = child.Find("GodCube(Clone)/Cube");
            childTransform.transform.rotation = Quaternion.identity;

            GameObject cube = cubeTransform.gameObject;
            Animator cubeAnimator = cube.GetComponent<Animator>();

            cubeAnimator.Play("Defend", 0, 0);
        }

        yield return new WaitForSeconds(1.5f);
    }

    IEnumerator CombatSection()
    {
        phase = GamePhase.Combat;

        TMP_Text progressTxtObj = progressText.GetComponent<TMP_Text>();

        progressTxtObj.text = "They're gonna fight!";
        TMP_Text notificationTxt = notificationUIText.GetComponent<TMP_Text>();

        Player whoWon = DetermineBlackjackWinner();

        yield return new WaitForSeconds(1.5f);

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

        yield return new WaitForSeconds(1.5f);

        /*Cleanup();
        StartCoroutine(StartNewRound());
        StartCoroutine(ShowUIButtons());
        inventoryButton.SetActive(true);
        stayButton.SetActive(true);
        drawButton.SetActive(true);
        inventoryPanel.SetActive(true);*/
    }

    void DrawNumberCard(Player ply)
    {
        NumberCard card = deck.Draw();
        ply.NumberCards.Add(card);

        GameObject parent = playerNumberCards;

        if (ply == ply2)
        {
            parent = enemyNumberCards;
        }

        GameObject cardRepresentation = Instantiate(numberCard, parent.transform, false);
        cardRepresentation.transform.localPosition = new Vector3(3f + (ply.NumberCards.Count - 1), 0f, 0f);

        // Changing the numbers on the card visual,,,.
        GameObject valueLabel = cardRepresentation.transform.Find("Canvas/ValueLabel").gameObject;
        GameObject damageLabel = cardRepresentation.transform.Find("Canvas/DamageLabel").gameObject;
        GameObject healthLabel = cardRepresentation.transform.Find("Canvas/HealthLabel").gameObject;

        TMP_Text valueText = valueLabel.GetComponent<TMP_Text>();
        TMP_Text damageText = damageLabel.GetComponent<TMP_Text>();
        TMP_Text healthText = healthLabel.GetComponent<TMP_Text>();

        valueText.text = card.Value.ToString();
        damageText.text = card.Damage.ToString();
        healthText.text = card.Health.ToString();

        if (ply == ply2){
            if (ply2.NumberCards.Count == 1){
                valueText.text = "?";
                damageText.text = "?";
                healthText.text = "?";
            }
        }

        // Setting camera position

        Player otherPlayer;

        if (ply == ply1)
        {
            otherPlayer = ply2;
        } else
        {
            otherPlayer = ply1;
        }

        int amountOfCards = ply.NumberCards.Count;
        if (amountOfCards < otherPlayer.NumberCards.Count)
        {
            amountOfCards = otherPlayer.NumberCards.Count;
        }

        Vector3 newPos = oldCameraPos;
        if (amountOfCards > 3)
        {
            newPos = oldCameraPos + new Vector3(1f * (amountOfCards-3), 0f, 0f);
        }
        Camera.transform.DOMove(newPos, 1f);
    }

    public void DrawNumberCard(int ply)
    {
        Player plyToUse = ply2;

        if (ply == 1)
        {
            plyToUse = ply1;
        }

        DrawNumberCard(plyToUse);
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
        if (ply1.IsBust && ply2.IsBust)
        {
            if (ply1.BlackjackTotal == ply2.BlackjackTotal)
            {
                return null;
            } else if (ply1.BlackjackTotal < ply2.BlackjackTotal)
            {
                return ply1;
            } else
            {
                return ply2;
            }
        }

        if (ply1.IsBust) return ply2;
        if (ply2.IsBust) return ply1;

        int p1Diff = 21 - ply1.BlackjackTotal;
        int p2Diff = 21 - ply2.BlackjackTotal;

        if (p1Diff < p2Diff) return ply1;
        if (p2Diff < p1Diff) return ply2;

        return null;
    }

    void ResolveCombat(Player attacker, Player defender, float counterMultiplier = 1f)
    {
        int attackPower = attacker.TotalDamage;
        int defencePower = defender.TotalHealth;

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
