/*

Generic superclass for an ability card.

Written by plexinator-9000.

*/

using System;
using UnityEngine;

public abstract class AbilityCard : ScriptableObject, IComparable<AbilityCard>
{
    public bool drawn; // shared field across all abilities
    private int tier; // determines card rarity

    public abstract int GetTier();
    public abstract string GetName();
    public abstract string GetDescription();
    public abstract int GetDecayTime();
    public abstract void IncrementDecayTime();
    public abstract int GetLifeTime();
    public abstract void Execute(int player);
    public abstract void Destroyed(int drawer);

    public int CompareTo(AbilityCard other) {
        if (other == null) return 1;
        return this.GetTier().CompareTo(other.GetTier());
    }
}