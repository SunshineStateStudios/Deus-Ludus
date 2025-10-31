/*

Generic superclass for an ability card.

Written by plexinator-9000.

*/

using UnityEngine;

[System.Serializable]
public class AbilityCard : MonoBehaviour
{
    private string NiceName = "";

    public virtual string GetName()
    {
        return NiceName;
    }

    public virtual void Execute(int player)
    {
        
    }
}