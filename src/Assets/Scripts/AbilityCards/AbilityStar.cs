using UnityEngine;
using TMPro;

public class AbilityStar : AbilityCard
{
    public override string name => "Star";
    public override string description => "Doubles the <color=#8787ff>defence<color=#ffffff> of all <color=#FFF700>Egyptian</color> number cards you've drawn.";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        CanvasManager canvasMngr = gm.canvasObject.GetComponent<CanvasManager>();
        for (int i = 0; i < opponent.NumberCards.Count; i++)
        {
            NumberCard card = opponent.NumberCards[i];
            if (card.Suit != 3) continue;
            card.Health = card.Health * 2;

            GameObject parent = gm.playerNumberCards;
            if (owner == gm.ply2) parent = gm.enemyNumberCards;

            Transform cardRepresentation = parent.transform.Find(i.ToString());
        
            if (parent == gm.enemyNumberCards && i == 0) continue;

            Transform HealthLabel = cardRepresentation.transform.Find("Container/Canvas/DefendValue");
            if (HealthLabel == null) continue;

            TMP_Text healthText =
                HealthLabel.gameObject
                .GetComponent<TMP_Text>();

            healthText.text = card.Health.ToString();
        }
        canvasMngr.CalculateText(gm.ply1, gm.ply2, true);
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {
        gm.BlackjackThreshold -= 3;
    }

    public override bool AIShouldDraw(GameManager gm, Player owner)
    {
        int totalImpact = 0;
        int egyptianCount = 0;

        foreach (NumberCard card in owner.NumberCards)
        {
            if (card.Suit != 3)
                continue;

            egyptianCount++;
            totalImpact += card.Health;
        }

        if (egyptianCount == 0)
            return false;

        if (egyptianCount == 1 && totalImpact < 3)
            return false;

        return totalImpact >= 4;
    }
}