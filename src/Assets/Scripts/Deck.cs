using UnityEngine;

public class Deck
{
    public NumberCard Draw()
    {
        return new NumberCard(Random.Range(1,12),Random.Range(1,4));
    }
}
