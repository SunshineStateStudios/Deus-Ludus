/*

Generic superclass for an ability card.

Written by plexinator-9000.

*/

using UnityEngine;

public abstract class AbilityCard : ScriptableObject
{
    public bool drawn; // shared field across all abilities

    public abstract string GetName();
    public abstract string GetDescription();
    public abstract int GetDecayTime();
    public abstract int GetLifeTime();
    public abstract void Execute(int player);
    public abstract void Destroyed(int drawer);
}