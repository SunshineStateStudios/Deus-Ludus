using UnityEngine;
using TMPro;

public class AbilityMoon : AbilityCard
{
    public override string name => "Moon";
    public override string description => "Sets the defence of all of the opponent's Japanese number cards to 1.";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        for (int i = 0; i < opponent.NumberCards.Count; i++)
        {
            NumberCard card = opponent.NumberCards[i];
            if (card.Suit != 2) continue;
            card.Health = 1;

            GameObject parent = gm.enemyNumberCards;
            if (owner == gm.ply2) parent = gm.playerNumberCards;

            Transform cardRepresentation = parent.transform.Find(i.ToString());
        
            if (parent == gm.enemyNumberCards && i == 0) continue;

            TMP_Text healthText =
                cardRepresentation.transform.Find("Card/Canvas/HealthLabel")
                .GetComponent<TMP_Text>();

            healthText.text = card.Health.ToString();
        }
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {
        gm.BlackjackThreshold -= 3;
    }

    public override bool AIShouldDraw(GameManager gm, Player owner)
    {
        Player opponent = gm.ply2;
        if (owner == gm.ply2) opponent = gm.ply1;

        bool hasValidTargets = false;
        int totalImpact = 0;

        foreach (NumberCard card in opponent.NumberCards)
        {
            if (card.Suit != 2)
                continue;

            hasValidTargets = true;
            totalImpact += Mathf.Max(0, card.Health - 1);
        }

        if (!hasValidTargets)
            return false;

        return totalImpact >= 2;
    }
}