using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    public GameObject PauseSunMusicObj;
    public GameObject PauseMoonMusicObj;
    public GameObject PauseDefaultMusicObj;
    public GameObject PauseOrNot;
    public MusicController musicController = new MusicController();
    public bool alreadyPrompted { get; set; }
    public CanvasGroup blackFade;
    public GameObject ConclusionText;
    public GameObject[] Male_Gods;
    public GameObject[] Female_Gods;
    public GameObject VictoryLossFrame;
    public GameObject PauseMenu;
    public GameObject BlowupParticle;
    public GameObject[] IntroText;
    public CanvasGroup IntroTextHolder;
    public CanvasGroup IntroDrawDisplay;
    public CanvasGroup IntroStayDisplay;
    public CanvasGroup IntroInvDisplay;
    public CanvasGroup IntroOptionsPanelDisplay;

    public GameObject[] EndingText;

    private AudioSource decidedSound;
    private AudioSource attackGodCubeSound;
    private Animator ConclusionTextAnimator;
    private List<AbilityCard> abilityCardList;
    private CanvasManager canvasManagerScript;
    private Coroutine IntroCoroutine;
    private int rounds = 0;
    private int turns = 0;

    bool IsPlayerTurn()
    {
        return phase == GamePhase.PlayerTurn;
    }
    void Start()
    {
        IntroCoroutine = StartCoroutine(IntroSequence());
    }
    IEnumerator IntroSequence()
    {
        IntroTextHolder.gameObject.SetActive(true);
        int linesGoneThrough = 0;
        foreach (GameObject obj in IntroText)
        {
            linesGoneThrough += 1;
            GameIntroEndingText introLines = obj.GetComponent<GameIntroEndingText>();
            if (introLines != null)
            {
                introLines.gameObject.SetActive(true);
                introLines.runRevealText();
            }
            if (linesGoneThrough == 3)
            {
                IntroDrawDisplay.DOFade(1f, 5f);
                yield return new WaitForSeconds(2.5f);
                IntroStayDisplay.DOFade(1f, 5f);
            }
            if (linesGoneThrough == 6)
            {
                IntroInvDisplay.DOFade(1f, 5f);
            }
            if (linesGoneThrough == 9)
            {
                yield return new WaitForSeconds(5f);
                blackFade.DOFade(0f, 10f);
                IntroTextHolder.DOFade(0f, 10f);
                IntroOptionsPanelDisplay.DOFade(0f, 10f);
                linesGoneThrough = 0;
                yield return new WaitForSeconds(10f);
                endIntroSequence();
            }
            yield return new WaitForSeconds(7f);
        }
    }
    public void endIntroSequence()
    {
        StopCoroutine(IntroCoroutine);
        foreach (GameObject obj in IntroText) {
            GameIntroEndingText introLines = obj.GetComponent<GameIntroEndingText>();
            if (obj != null) {
                StopCoroutine(introLines.RevealText());
            }
        }
        IntroTextHolder.gameObject.SetActive(false);
        IntroOptionsPanelDisplay.gameObject.SetActive(false);
        StartGame();
    }
    void StartGame()
    {
        StartCoroutine(unfadeBlack());
        alreadyPrompted = false;

        ConclusionTextAnimator = ConclusionText.GetComponent<Animator>();
        
        musicController.losingMusic = losingMusicObj;
        musicController.winningMusic = winningMusicObj;
        musicController.defaultMusic = defaultMusicObj;
        musicController.sunPause = PauseSunMusicObj;
        musicController.moonPause = PauseMoonMusicObj;
        musicController.defaultPause = PauseDefaultMusicObj;
        musicController.pauseornot = PauseOrNot;
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
    IEnumerator playEndingSequence()
    {
        foreach (GameObject obj in EndingText)
        {
            GameIntroEndingText endingLine = obj.GetComponent<GameIntroEndingText>();
            if (endingLine != null)
            {
                endingLine.gameObject.SetActive(true);
                endingLine.runRevealText();
            }
            yield return new WaitForSeconds(5f);
        }
    }
    IEnumerator TurnOffEndingText() {
        yield return new WaitForSeconds(58f);
        foreach (GameObject obj in EndingText) {
            if (obj != null) {
                obj.SetActive(false);
                yield return new WaitForSeconds(0.1f);
            }
        }
}
    IEnumerator unfadeBlack()
    {
        blackFade.DOFade(0f, 1.5f);
        yield return new WaitForSeconds(1.5f);
        blackFade.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
    }
    IEnumerator fadeBlack()
    {
        blackFade.gameObject.SetActive(true);
        blackFade.DOFade(1f, 1.5f);
        yield return new WaitForSeconds(0.5f);
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
        int availableCards = 0;

        foreach (AbilityCard card in ply.AbilityCards) {
            if (card.Drawn) continue;
            availableCards++;
        }

        if (availableCards < 5)
        {
            canvasManagerScript.inventoryButton.GetComponent<Animator>().Play("Notify", 0, 0);
            
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

        for (int i = 0; i < 3; i++) GivePlayerAbilityCard(ply1);
        for (int i = 0; i < 3; i++) GivePlayerAbilityCard(ply2);

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
        yield return new WaitForSeconds(2f);

        for (int i = ply2.AbilityCards.Count - 1; i >= 0; i--)
        {
            AbilityCard card = ply2.AbilityCards[i];
            if (card.Drawn) continue;
            if (!card.AIShouldDraw(this, ply2)) continue;

            DrawAbilityCard(ply2, i);
            yield return new WaitForSeconds(2f);
        }

        bool draw = ply2.BlackjackTotal(BlackjackThreshold, false) < BlackjackThreshold && ply2.NumberCards.Count < 6;

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

                if (ply1.NumberCards.Count >= 6 || ply1.BlackjackTotal(BlackjackThreshold, false) >= BlackjackThreshold)
                {
                    canvasManagerScript.ShowDrawButton(false);
                } else
                {
                    canvasManagerScript.ShowDrawButton(true);
                }
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

        int availableCards = 0;
        foreach (AbilityCard abilCard in owner.AbilityCards) {
            if (abilCard.Drawn) continue;
            availableCards++;
        }

        if (availableCards == 0) return;

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

        Transform parentCards = playerAbilityCards.transform;
        if (ply == ply2) parentCards = enemyAbilityCards.transform;

        Transform cardRepTransform = parentCards.Find(index.ToString());
        if (cardRepTransform == null) return;
        Destroy(cardRepTransform.gameObject);
        RepositionCards(parentCards);
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

        canvasManagerScript.CalculateText(ply1,ply2,true,true,0f);
        ply.NumberCards.RemoveAt(index);
        RepositionCards(cardsObject.transform);
    }

    public void RepositionCards(Transform parent)
    {
        int cardCount = 0;
        foreach (Transform child in parent) {
            if (!child) continue;
            cardCount++;
        }

        float normalSpacing = 1.2f;
        float compressedSpacing = 0.9f;
        float spacing = cardCount > 3 ? compressedSpacing : normalSpacing;

        float totalWidth = (cardCount - 1) * spacing;
        float startX = -totalWidth / 2f;

        int correctIndex = 0;

        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Transform child = parent.GetChild(i);
            if (child == null) continue;

            Transform container = child.Find("Container");
            if (container == null) continue;

            Vector3 targetPos = new Vector3(
                startX + (correctIndex * spacing),
                0f,
                0f
            );

            correctIndex++;

            container.DOKill();
            container.DOLocalMove(targetPos, 0.5f)
                .SetEase(Ease.InOutSine);
        }
    }

    public void UpdateNumberCardSuit(Player ply, int cardIndex, int suit) {
        if (ply == ply2 && cardIndex == 0) return;

        NumberCard card = ply.NumberCards[cardIndex];
        GameObject parent = (ply == ply2) ? enemyNumberCards : playerNumberCards;
        GameObject cardRepresentation = parent.transform.Find(cardIndex.ToString()).gameObject;

        card.Suit = suit;
        
        NumberCardVisuals cardVisualsScript = cardRepresentation.GetComponent<NumberCardVisuals>();
        cardVisualsScript.cardSuit = suit - 1;

        GameObject correctCard = cardVisualsScript.CardVariations[4];
        if (suit >= 0 && suit < cardVisualsScript.CardVariations.Length) correctCard = cardVisualsScript.CardVariations[suit-1];

        correctCard.SetActive(true);
        foreach (GameObject chosenCard in cardVisualsScript.CardVariations) {
            if (chosenCard == correctCard) continue;
            chosenCard.SetActive(false);
        }

        if (ply.NumberCards[cardIndex].Value == 12) {
            Renderer renderer = correctCard.GetComponent<Renderer>();
            Material[] mats = renderer.materials;
                
            int frontIndex = -1;
            int backIndex = -1;

            for (int i = 0; i < mats.Length; i++) {
                if (mats[i].name.Contains("-Front")) {
                    frontIndex = i;
                } else if (mats[i].name.Contains("-Back")) {
                    backIndex = i;
                }
            }

            if (frontIndex != -1 && backIndex != -1) {
                mats[frontIndex] = mats[backIndex];
                renderer.materials = mats;
            }
        }
    }

    public void DrawNumberCard(Player ply)
    {
        NumberCard card = deck.Draw();
        ply.NumberCards.Add(card);

        if (ply == ply1)
        {
            if (ply1.NumberCards.Count >= 6 || ply1.BlackjackTotal(BlackjackThreshold, false) >= BlackjackThreshold)
            {
                canvasManagerScript.ShowDrawButton(false);
            } else
            {
                canvasManagerScript.ShowDrawButton(true);
            }
        }

        canvasManagerScript.CalculateText(ply1, ply2, true);

        GameObject parent = (ply == ply2) ? enemyNumberCards : playerNumberCards;
        GameObject cardRepresentation = Instantiate(numberCard, parent.transform);
        cardRepresentation.name = (ply.NumberCards.Count - 1).ToString();

        NumberCardVisuals cardVisualsScript = cardRepresentation.GetComponent<NumberCardVisuals>();

        if (ply.NumberCards.Count == 1 && ply == ply2) {
            cardVisualsScript.cardSuit = 978;
        } else {
            cardVisualsScript.cardSuit = card.Suit - 1;
        }

        RepositionCards(parent.transform);

        TMP_Text valueText = cardRepresentation.transform.Find("Container/Canvas/ValueText").GetComponent<TMP_Text>();
        TMP_Text valueShadowText = cardRepresentation.transform.Find("Container/Canvas/ValueTextShadow").GetComponent<TMP_Text>();
        TMP_Text damageText = cardRepresentation.transform.Find("Container/Canvas/AttackValue").GetComponent<TMP_Text>();
        TMP_Text healthText = cardRepresentation.transform.Find("Container/Canvas/DefendValue").GetComponent<TMP_Text>();

        if (ply == ply2 && ply2.NumberCards.Count == 1)
        {
            valueText.text = "?";
            valueShadowText.text = "?";
            damageText.text = "?";
            healthText.text = "?";
        }
        else
        {
            if (card.Value == 12) {
                valueText.text = "";
                valueShadowText.text = "";

                GameObject cardModel = cardVisualsScript.CardVariations[cardVisualsScript.cardSuit];
                Renderer renderer = cardModel.GetComponent<Renderer>();
                Material[] mats = renderer.materials;
                
                int frontIndex = -1;
                int backIndex = -1;

                for (int i = 0; i < mats.Length; i++) {
                    if (mats[i].name.Contains("-Front")) {
                        frontIndex = i;
                    } else if (mats[i].name.Contains("-Back")) {
                        backIndex = i;
                    }
                }

                if (frontIndex != -1 && backIndex != -1) {
                    mats[frontIndex] = mats[backIndex];
                    renderer.materials = mats;
                }
            } else {
                valueText.text = card.Value.ToString();
                valueShadowText.text = card.Value.ToString();
            }
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

        AbilityCard abilityCardPlayer = ply.AbilityCards[index];

        abilityCardPlayer.Drawn = true;
        canvasManagerScript.WhiteFlash();

        Player opp = (ply == ply2) ? ply1 : ply2;
        abilityCardPlayer.Apply(this, ply, opp);

        if (ply == ply1)
            canvasManagerScript.RebuildInventoryPanel();

        Transform parent = (ply == ply2) ? enemyAbilityCards.transform : playerAbilityCards.transform;

        GameObject obj = Instantiate(abilityCard, parent);
        obj.name = index.ToString();

        Image img = obj.transform.Find("Container/Card/Canvas/Image").gameObject.GetComponent<Image>();
        img.sprite = abilityCardPlayer.icon;
        img.preserveAspect = true;

        RepositionCards(parent.transform);
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
        if (attacker != null) {

            GameObject physicalAttackerCards = playerNumberCards;
            GameObject physicalDefenderCards = enemyNumberCards;

            GameObject hiddenCard = enemyNumberCards.transform.Find("0").gameObject;
            NumberCard enemyFirstCard = ply2.NumberCards[0];

            TMP_Text valueText =
                hiddenCard.transform.Find("Container/Canvas/ValueText")
                .GetComponent<TMP_Text>();

            TMP_Text valueShadowText =
                hiddenCard.transform.Find("Container/Canvas/ValueTextShadow")
                .GetComponent<TMP_Text>();

            TMP_Text damageText =
                hiddenCard.transform.Find("Container/Canvas/AttackValue")
                .GetComponent<TMP_Text>();

            TMP_Text healthText =
                hiddenCard.transform.Find("Container/Canvas/DefendValue")
                .GetComponent<TMP_Text>();

            if (enemyFirstCard.Value == 12) {
                valueText.text = "A";
                valueShadowText.text = "A";
            } else {
                valueText.text = enemyFirstCard.Value.ToString();
                valueShadowText.text = enemyFirstCard.Value.ToString();
            }
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

        yield return new WaitForSeconds(0.6f);
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

    void SummonGods(Player ply) {
        GameObject parent = playerNumberCards;
        if (ply == ply2) parent = enemyNumberCards;

        for (int i = 0; i < ply.NumberCards.Count; i++) {
            NumberCard card = ply.NumberCards[i];
            GameObject cardRepresentation = parent.transform.Find(i.ToString()).gameObject;
            NumberCardVisuals cardVisuals = cardRepresentation.GetComponent<NumberCardVisuals>();

            GameObject chosenGodModel;
            if (Random.value <= 0.5) {
                chosenGodModel = Male_Gods[card.Suit-1];
            } else {
                chosenGodModel = Female_Gods[card.Suit-1];
            }
            GameObject godCopy = Instantiate(chosenGodModel, cardVisuals.GodPos);
            cardVisuals.GodModel = godCopy;
        }
    }

    void PlayAttackAnimations(Player ply) {
        string animName = "Attack";
        GameObject parent = playerNumberCards;
        if (ply == ply2) {
            parent = enemyNumberCards;
            animName = "AttackEnemy";
        }

        for (int i = 0; i < ply.NumberCards.Count; i++) {
            NumberCard card = ply.NumberCards[i];
            GameObject cardRepresentation = parent.transform.Find(i.ToString()).gameObject;
            Animator cardAnimator = cardRepresentation.GetComponent<Animator>();
            cardAnimator.Play(animName, 0, 0);
        }
    }

    public void ReturnToMenu() {
        SceneManager.LoadScene(0);
    }

    IEnumerator CombatSection()
    {
        Player whoWon = DetermineBlackjackWinner();
        Player whoLost = ply1;
        if (whoWon == ply1) whoLost = ply2;
        
        // Reveal card
        GameObject cardRepresentation = enemyNumberCards.transform.Find("0").gameObject;
        NumberCardVisuals cardVisualsScript = cardRepresentation.GetComponent<NumberCardVisuals>();

        GameObject correctCard = cardVisualsScript.CardVariations[ply2.NumberCards[0].Suit-1];
        correctCard.SetActive(true);
        foreach (GameObject chosenCard in cardVisualsScript.CardVariations) {
            if (chosenCard == correctCard) continue;
            chosenCard.SetActive(false);
        }

        TMP_Text healthText = cardRepresentation.transform.Find("Container/Canvas/DefendValue").GetComponent<TMP_Text>();
        TMP_Text attackText = cardRepresentation.transform.Find("Container/Canvas/AttackValue").GetComponent<TMP_Text>();
        TMP_Text valueText = cardRepresentation.transform.Find("Container/Canvas/ValueText").GetComponent<TMP_Text>();
        TMP_Text valueShadowText = cardRepresentation.transform.Find("Container/Canvas/ValueTextShadow").GetComponent<TMP_Text>();

        if (ply2.NumberCards[0].Value == 12) {
            GameObject cardModel = cardVisualsScript.CardVariations[ply2.NumberCards[0].Suit-1];
            Renderer renderer = cardModel.GetComponent<Renderer>();
            Material[] mats = renderer.materials;
                
            int frontIndex = -1;
            int backIndex = -1;

            for (int i = 0; i < mats.Length; i++) {
                if (mats[i].name.Contains("-Front")) {
                    frontIndex = i;
                } else if (mats[i].name.Contains("-Back")) {
                    backIndex = i;
                }
            }

            if (frontIndex != -1 && backIndex != -1) {
                mats[frontIndex] = mats[backIndex];
                renderer.materials = mats;
            }
        } else {
            valueText.text = ply2.NumberCards[0].Value.ToString();
        }
        valueShadowText.text = valueText.text;
        healthText.text = ply2.NumberCards[0].Health.ToString();
        attackText.text = ply2.NumberCards[0].Damage.ToString();

        ConclusionText.SetActive(true);
        blackFade.gameObject.SetActive(true);
        blackFade.DOFade(0.5f, 0.3f);
        ConclusionTextAnimator.Play("Conclusion", 0, 0);

        TMP_Text conclusiontxtcomponent = ConclusionText.GetComponent<TMP_Text>();
        if (whoWon == ply2) {
            conclusiontxtcomponent.text = "You lost!";
            conclusiontxtcomponent.color = new Color(1f, 0.98f, 0f, 1f);
        } else if (whoWon == ply1) {
            conclusiontxtcomponent.text = "You won!";
            conclusiontxtcomponent.color = new Color(0f, 0.85f, 1f, 1f);
        } else {
            conclusiontxtcomponent.text = "Nobody won!";
            conclusiontxtcomponent.color = new Color(1f, 1f, 1f, 1f);
        }

        canvasManagerScript.CalculateText(ply1,ply2,false,false,0f);
        decidedSound.Play();
        phase = GamePhase.Combat;

        yield return new WaitForSeconds(4f);
        blackFade.DOFade(0f, 0.3f);
        blackFade.gameObject.SetActive(false);
        ConclusionText.SetActive(false);

        bool stop = false;
        if (whoWon != null) {
            GetComponents<AudioSource>()[2].Play();
            canvasManagerScript.WhiteFlash();

            SummonGods(ply1);
            SummonGods(ply2);

            yield return new WaitForSeconds(1f);
            PlayAttackAnimations(whoWon);

            AudioSource boomSound = GetComponents<AudioSource>()[1];
            boomSound.PlayOneShot(boomSound.clip);

            yield return new WaitForSeconds(0.28f);

            BlowupParticle.GetComponent<ParticleSystem>().Emit(15);

            yield return new WaitForSeconds(1.2f);

            PlayAttackAnimations(whoLost);
            boomSound.PlayOneShot(boomSound.clip);

            yield return new WaitForSeconds(0.28f);

            BlowupParticle.GetComponent<ParticleSystem>().Emit(15);

            yield return new WaitForSeconds(0.5f);

            int WinnerDef = whoWon.TotalHealth(BlackjackThreshold, false);
            int WinnerAtk = whoWon.TotalDamage(false);
            int LoserDef = whoLost.TotalHealth(BlackjackThreshold, false);
            int LoserAtk = whoLost.TotalDamage(false);

            if (LoserDef - WinnerAtk < 0) {
                whoLost.Life += LoserDef - WinnerAtk;
                whoWon.Life -= LoserDef - WinnerAtk;
            }

            canvasManagerScript.SetHealth(ply1.Life, ply2.Life);
            if (ply2.Life > 0) {
            musicController.ControlMusic(this);
            }

            yield return new WaitForSeconds(1f);

            if (ply1.Life <= 0) {
                VictoryLossFrame.SetActive(true);
                VictoryLossFrame.transform.Find("Text (TMP)").gameObject.GetComponent<TMP_Text>().text = "YOU LOST...\nBetter luck next time!";

                stop = true;
                Destroy(PauseMenu);
            } else if (ply2.Life <= 0) {
                //StartCoroutine(StartNewRound());
                StartCoroutine(fadeBlack());
                yield return new WaitForSeconds(3f);
                musicController.FinaleMusic(this);
                StartCoroutine(playEndingSequence());
                StartCoroutine(TurnOffEndingText());
                yield return new WaitForSeconds(60f);
                StartCoroutine(TurnOffEndingText());
                VictoryLossFrame.SetActive(true);
                stop = true;
                Destroy(PauseMenu);
            }
        }

        if (!stop) {

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

            Cleanup();
            BlackjackThreshold = 21;
            StartCoroutine(StartNewRound());
        }
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