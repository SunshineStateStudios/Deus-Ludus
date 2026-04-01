using UnityEngine;
using UnityEngine.UI;

public class AbilityCardPromptScript : MonoBehaviour
{
    public int cardIndex;
    public int plyNumber;
    public PromptAbilityCardAlt card;
    private Button button;
    
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(CallbackPrompt);
    }

    void CallbackPrompt()
    {
        GameManager manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        manager.AnswerAbilityCardPrompt(plyNumber, cardIndex, card);
    }
}
