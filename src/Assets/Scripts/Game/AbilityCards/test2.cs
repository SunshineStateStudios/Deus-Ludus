/*

Test ability class. Doesn't do shit.

Written by plexinator-9000.

*/

public class test2 : AbilityCard
{
    private string NiceName = "Test 2";
    private int decayTime = 1; // How many rounds until this card expires. 0 = instant

    public override string GetName()
    {
        return NiceName;
    }
    
    public int GetDecayTime()
    {
        return decayTime;
    }
    
    public override void Execute(int player)
    {

    }
}