using UnityEngine;
using TMPro;

public class AbilityStar : AbilityCard
{
    public override string name => "Star";
    public override string description => "Doubles the defence of all Norse number cards you've drawn.";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        for (int i = 0; i < opponent.NumberCards.Count; i++)
        {
            NumberCard card = opponent.NumberCards[i];
            if (card.Suit != 2) continue;
            card.Health = card.Health * 2;

            GameObject parent = gm.playerNumberCards;
            if (owner == gm.ply2) parent = gm.enemyNumberCards;

            Transform cardRepresentation = parent.transform.Find(i.ToString());
        
            if (parent == gm.enemyNumberCards && i == 0) continue;

            TMP_Text healthText =
                cardRepresentation.transform.Find("Card/Canvas/HealthLabel")
                .GetComponent<TMP_Text>();

            healthText.text = card.Health.ToString();
        }

        gm.UpdateAttackDefendText();
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {
        gm.BlackjackThreshold -= 3;
    }

    public override bool AIShouldDraw(GameManager gm, Player owner)
    {
        return true; // Always draw
    }
}