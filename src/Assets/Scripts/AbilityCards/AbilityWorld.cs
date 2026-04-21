using UnityEngine;
using TMPro;

public class AbilityWorld : AbilityCard
{
    public override string name => "World";
    public override string description => "Set the <color=#ff8282>attack<color=#ffffff> of the opponent's <color=#fff0c4>Greek</color> number cards to 1";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        for (int i = 0; i < opponent.NumberCards.Count; i++)
        {
            NumberCard card = opponent.NumberCards[i];
            if (card.Suit != 4) continue;
            card.Damage = 1;

            GameObject parent = gm.enemyNumberCards;
            if (owner == gm.ply2) parent = gm.playerNumberCards;

            Transform cardRepresentation = parent.transform.Find(i.ToString());
        
            if (parent == gm.enemyNumberCards && i == 0) continue;

            TMP_Text damageText =
                cardRepresentation.transform.Find("Card/Canvas/DamageLabel")
                .GetComponent<TMP_Text>();

            damageText.text = card.Damage.ToString();
        }
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {
        
    }

    public override bool AIShouldDraw(GameManager gm, Player owner)
    {
        Player opponent = (owner == gm.ply1) ? gm.ply2 : gm.ply1;

        for (int i = 0; i < opponent.NumberCards.Count; i++)
        {
            NumberCard card = opponent.NumberCards[i];

            if (card.Suit == 4 && card.Damage > 1)
            {
                return true;
            }
        }

        return false;
    }
}