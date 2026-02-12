public class NumberCard : Card
{
    public int Value;
    public int Damage;
    public int Health;
    public AbilityCard attachedCard = null;

    public NumberCard(int val)
    {
        Value = val;

        switch(Value)
        {
            case 1:
                Damage = 1;
                Health = 11;
                break;
            case 2:
                Damage = 2;
                Health = 10;
                break;
            case 3:
                Damage = 3;
                Health = 9;
                break;
            case 4:
                Damage = 4;
                Health = 8;
                break;
            case 5:
                Damage = 5;
                Health = 7;
                break;
            case 6:
                Damage = 6;
                Health = 6;
                break;
            case 7:
                Damage = 7;
                Health = 5;
                break;
            case 8:
                Damage = 8;
                Health = 4;
                break;
            case 9:
                Damage = 9;
                Health = 3;
                break;
            case 10:
                Damage = 10;
                Health = 2;
                break;
            case 11:
                Damage = 11;
                Health = 1;
                break;
        }
    }
}
