using UnityEngine;
using System.Collections.Generic;

public class Deck
{
    private List<NumberCard> cards = new List<NumberCard>();

    public Deck()
    {
        RecreateDeck();
    }

    void RecreateDeck()
    {
        cards = new List<NumberCard>();
        for (int i = 1; i <= 12; i++)
        {
            cards.Add(new NumberCard(i, Random.Range(1,4)));
        }
    }

    public NumberCard Draw()
    {
        if (cards.Count == 0) RecreateDeck();

        NumberCard chosenCard = cards[0];
        cards.RemoveAt(0);

        return chosenCard;
    }
}
