/*

Test ability class. Doesn't do shit.

Written by plexinator-9000.

*/

[System.Serializable]
public class test1 : AbilityCard
{
    private string NiceName = "Test 1";
    private bool isPassive = false;

    public override string GetName()
    {
        return NiceName;
    }
    public bool GetPassivity()
    {
        return isPassive;
    }

    public override void Execute()
    {
        
    }
}