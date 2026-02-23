using System.Collections.Generic;
using System.Linq;

public class Player
{
    public List<NumberCard> NumberCards = new();
    public List<AbilityCard> AbilityCards = new();

    public int Life = 20;

    public int BlackjackTotal(bool ignoreFirstCard = true)
    {
        int total = 0;
        int startingIndex = 0;

        if (ignoreFirstCard) startingIndex = 1;

        for (int i = startingIndex; i < NumberCards.Count; i++)
        {
            total += NumberCards[i].Value;
        }

        for (int i = startingIndex; i < NumberCards.Count; i++)
        {
            if (total <= 21) continue;
            NumberCard card = NumberCards[i];
            if (card.Value != 12) continue;
            total -= 10;
        }

        return total;
    }
    public int TotalDamage => NumberCards.Sum(c => c.Damage);
    public int TotalHealth => NumberCards.Sum(c => c.Health);
    public bool IsBust(int threshold) {
        return BlackjackTotal(false) < threshold;
    }
}
