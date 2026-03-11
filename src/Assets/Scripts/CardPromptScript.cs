using UnityEngine;
using UnityEngine.UI;

public class CardPromptScript : MonoBehaviour
{
    public int cardIndex;
    public int plyNumber;
    public PromptAbilityCard card;
    private Button button;
    
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(CallbackPrompt);
    }

    void CallbackPrompt()
    {
        GameManager manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        manager.AnswerNumberCardPrompt(plyNumber, cardIndex, card);
    }
}
