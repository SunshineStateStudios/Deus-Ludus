using UnityEngine;

public class CardView : MonoBehaviour
{
    public NumberCard CardData;

    public void Initialise(NumberCard card)
    {
        CardData = card;
        // Update UI
    }
}
