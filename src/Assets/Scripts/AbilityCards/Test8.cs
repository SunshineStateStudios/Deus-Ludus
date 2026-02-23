public class Test8 : AbilityCard
{
    public override string name => "Test8";
    public override string description => "this is test8";
    public override int triesDecayTime => 8;
    
    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {
        
    }
}