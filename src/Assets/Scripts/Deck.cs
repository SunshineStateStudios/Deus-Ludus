using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck
{
    private Stack<Card> cards;

    public Deck(IEnumerable<Card> startingCards)
    {
        cards = new Stack<Card>(startingCards.OrderBy(_ => UnityEngine.Random.value));
    }

    public Card Draw()
    {
        return cards.Pop();
    }
}
