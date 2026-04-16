/*

Suit numbers to actual suits:
1: mayan
2: japanese
3: egyptian
4: greek

*/

public class NumberCard : Card
{
    public int Value;
    public int Suit;
    public int Damage;
    public int Health;

    public NumberCard(int val, int suit)
    {
        Value = val;
        //Suit = suit;
        Suit = 0;

        switch(Value)
        {
            case 1:
                Damage = 1;
                Health = 4;
                break;
            case 2:
                Damage = 1;
                Health = 4;
                break;
            case 3:
                Damage = 1;
                Health = 4;
                break;
            case 4:
                Damage = 2;
                Health = 3;
                break;
            case 5:
                Damage = 2;
                Health = 3;
                break;
            case 6:
                Damage = 2;
                Health = 3;
                break;
            case 7:
                Damage = 3;
                Health = 2;
                break;
            case 8:
                Damage = 3;
                Health = 2;
                break;
            case 9:
                Damage = 3;
                Health = 2;
                break;
            case 10:
                Damage = 4;
                Health = 1;
                break;
            case 11:
                Damage = 4;
                Health = 1;
                break;
            default:
                Damage = 4;
                Health = 1;
                break;
        }
    }
}
