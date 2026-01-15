public abstract class AbilityCard : Card
{
    public abstract void Apply(Player owner, Player opponent);
    public abstract void Remove(Player owner, Player opponent);
}
