/*

Test ability class. Doesn't do shit.

Written by plexinator-9000.

*/

public class test1 : AbilityCard
{
    private string NiceName = "Test 1";
    private bool isPassive = true;

    public override string GetName()
    {
        return NiceName;
    }
    
    public bool GetPassivity()
    {
        return isPassive;
    }
    
    public override void Execute(int player)
    {

    }
}