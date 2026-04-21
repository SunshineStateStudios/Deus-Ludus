using UnityEngine;
using TMPro;

public class AbilityJudgement : PromptAbilityCard
{
    public override string name => "Judgement"; //must be edited later to give the player a choice
    public override string description => "Randomly alter the <color=#ff8282>suit</color> of a Number Card you choose.";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        gm.PromptForNumberCard(owner, this);
    }

    public override void PromptChosen(GameManager gm, Player owner, Player opponent, int indexChosen)
    {
        owner.NumberCards[indexChosen].Suit = Random.Range(1,4);
        gm.UpdateNumberCardSuit(owner, indexChosen, owner.NumberCards[indexChosen].Suit);
    }

    public override int AICardDecision(Player player) {
        return Random.Range(0, player.NumberCards.Count);
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {

    }

    public override bool AIShouldDraw(GameManager gm, Player owner)
    {
        return true;
    }
}
