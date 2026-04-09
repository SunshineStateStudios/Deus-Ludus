using UnityEngine;
using UnityEngine.UI;

public class NumberCardPromptScript : MonoBehaviour
{
    public int cardIndex;
    public PromptAbilityCard card;
    public GameManager gameManager;

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(CallbackPrompt);
    }

    void CallbackPrompt() {
        gameManager.AnswerNumberCardPrompt(1, cardIndex, card);
    }
}
