using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;

public class Player
{
    public List<NumberCard> NumberCards = new();
    public List<AbilityCard> AbilityCards = new();
    public int Life = 10;
    
    public int BlackjackTotal(int currentThreshold, bool ignoreFirstCard = true)
    {
        int total = 0;
        int startingIndex = 0;

        if (ignoreFirstCard) startingIndex = 1;

        for (int i = startingIndex; i < NumberCards.Count; i++)
        {
            if (NumberCards[i].Value == 12)
            {
                total += 11;
                continue;
            }
            total += NumberCards[i].Value;
        }

        if (total > currentThreshold)
        {
            for (int i = startingIndex; i < NumberCards.Count; i++)
            {
                if (NumberCards[i].Value == 12) {
                    total -= 10;
                }
            }
        }
        
        return total;
    }

    public int TotalDamage(bool ignoreFirstCard = true)
    {
        int total = 0;
        int startingIndex = 0;

        if (ignoreFirstCard) startingIndex = 1;

        for (int i = startingIndex; i < NumberCards.Count; i++) {
            total += NumberCards[i].Damage;
        }

        return total;
    }
    
    public int TotalHealth(int currentThreshold, bool ignoreFirstCard = true)
    {
        int total = 0;
        int startingIndex = 0;
        int Blackjacktotal = BlackjackTotal(currentThreshold, false);

        if (ignoreFirstCard) startingIndex = 1;

        for (int i = startingIndex; i < NumberCards.Count; i++) {
            total += NumberCards[i].Health;
        }
        if (Blackjacktotal > currentThreshold)
        {
            total /= 2;
        }

        return total;
    }

    public bool IsBust(int threshold) {
        return BlackjackTotal(threshold, false) < threshold;
    }
}
