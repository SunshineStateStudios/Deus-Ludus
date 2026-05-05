using UnityEngine;
using TMPro;

public class AbilityLovers : AbilityCard
{
    public override string name => "Lovers";
    public override string description => "Replaces this card with a random <color=#ff8282>ability</color> card if the pool has any.";
    public override int triesDecayTime => 1;

    void GiveCard(GameManager gm, Player ply) {
        while (true) {
            AbilityCard chosenCard = gm.GivePlayerAbilityCard(ply);
            if (chosenCard != null) {
                if (!chosenCard.GetType().Name.Equals("AbilityLovers")) {
                    break;
                }
            }
        }
    }

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        gm.GivePlayerAbilityCard(owner);
        gm.canvasObject.GetComponent<CanvasManager>().RebuildInventoryPanel();
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {

    }

    public override bool AIShouldDraw(GameManager gm, Player owner)
    {
        Player opponent = gm.ply1;

        if (owner.AbilityCards.Count <= 1)
            return false;

        int myAbilities = owner.AbilityCards.Count;
        int oppAbilities = opponent.AbilityCards.Count;

        if (oppAbilities >= myAbilities)
            return false;

        if (myAbilities + 2 <= oppAbilities)
            return true;

        return Random.value < 0.25f;
    }
}