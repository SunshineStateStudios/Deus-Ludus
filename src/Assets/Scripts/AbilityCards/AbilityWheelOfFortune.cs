using UnityEngine;
using TMPro;

public class AbilityWheelOfFortune : AbilityCard
{
    public override string name => "The Wheel of Fortune";
    public override string description => "<color=#ff8282>50%<color=#ffffff> chance to either <color=#42f548>triple the attack of your latest number card<color=#ffffff> or <color=#ff2e1f>set the attack of your latest number card to 0";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        CanvasManager canvasMngr = gm.canvasObject.GetComponent<CanvasManager>();
        NumberCard chosenNumberCard = owner.NumberCards[owner.NumberCards.Count-1];

        if (Random.Range(1,2) == 1) {
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
        canvasMngr.CalculateText(gm.ply1, gm.ply2, true);
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {
        
    }

    public override bool AIShouldDraw(GameManager gm, Player owner)
    {
        return Random.Range(1,2) == 1;
    }
}