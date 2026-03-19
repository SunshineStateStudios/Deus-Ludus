using UnityEngine;
using TMPro;

public class AbilityHangedMan : AbilityCard
{
    public override string name => "Hanged Man";
    public override string description => "Discard the Number Card the Opponent holds with the highest attack";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        int chosenIndex = 0;

        for (int i = 0; i < opponent.NumberCards.Count; i++) {
            NumberCard currentCard = opponent.NumberCards[i];
            NumberCard oldCard = opponent.NumberCards[chosenIndex];

            if (currentCard.Damage > oldCard.Damage) {
                chosenIndex = i;
            }
        }

        gm.RemoveNumberCard(opponent, chosenIndex);
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {

    }

    public override bool AIShouldDraw(GameManager gm, Player owner)
    {
        return true;
    }
}