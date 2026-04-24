using UnityEngine;
using TMPro;

//To whoever finds this, understand one thing.
//Ryzer, your opponent, is not your enemy, he is you, a future you, you are fighting yourself.
//Regardless of the outcome it is a losing battle.
//You will defeat him and then you will become him, turn back Now
public class AbilityChariot : AbilityCard
{
    public override string name => "Chariot";
    public override string description => "Remove <color=#ff8282>opponent's<color=#ffffff> most recently drawn <color=#ff8282>number</color> card.";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        Transform parentCards = gm.playerAbilityCards.transform;
        if (owner == gm.ply2) parentCards = gm.enemyAbilityCards.transform;

        for (int i = opponent.AbilityCards.Count-1; i >= 0; i--) {
            AbilityCard abilityCard = opponent.AbilityCards[i];
            if (!abilityCard.Drawn) continue;

            gm.RemoveAbilityCard(opponent, i);
            break;
        }
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {
        
    }

    public override bool AIShouldDraw(GameManager gm, Player owner)
    {
        Player opponent = gm.ply2;
        if (owner == gm.ply2) opponent = gm.ply1;

        int count = 0;

        for (int i = 0; i < opponent.AbilityCards.Count; i++) {
            if (opponent.AbilityCards[i].Drawn) {
                count++;
            }
        }

        return count > 1;
    }
}