/*

Generic superclass for an ability card.

Written by plexinator-9000.

*/

using UnityEngine;

[System.Serializable]
public class AbilityCard : MonoBehaviour
{
    public string suit;

    public AbilityCard(string suit)
    {
        this.suit = suit;
    }

    public void Execute()
    {
        
    }
}