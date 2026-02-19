public abstract class AbilityCard : Card
{
    public virtual string name { get; }
    public virtual string description { get; }
    public virtual int triesDecayTime { get; }
    
    public abstract void Apply(Player owner, Player opponent);
    public abstract void Remove(Player owner, Player opponent);
    public bool Drawn = false;
    public int triesPassed = 0;
}
