/*

Generic superclass for an ability card.

Written by plexinator-9000.

*/

using UnityEngine;

[System.Serializable]
public class AbilityCardClass : MonoBehaviour
{
    public string suit;

    public AbilityCardClass(string suit)
    {
        this.suit = suit;
    }

    public void Execute()
    {
        
    }
}