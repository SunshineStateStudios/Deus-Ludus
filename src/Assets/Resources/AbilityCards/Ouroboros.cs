/*

"Ouroboros" ability card. Passive ability card that adds threshold by 3.

Written by plexinator-9000
*/

using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Ouroboros", menuName = "AbilityCards/Ouroboros")]
public class Ouroboros : AbilityCard
{
    private int lifeTime = 0;  // number of turns this passive lasts
    private int decayTime = 0; // increments while active

    private int appliedReduction = 0;

    public override string GetName() => "Ouroboros";
    public override int GetTier() => 2;

    public override string GetDescription()
    {
        return "<color=#ffffff>Resets your </color><color=#ff0000>hand (number cards).</color>";
    }

    public override int GetDecayTime()
    {
        return decayTime;
    }

    public override void IncrementDecayTime() {
        decayTime = decayTime + 1;
    }

    public override int GetLifeTime() {
        return lifeTime;
    }

    public override void Execute(int player)
    {
        GameManager gm = GameObject.FindFirstObjectByType<GameManager>();
        if (gm == null)
        {
            Debug.LogError("[Ouroboros] GameManager not found in scene.");
            return;
        }

        Debug.Log($"[Ouroboros] Ouroboros activated by Player {player}!");
        gm.OuroborosCard(player);
    }

    public override bool AIDrawAbilityCard()
    {
        GameManager gm = GameObject.FindFirstObjectByType<GameManager>();
        if (gm == null) { return false; }

        int handTotal = gm.GetHandTotal(gm.placedNumberCards_Enemy);

        if (handTotal > gm.threshold)
        {
            return true;
        } else
        {
            if (handTotal >= gm.threshold-2 && handTotal != gm.threshold)
            {
                return Random.value <= 0.2 && true || false;
            }
        }

        return false;
    }

    public override void Destroyed(int drawer)
    {
        
    }
}