using UnityEngine;

public abstract class AbilityCard : Card
{
    public virtual string name { get; }
    public virtual string description { get; }
    public virtual int triesDecayTime { get; }
    public virtual Sprite icon { get; set; }
    
    public abstract void Apply(GameManager gm, Player owner, Player opponent);
    public abstract void Remove(GameManager gm, Player owner, Player opponent);
    public abstract bool AIShouldDraw(GameManager gm, Player owner);
    public bool Drawn = false;
    public int triesPassed = 0;
}
