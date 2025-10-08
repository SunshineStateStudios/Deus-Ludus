/*

Generic class for a number card.

Written by plexinator-9000.

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardData : MonoBehaviour
{
    public int value;
    public string suit;

    public CardData(int value, string suit)
    {
        this.value = value;
        this.suit = suit;
    }
}