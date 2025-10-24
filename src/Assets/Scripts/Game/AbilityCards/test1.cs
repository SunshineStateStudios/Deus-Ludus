/*

Test ability class. Doesn't do shit.

Written by plexinator-9000.

*/

[System.Serializable]
public class test1 : AbilityCard
{
    private string NiceName = "Test 1";

    public override string GetName()
    {
        return NiceName;
    }

    public override void Execute()
    {
        
    }
}