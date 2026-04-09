using UnityEngine;
using UnityEngine.UI;

public class AbilityCardPromptScript : MonoBehaviour
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

    void CallbackPrompt()
    {
        gameManager.AnswerAbilityCardPrompt(gameManager.ply1, cardIndex, card);
    }
}
