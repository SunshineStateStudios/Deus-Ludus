using UnityEngine;
using TMPro;

public class AbilityHangedMan : AbilityCard
{
    public override string name => "Hanged Man";
    public override string description => "Discard the Number Card the Opponent holds with the <color=#ff8282>highest attack</color>";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        CanvasManager canvasMngr = gm.canvasObject.GetComponent<CanvasManager>();
        int chosenIndex = 0;

        for (int i = 0; i < opponent.NumberCards.Count; i++) {
            NumberCard currentCard = opponent.NumberCards[i];
            NumberCard oldCard = opponent.NumberCards[chosenIndex];

            if (currentCard.Damage > oldCard.Damage) {
                chosenIndex = i;
            }
        }

        Transform parent = gm.enemyNumberCards.transform;
        if (owner == gm.ply2) parent = gm.playerNumberCards.transform;

        gm.RemoveNumberCard(opponent, chosenIndex);
        gm.RepositionCards(parent);
        canvasMngr.CalculateText(gm.ply1, gm.ply2, true, true);
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {

    }

    public override bool AIShouldDraw(GameManager gm, Player owner)
    {
        Player opponent = gm.ply2;
        if (owner == gm.ply2) opponent = gm.ply1;

        if (opponent.NumberCards.Count == 0)
            return false;

        int maxThreat = 0;
        foreach (NumberCard card in opponent.NumberCards)
        {
            int threat = card.Damage * card.Health;
            if (threat > maxThreat)
                maxThreat = threat;
        }

        return maxThreat >= 4;
    }
}