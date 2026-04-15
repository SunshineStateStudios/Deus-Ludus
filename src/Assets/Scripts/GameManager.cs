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
    public int BlackjackThreshold = 21;

    public GameObject losingMusicObj;
    public GameObject winningMusicObj;
    public GameObject defaultMusicObj;
    public bool alreadyPrompted { get; set; }

    private AudioSource decidedSound;
    private AudioSource attackGodCubeSound;

    private List<AbilityCard> abilityCardList;
    private CanvasManager canvasManagerScript;
    private MusicController musicController = new MusicController();
    private int rounds = 0;
    private int turns = 0;
    private float abilityCardDebounce = 0;

    bool IsPlayerTurn()
    {
        return phase == GamePhase.PlayerTurn;
    }

    void Start()
    {
        alreadyPrompted = false;
        
        musicController.losingMusic = losingMusicObj;
        musicController.winningMusic = winningMusicObj;
        musicController.defaultMusic = defaultMusicObj;
        musicController.Initialise();

        canvasManagerScript = canvasObject.GetComponent<CanvasManager>();

        AudioSource[] sources = GetComponents<AudioSource>();
        decidedSound = sources[0];
        attackGodCubeSound = sources[1];

        ply1 = new Player();
        ply2 = new Player();
        deck = new Deck();

        canvasManagerScript.SetHealth(ply1.Life, ply2.Life);

        StartCoroutine(StartNewRound());
    }

    void Update() {
        abilityCardDebounce += Time.deltaTime;
    }

    void RebuildAbilityCardPool()
    {
        abilityCardList = new List<AbilityCard>();

        abilityCardList.Add(new AbilityChariot());
        abilityCardList.Add(new AbilityDeath());
        abilityCardList.Add(new AbilityDevil());
        abilityCardList.Add(new AbilityEmperor());
        abilityCardList.Add(new AbilityEmpress());
        abilityCardList.Add(new AbilityHangedMan());
        abilityCardList.Add(new AbilityHermit());
        abilityCardList.Add(new AbilityHierophant());
        abilityCardList.Add(new AbilityHighPriestess());
        abilityCardList.Add(new AbilityJudgement());
        abilityCardList.Add(new AbilityJustice());
        abilityCardList.Add(new AbilityLovers());
        abilityCardList.Add(new AbilityMagician());
        abilityCardList.Add(new AbilityMoon());
        abilityCardList.Add(new AbilityStar());
        abilityCardList.Add(new AbilityStrength());
        abilityCardList.Add(new AbilitySun());
        abilityCardList.Add(new AbilityTemperance());
        abilityCardList.Add(new AbilityTower());
        abilityCardList.Add(new AbilityWheelOfFortune());
        abilityCardList.Add(new AbilityWorld());

        foreach (AbilityCard card in abilityCardList) {
            card.icon = Resources.Load<Sprite>("Icons/" + card.GetType().Name.Replace("Ability", ""));
        }

        int n = abilityCardList.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            AbilityCard temp = abilityCardList[i];
            abilityCardList[i] = abilityCardList[j];
            abilityCardList[j] = temp;
        }
    }

    public AbilityCard GivePlayerAbilityCard(Player ply)
    {
        if (abilityCardList.Count > 0 && ply.AbilityCards.Count < 5)
        {
            AbilityCard chosenCard = abilityCardList[0];
            ply.AbilityCards.Add(chosenCard);
            abilityCardList.RemoveAt(0);
            return chosenCard;
        }
        return null;
    }

    IEnumerator StartNewRound()
    {
        rounds++;
        turns = 1;

        canvasManagerScript.SetRounds(rounds, turns);

        ply1.NumberCards.Clear();
        ply2.NumberCards.Clear();

        RebuildAbilityCardPool();

        for (int i = 0; i < 4; i++) GivePlayerAbilityCard(ply1);
        for (int i = 0; i < 5; i++) GivePlayerAbilityCard(ply2);

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

        if (IsPlayerTurn())
        {
            canvasManagerScript.SetStatus("It's your turn!");
            if (rounds > 1) canvasManagerScript.SetActive(true);
        }
    }

    public IEnumerator EndRound(bool didStay)
    {
        canvasManagerScript.SetActive(false);
        canvasManagerScript.SetStatus("It's the opponent's turn!");

        if (!didStay)
            DrawNumberCard(ply1);

        phase = GamePhase.AITurn;
        yield return new WaitForSeconds(1f);

        for (int i = ply2.AbilityCards.Count - 1; i >= 0; i--)
        {
            AbilityCard card = ply2.AbilityCards[i];
            if (card.Drawn) continue;
            if (!card.AIShouldDraw(this, ply2)) continue;

            DrawAbilityCard(ply2, i);
            yield return new WaitForSeconds(1f);
        }

        bool draw = ply2.BlackjackTotal(BlackjackThreshold, false) < BlackjackThreshold;

        yield return new WaitForSeconds(1f);

        if (draw)
        {
            DrawNumberCard(ply2);
            yield return new WaitForSeconds(0.5f);
        }

        if (!draw && didStay)
        {
            StartCoroutine(CombatSection());
        }
        else
        {
            phase = GamePhase.PlayerTurn;

            if (IsPlayerTurn())
            {
                canvasManagerScript.SetStatus("It's your turn!");
                canvasManagerScript.SetActive(true);
            }
        }

        turns++;
        canvasManagerScript.SetRounds(rounds, turns);
    }

    public void DrawTwice(int ply) {
        Player plyChosen = ply1;
        if (ply == 2) plyChosen = ply2;

        StartCoroutine(DrawTwice(plyChosen));
    }

    public IEnumerator DrawTwice(Player ply)
    {
        DrawNumberCard(ply);
        yield return new WaitForSeconds(0.5f);
        DrawNumberCard(ply);

        if (ply == ply1)
        {
            phase = GamePhase.PlayerTurn;
            canvasManagerScript.HidePrompt();
            canvasManagerScript.SetStatus("It's your turn!");
            canvasManagerScript.SetActive(true);
        }
    }

    public void PromptForAbilityCard(Player owner, PromptAbilityCard card)
    {
        if (alreadyPrompted) return;
        alreadyPrompted = true;

        if (owner == ply2)
        {
            AnswerAbilityCardPrompt(ply2, card.AICardDecision(owner), card);
            return;
        }

        if (!IsPlayerTurn()) return;

        canvasManagerScript.SetActive(false);
        canvasManagerScript.SetStatus("Choose an ability card!");
        canvasManagerScript.ShowPrompt("ability", owner, card);
    }

    public void PromptForNumberCard(Player owner, PromptAbilityCard card)
    {
        if (alreadyPrompted) return;
        alreadyPrompted = true;

        if (owner == ply2)
        {
            AnswerNumberCardPrompt(owner, card.AICardDecision(owner), card);
            return;
        }

        if (!IsPlayerTurn()) return;

        canvasManagerScript.SetActive(false);
        canvasManagerScript.SetStatus("Choose a number card!");
        canvasManagerScript.ShowPrompt("number", owner, card);
    }

    public void AnswerAbilityCardPrompt(Player ply, int index, PromptAbilityCard card)
    {
        if (ply.AbilityCards[index] == null) return;

        alreadyPrompted = false;
        canvasManagerScript.HidePrompt();

        if (IsPlayerTurn())
        {
            canvasManagerScript.SetActive(true);
            canvasManagerScript.SetStatus("It's your turn!");
        }

        Player opponent = (ply == ply2) ? ply1 : ply2;
        card.PromptChosen(this, ply, opponent, index);
    }

    public void AnswerNumberCardPrompt(int owner, int index, PromptAbilityCard card) {
        Player ply = ply1;
        if (owner == 2) ply = ply2;

        AnswerNumberCardPrompt(ply, index, card);
    }

    public void AnswerNumberCardPrompt(Player owner, int index, PromptAbilityCard card)
    {
        if (owner.NumberCards[index] == null) return;

        alreadyPrompted = false;
        canvasManagerScript.HidePrompt();

        if (IsPlayerTurn())
        {
            canvasManagerScript.SetActive(true);
            canvasManagerScript.SetStatus("It's your turn!");
        }

        Player opponent = (owner == ply2) ? ply1 : ply2;
        card.PromptChosen(this, owner, opponent, index);
    }

    public GameObject InstantiateNumberCard(Transform parent) {
        return Instantiate(numberCard, parent);
    }

    public void RemoveAbilityCard(Player ply, int index) {
        ply.AbilityCards.RemoveAt(index);
    }

    public void RemoveNumberCard(int ply, int index) {
        Player chosenPly = ply1;
        if (ply == 2) chosenPly = ply2;
        RemoveNumberCard(chosenPly, index);
    }

    public void RemoveNumberCard(Player ply, int index) {
        GameObject cardsObject = playerNumberCards;
        if (ply == ply2) cardsObject = enemyNumberCards;
        
        Transform cardToRemove = cardsObject.transform.Find(index.ToString());
        
        if (cardToRemove != null) {
            Destroy(cardToRemove.gameObject);
        }
        
        for (int i = index + 1; i < ply.NumberCards.Count; i++) {
            Transform card = cardsObject.transform.Find(i.ToString());
            if (card != null) { card.gameObject.name = (i - 1).ToString(); }
        }
        ply.NumberCards.RemoveAt(index); RepositionCards(cardsObject.transform, false);
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

            if (isAbilityCard)
            {
                if (parent == enemyAbilityCards.transform)
                {
                    targetPos += new Vector3(-0.8f,0.2f,-5.5f);
                } else {
                    targetPos += new Vector3(-0.8f,0.2f,-9f);
                }
            }

            card.DOMove(targetPos, 0.5f)
                .SetEase(Ease.InOutSine);
        }
    }

    void DrawNumberCard(Player ply)
    {
        NumberCard card = deck.Draw();
        ply.NumberCards.Add(card);

        canvasManagerScript.CalculateText(ply1, ply2, true);

        GameObject parent = (ply == ply2) ? enemyNumberCards : playerNumberCards;
        GameObject cardRepresentation = Instantiate(numberCard, parent.transform);
        cardRepresentation.name = (ply.NumberCards.Count - 1).ToString();

        NumberCardVisuals cardVisualsScript = cardRepresentation.transform.Find("Card").gameObject.GetComponent<NumberCardVisuals>();
        cardVisualsScript.canvasManager = canvasManagerScript;

        RepositionCards(parent.transform, false);

        TMP_Text valueText = cardRepresentation.transform.Find("Card/Canvas/ValueLabel").GetComponent<TMP_Text>();
        TMP_Text damageText = cardRepresentation.transform.Find("Card/Canvas/DamageLabel").GetComponent<TMP_Text>();
        TMP_Text healthText = cardRepresentation.transform.Find("Card/Canvas/HealthLabel").GetComponent<TMP_Text>();

        if (ply == ply2 && ply2.NumberCards.Count == 1)
        {
            valueText.text = "?";
            damageText.text = "?";
            healthText.text = "?";
        }
        else
        {
            valueText.text = card.Value.ToString();
            damageText.text = card.Damage.ToString();
            healthText.text = card.Health.ToString();
        }
    }

    public void DrawAbilityCard(int ply, int index) {
        Player plyChosen = ply1;
        if (ply == 2) plyChosen = ply2;

        DrawAbilityCard(plyChosen, index);
    }

    public void DrawAbilityCard(Player ply, int index)
    {
        if (alreadyPrompted) return;
        if (abilityCardDebounce <= 0.75f) return;
        abilityCardDebounce = 0;

        ply.AbilityCards[index].Drawn = true;
        canvasManagerScript.WhiteFlash();

        Player opp = (ply == ply2) ? ply1 : ply2;
        ply.AbilityCards[index].Apply(this, ply, opp);

        if (ply == ply1)
            canvasManagerScript.RebuildInventoryPanel();

        Transform parent = (ply == ply2) ? enemyAbilityCards.transform : playerAbilityCards.transform;

        GameObject obj = Instantiate(abilityCard, parent);
        RepositionCards(parent.transform, true);

        TMP_Text txt = obj.transform.Find("Card/Canvas/NameLabel").GetComponent<TMP_Text>();
        txt.text = ply.AbilityCards[index].name;
    }

    void MakeGodCubesPlayAnimation(string animationName, Transform transform)
    {
        foreach (Transform child in transform)
        {
            Transform childTransform = child.Find("GodCube(Clone)");
            if (childTransform == null) continue;
            Transform cubeTransform = child.Find("GodCube(Clone)/Cube");
            childTransform.transform.rotation = Quaternion.identity;

            GameObject cube = cubeTransform.gameObject;
            Animator cubeAnimator = cube.GetComponent<Animator>();

            cubeAnimator.Play(animationName, 0, 0);
        }
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

    Player BlackjackWinner()
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

        int laneCount = attacker.NumberCards.Count;
        if (defender.NumberCards.Count < laneCount) laneCount = defender.NumberCards.Count;

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

    IEnumerator CombatSection()
    {
        phase = GamePhase.Combat;

        Player whoWon = DetermineBlackjackWinner();
        Player whoLost = ply1;

        ////////reveal opp card

        if (whoWon == ply1) whoLost = ply2;

        yield return new WaitForSeconds(1f);

        int YouDef = ply1.TotalHealth(BlackjackThreshold, false);
        int YouAtk = ply1.TotalDamage(false);
        int OppDef = ply2.TotalHealth(BlackjackThreshold, false);
        int OppAtk = ply2.TotalDamage(false);

        decidedSound.Play();

        if (whoWon == ply1)
        {
            if (OppDef - YouAtk >= 0)
            {
                
            } else
            {
                ply2.Life += OppDef - YouAtk; // formular for Ryzer Hp if you win
                ply1.Life -= OppDef - YouAtk; // formular for Your Hp if you win
            }

            StartCoroutine(Fight(whoWon));
            yield return new WaitForSeconds(2.5f);
            StartCoroutine(Fight(whoLost));
            Debug.Log("yeah");
        } else
        {
            if (YouDef - OppAtk >= 0)
            {
                
            } else
            {
                ply1.Life += YouDef - OppAtk; // formular for Your Hp if you lose
                ply2.Life -= YouDef - OppAtk; // formular for Ryzer Hp if you lose
            }
            StartCoroutine(Fight(whoLost));
            yield return new WaitForSeconds(2.5f);
            StartCoroutine(Fight(whoWon));
            Debug.Log("yeah2");
        }
        canvasManagerScript.SetHealth(ply1.Life, ply2.Life);
        yield return new WaitForSeconds(2f);
        musicController.ControlMusic(this);

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

                Transform abilityCardManifestation = playerAbilityCards.transform.Find(i.ToString());
                if (abilityCardManifestation != null) Destroy(abilityCardManifestation.gameObject);

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
                Transform cardRep = playerAbilityCards.transform.Find(i.ToString());
                if (cardRep == null) continue;
                Destroy(cardRep.gameObject);
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