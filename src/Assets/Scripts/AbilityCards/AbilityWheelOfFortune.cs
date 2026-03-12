using UnityEngine;
using TMPro;

public class AbilityWheelOfFortune : AbilityCard
{
    public override string name => "The Wheel of Fortune";
    public override string description => "1/2 chance to either triple the attack of your latest number card or set the attack of your latest number card to 0";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        NumberCard chosenNumberCard = owner.NumberCards[owner.NumberCards.Count-1];

        if (Random.Range(1,2) == 1) {
            chosenNumberCard.Damage *= 3;
        } else {
            chosenNumberCard.Damage = 0;
        }
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {
        
    }

    public override bool AIShouldDraw(GameManager gm, Player owner)
    {
        return true;
    }
}