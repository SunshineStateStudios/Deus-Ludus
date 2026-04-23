using UnityEngine;
using TMPro;

public class AbilitySun : AbilityCard
{
    public override string name => "Sun";
    public override string description => "Doubles the <color=#ff8282>attack<color=#ffffff> of all <color=#27A6F5>Mayan</color> number cards you've drawn.";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        CanvasManager canvasMngr = gm.canvasObject.GetComponent<CanvasManager>();
        for (int i = 0; i < owner.NumberCards.Count; i++)
        {
            NumberCard card = owner.NumberCards[i];
            if (card.Suit != 1) continue;
            card.Damage = card.Damage * 2;

            GameObject parent = gm.playerNumberCards;
            if (owner == gm.ply2) parent = gm.enemyNumberCards;

            Transform cardRepresentation = parent.transform.Find(i.ToString());
            if (cardRepresentation == null) continue;

            TMP_Text damageText =
                cardRepresentation.Find("Card/Canvas/DamageLabel")
                    .GetComponent<TMP_Text>();

            damageText.text = card.Damage.ToString();
        }
        canvasMngr.CalculateText(gm.ply1, gm.ply2, true);
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {
        gm.BlackjackThreshold -= 3;
    }

    public override bool AIShouldDraw(GameManager gm, Player owner)
    {
        return true;
    }
}