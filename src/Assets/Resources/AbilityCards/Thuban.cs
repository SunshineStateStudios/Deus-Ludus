/*

"Thuban" ability card. Passive ability card that lowers threshold by 4.

Written by plexinator-9000
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Thuban", menuName = "AbilityCards/Thuban")]
public class Thuban : AbilityCard
{
    [SerializeField] private string cardName = "Thuban";
    [SerializeField] private int lifeTime = 1;  // number of turns this passive lasts
    [SerializeField] private int decayTime = 0; // increments while active
    [SerializeField] private int tier = 2;

    private int appliedReduction = 0;

    public override string GetName()
    {
        return cardName;
    }

    public override int GetTier() {
        return tier;
    }

    public override string GetDescription()
    {
        return "<color=#ffffff>Subtracts the current </color><color=#ff0000>threshold </color>by 4. Lasts one round.";
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
        GameManager gm = GameObject.FindObjectOfType<GameManager>();
        if (gm == null)
        {
            Debug.LogError("[Thuban] GameManager not found in scene.");
            return;
        }

        Debug.Log($"[Thuban] {cardName} activated by Player {player}!");

        int originalThreshold = gm.threshold;
        gm.threshold -= 4;
        if (gm.threshold < 7) gm.threshold = 7;

        appliedReduction = originalThreshold - gm.threshold;
        gm.UpdateHandValueText();
    }

    public override void Destroyed(int drawer)
    {
        GameManager gm = GameObject.FindObjectOfType<GameManager>();
        gm.threshold += appliedReduction;
        gm.UpdateHandValueText();
    }
}