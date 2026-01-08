/*

"Polaris" ability card. Passive ability card that adds threshold by 3.

Written by plexinator-9000
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Polaris", menuName = "AbilityCards/Polaris")]
public class Polaris : AbilityCard
{
    private int lifeTime = 1;  // number of turns this passive lasts
    private int decayTime = 0; // increments while active

    private int appliedReduction = 0;

    public override string GetName() => "Polaris";
    public override int GetTier() => 2;

    public override string GetDescription()
    {
        return "<color=#ffffff>Adds </color><color=#ff0000>3 </color><color=#ffffff>to the current <color=#ff0000>threshold.</color> Lasts one round.";
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
            Debug.LogError("[Polaris] GameManager not found in scene.");
            return;
        }

        Debug.Log($"[Polaris] Polaris activated by Player {player}!");

        int originalThreshold = gm.threshold;
        gm.threshold += 3;
        if (gm.threshold > 30) gm.threshold = 30;

        appliedReduction = gm.threshold - originalThreshold;
        gm.UpdateHandValueText();
    }

    public override void Destroyed(int drawer)
    {
        GameManager gm = GameObject.FindFirstObjectByType<GameManager>();
        gm.threshold -= appliedReduction;
        gm.UpdateHandValueText();
    }

    public override bool AIDrawAbilityCard()
    {
        GameManager gm = GameObject.FindFirstObjectByType<GameManager>();
        int enemyHandTotal = gm.GetHandTotal(gm.placedNumberCards_Enemy);

        if (enemyHandTotal == gm.threshold) { return false; }
        if (gm.threshold - 3 > enemyHandTotal || enemyHandTotal > gm.threshold)
        {
            return true;
        }

        return false;
    }
}