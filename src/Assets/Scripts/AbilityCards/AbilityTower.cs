using UnityEngine;
using TMPro;

public class AbilityTower : PromptAbilityCard
{
    public override string name => "Tower";
    public override string description => "Choose a Number Card you hold to copy the highest defense the opponent has.";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        gm.PromptForNumberCard(owner, this);
    }

    public override void PromptChosen(GameManager gm, Player owner, Player opponent, int indexChosen)
    {
        NumberCard chosenCard = owner.NumberCards[indexChosen];
        NumberCard oppCard = opponent.NumberCards[0];

        GameObject parent = gm.playerNumberCards;
        if (owner == gm.ply2) parent = gm.enemyNumberCards;

        foreach (NumberCard card in opponent.NumberCards) {
            if (card.Health > oppCard.Health) {
                oppCard = card;
            }
        }

        chosenCard.Health = oppCard.Health;

        Transform cardRepresentation = parent.transform.Find(indexChosen.ToString());
        TMP_Text healthText =
            cardRepresentation.transform.Find("Card/Canvas/HealthLabel")
            .GetComponent<TMP_Text>();

        healthText.text = oppCard.Health.ToString();
    }

    public override int AICardDecision(Player player) {
        int chosenIndex = 0;

        for (int i = 1; i < player.NumberCards.Count; i++) {
            NumberCard oldCard = player.NumberCards[chosenIndex];
            NumberCard currentCard = player.NumberCards[i];

            if (currentCard.Damage < oldCard.Damage) {
                chosenIndex = i;
            }
        }

        return chosenIndex;
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {

    }

    public override bool AIShouldDraw(GameManager gm, Player owner)
    {
        Player opponent = gm.ply2;
        if (owner == gm.ply1) opponent = gm.ply1;

        int highestOpponentDefense = 0;
        foreach (NumberCard card in opponent.NumberCards)
        {
            if (card.Health > highestOpponentDefense)
            {
                highestOpponentDefense = card.Health;
            }
        }

        int highestAIHealth = 0;
        foreach (NumberCard card in owner.NumberCards)
        {
            if (card.Health > highestAIHealth)
            {
                highestAIHealth = card.Health;
            }
        }

        // The AI should draw the card to match or surpass the opponent's defense
        if (highestOpponentDefense > highestAIHealth) return true;

        // Optionally, the AI can also draw if it has a weak overall defense and the opponent has a strong defense
        int totalAIHealth = 0;
        foreach (NumberCard card in owner.NumberCards)
        {
            totalAIHealth += card.Health;
        }

        int totalOpponentHealth = 0;
        foreach (NumberCard card in opponent.NumberCards)
        {
            totalOpponentHealth += card.Health;
        }

        // If the opponent's total health is significantly higher, the AI may want to draw the card
        if (totalOpponentHealth > totalAIHealth)
        {
            return true;
        }

        // Otherwise, the AI doesn't need to draw the card
        return false;
    }
}
