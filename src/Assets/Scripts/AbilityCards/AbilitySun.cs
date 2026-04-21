using UnityEngine;
using TMPro;

public class AbilitySun : AbilityCard
{
    public override string name => "Sun";
    public override string description => "Doubles the attack of all Mayan number cards you've drawn.";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
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