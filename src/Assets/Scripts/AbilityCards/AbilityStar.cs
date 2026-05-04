using UnityEngine;
using TMPro;

public class AbilityStar : AbilityCard
{
    public override string name => "Star";
    public override string description => "Doubles the <color=#8787ff>defence<color=#ffffff> of all <color=#FFF700>Egyptian</color> number cards you've drawn.";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        Transform parent = gm.playerNumberCards.transform;
        if (owner == gm.ply2) parent = gm.enemyNumberCards.transform;

        CanvasManager canvasMngr = gm.canvasObject.GetComponent<CanvasManager>();
        for (int i = 0; i < owner.NumberCards.Count; i++)
        {
            if (i == 0 && owner == gm.ply2) continue;
            NumberCard card = owner.NumberCards[i];
            if (card.Suit != 3) continue;
            card.Health = card.Health * 2;

            parent.Find(i.ToString() + "/Container/Canvas/DefendValue").GetComponent<TMP_Text>().text = card.Health.ToString();
        }
        canvasMngr.CalculateText(gm.ply1, gm.ply2, true, true, 0f);
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {
        
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