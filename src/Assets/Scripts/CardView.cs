using UnityEngine;

public class CardView : MonoBehaviour
{
    public Card CardData;

    public void Initialise(Card card)
    {
        CardData = card;
        // Update UI
    }
}
