using UnityEngine;
using TMPro;

public class AbilityWorld : AbilityCard
{
    public override string name => "World";
    public override string description => "Set the attack of the opponent's Greek elementals to 1";
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
        gm.BlackjackThreshold -= 3;
    }

    public override bool AIShouldDraw(GameManager gm, Player owner)
    {
        return true;
    }
}