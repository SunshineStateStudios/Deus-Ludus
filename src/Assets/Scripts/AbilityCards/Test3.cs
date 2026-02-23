public class Test3 : AbilityCard
{
    public override string name => "Test3";
    public override string description => "this is test3";
    public override int triesDecayTime => 3;
    
    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {
        
    }
}