using System.Collections.Generic;
using System.Linq;

public class Player
{
    public List<NumberCard> NumberCards = new();
    public List<AbilityCard> AbilityCards = new();

    public int Life = 20;

    public int BlackjackTotal => NumberCards.Sum(c => c.Value);
    public int TotalDamage => NumberCards.Sum(c => c.Damage);
    public int TotalHealth => NumberCards.Sum(c => c.Health);
    public bool IsBust => BlackjackTotal > 21;
}
