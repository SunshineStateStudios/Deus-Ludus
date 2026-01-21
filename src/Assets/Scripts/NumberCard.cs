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
                Health = 0;
                break;
            case 2:
                Damage = 1;
                Health = 1;
                break;
            case 3:
                Damage = 2;
                Health = 1;
                break;
            case 4:
                Damage = 2;
                Health = 2;
                break;
            case 5:
                Damage = 3;
                Health = 2;
                break;
            case 6:
                Damage = 3;
                Health = 3;
                break;
            case 7:
                Damage = 3;
                Health = 4;
                break;
            case 8:
                Damage = 4;
                Health = 5;
                break;
            case 9:
                Damage = 5;
                Health = 3;
                break;
            case 10:
                Damage = 6;
                Health = 2;
                break;
            case 11:
                Damage = 7;
                Health = 1;
                break;
        }
    }
}
