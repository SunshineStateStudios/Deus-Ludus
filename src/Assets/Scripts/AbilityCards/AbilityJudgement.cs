using UnityEngine;
using TMPro;

public class AbilityJudgement : AbilityCard
{
    public override string name => "Judgement"; //must be edited later to give the player a choice
    public override string description => "Randomly alter the suit of a Number Card you hold";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        NumberCard chosenCard = owner.NumberCards[Random.Range(0,owner.NumberCards.Count)];
        int originalSuit = chosenCard.Suit;

        while (true)
        {
            int chosenSuit = Random.Range(1,5);
            Debug.Log(chosenSuit);

            if (originalSuit != chosenSuit)
            {
                originalSuit = chosenSuit;
                break;
            }
        }

        chosenCard.Value = originalSuit;
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {

    }

    public override bool AIShouldDraw(GameManager gm, Player owner)
    {
        return true;
    }
}