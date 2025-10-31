/*

Test ability class. Doesn't do shit.

Written by plexinator-9000.

*/

public class test2 : AbilityCard
{
    private string NiceName = "Test 2";
    private bool isPassive = false;

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