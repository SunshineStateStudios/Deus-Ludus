/*

"Thuban" ability card. Passive ability card that lowers threshold by 4.

Written by plexinator-9000
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Thuban", menuName = "AbilityCards/Thuban")]
public class Thuban : AbilityCard
{
    private int lifeTime = 1;  // number of turns this passive lasts
    private int decayTime = 0; // increments while active

    private int appliedReduction = 0;

    public override string GetName() => "Thuban";
    public override int GetTier() => 2;

    public override string GetDescription()
    {
        return "<color=#ffffff>Subtracts </color><color=#ff0000>4 </color><color=#ffffff>from the current <color=#ff0000>threshold.</color> Lasts one round.";
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
            Debug.LogError("[Thuban] GameManager not found in scene.");
            return;
        }

        Debug.Log($"[Thuban] Thuban activated by Player {player}!");

        int originalThreshold = gm.threshold;
        gm.threshold -= 4;
        if (gm.threshold < 7) gm.threshold = 7;

        appliedReduction = originalThreshold - gm.threshold;
        gm.UpdateHandValueText();
    }

    public override void Destroyed(int drawer)
    {
        GameManager gm = GameObject.FindFirstObjectByType<GameManager>();
        gm.threshold += appliedReduction;
        gm.UpdateHandValueText();
    }

    public override bool AIDrawAbilityCard()
    {
        GameManager gm = GameObject.FindFirstObjectByType<GameManager>();
        int enemyHandTotal = gm.GetHandTotal(gm.placedNumberCards_Enemy);

        if (enemyHandTotal == gm.threshold) { return false; }
        if (gm.threshold + 3 < enemyHandTotal || enemyHandTotal < gm.threshold)
        {
            return true;
        }

        return false;
    }
}