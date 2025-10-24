/*

Test ability class. Doesn't do shit.

Written by plexinator-9000.

*/

[System.Serializable]
public class test2 : AbilityCard
{
    private string NiceName = "Test 2";

    public override string GetName()
    {
        return NiceName;
    }

    public override void Execute()
    {
        
    }
}