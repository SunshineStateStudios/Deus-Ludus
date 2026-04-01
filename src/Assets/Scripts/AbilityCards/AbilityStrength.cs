using UnityEngine;
using TMPro;

public class AbilityStrength : PromptAbilityCard
{
    public override string name => "Strength";
    public override string description => "Pick a number card you hold to double its attack.";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        gm.PromptForNumberCard(owner, "Choose a Number Card!", "The Strength card calls for it...", this);
    }

    public override void PromptChosen(GameManager gm, Player owner, Player opponent, int indexChosen)
    {
        owner.NumberCards[indexChosen].Damage *= 2;
    }

    public override int AICardDecision(Player player) {
        int chosenIndex = 0;

        for (int i = 1; i < player.NumberCards.Count; i++) {
            NumberCard oldCard = player.NumberCards[chosenIndex];
            NumberCard currentCard = player.NumberCards[i];

            if (currentCard.Damage > oldCard.Damage) {
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
        return true;
    }
}
