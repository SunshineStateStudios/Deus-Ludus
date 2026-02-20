public class Test5 : AbilityCard
{
    public override string name => "Test5";
    public override string description => "this is test5";
    public override int triesDecayTime => 5;
    
    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {
        
    }
}