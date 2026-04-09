using UnityEngine;
using UnityEngine.UI;

public class AbilityCardUIScript : MonoBehaviour
{
    public int index;
    public GameManager gameManager;
    public string desc;

    private GameObject nameText;
    private GameObject contentsText;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(CallbackPrompt);
    }

    void CallbackPrompt() {
        gameManager.DrawAbilityCard(1, index);
    }
}
