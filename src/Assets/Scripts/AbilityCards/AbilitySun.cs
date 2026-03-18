using UnityEngine;
using TMPro;

public class AbilitySun : AbilityCard
{
    public override string name => "Sun";
    public override string description => "Doubles the attack of all Roman number cards you've drawn.";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        for (int i = 0; i < opponent.NumberCards.Count; i++)
        {
            NumberCard card = opponent.NumberCards[i];
            if (card.Suit != 1) continue;
            card.Damage = card.Damage * 2;

            GameObject parent = gm.playerNumberCards;
            if (owner == gm.ply2) parent = gm.enemyNumberCards;

            Transform cardRepresentation = parent.transform.Find(i.ToString());
        
            if (parent == gm.enemyNumberCards && i == 0) continue;

            TMP_Text damageText =
                cardRepresentation.transform.Find("Card/Canvas/DamageLabel")
                    .GetComponent<TMP_Text>();

            damageText.text = card.Damage.ToString();
        }

        gm.UpdateAttackDefendText();
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