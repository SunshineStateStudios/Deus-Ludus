using UnityEngine;
using TMPro;

public class AbilityChariot : AbilityCard
{
    public override string name => "Chariot";
    public override string description => "Remove opponent's most recently played passive";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        Transform parentCards = gm.playerAbilityCards.transform;
        if (owner == gm.ply2) parentCards = gm.enemyAbilityCards.transform;

        for (int i = opponent.AbilityCards.Count-1; i >= 0; i--)
        {
            AbilityCard opponentsLastUsedPassive = opponent.AbilityCards[i];
            if (opponentsLastUsedPassive.Drawn){
            opponent.AbilityCards.RemoveAt(i);
            GameObject cardRepresentation = parentCards.Find(i.ToString()).gameObject;
            DestroyObj DestroyObjScript = cardRepresentation.GetComponent<DestroyObj>();
            DestroyObjScript.Begone();
            }
        }
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {
        
    }

    public override bool AIShouldDraw(GameManager gm, Player owner)
    {
        return true;
    }
}