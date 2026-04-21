using UnityEngine;
using TMPro;

public class AbilityWheelOfFortune : AbilityCard
{
    public override string name => "The Wheel of Fortune";
    public override string description => "50% chance to either triple the attack of your latest number card or set the attack of your latest number card to 0";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        NumberCard chosenNumberCard = owner.NumberCards[owner.NumberCards.Count-1];

        if (Random.value <= 0.5) {
            chosenNumberCard.Damage *= 3;
        } else {
            chosenNumberCard.Damage = 0;
        }

        GameObject parent = gm.playerNumberCards;
        if (owner == gm.ply2) parent = gm.enemyNumberCards;

        Transform cardRepresentation = parent.transform.Find((owner.NumberCards.Count-1).ToString());
        TMP_Text damageText =
            cardRepresentation.transform.Find("Card/Canvas/DamageLabel")
            .GetComponent<TMP_Text>();
        damageText.text = chosenNumberCard.Damage.ToString();
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {
        
    }

    public override bool AIShouldDraw(GameManager gm, Player owner)
    {
        return Random.Range(1,2) == 1;
    }
}