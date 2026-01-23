using UnityEngine;
using DG.Tweening;

public class CanvasManager : MonoBehaviour
{
    public GameObject drawNumberCardButtonObj;
    public GameObject stayButtonObj;
    public GameObject inventoryButtonObj;
    public GameObject inventoryPanelObj;
    public GameObject gameManager;

    private RectTransform drawNumberCardButton;
    private RectTransform stayButton;
    private RectTransform inventoryButton;
    private RectTransform inventoryPanel;

    private bool panelHidden = true;
    private Tween[] tweens = new Tween[4];
    private GameManager gameManagerScript;

    void Start()
    {
        drawNumberCardButton = drawNumberCardButtonObj.GetComponent<RectTransform>();
        stayButton = stayButtonObj.GetComponent<RectTransform>();
        inventoryButton = inventoryButtonObj.GetComponent<RectTransform>();
        inventoryPanel = inventoryPanelObj.GetComponent<RectTransform>();

        gameManagerScript = gameManager.GetComponent<GameManager>();
    }

    public void CallbackStay()
    {
        if (!panelHidden) return;
    }

    public void CallbackDrawNumberCard()
    {
        if (!panelHidden) return;
        gameManagerScript.DrawNumberCard(1);
    }

    public void ResolvePanel()
    {
        panelHidden = !panelHidden;
        Debug.Log("AAAHHHHH");

        foreach (Tween tween in tweens)
        {
            tween.Kill();
        }

        if (!panelHidden)
        {
            tweens[0] = inventoryPanel.DOAnchorPos(new UnityEngine.Vector2(-122f, -275f), 0.5f);
            tweens[1] = inventoryButton.DOAnchorPos(new UnityEngine.Vector2(-309f, 57f), 0.5f);
            tweens[2] = stayButton.DOAnchorPos(new UnityEngine.Vector2(-193f,158), 0.5f);
            tweens[3] = drawNumberCardButton.DOAnchorPos(new UnityEngine.Vector2(-193f,57f), 0.5f);
        } else
        {
            tweens[0] = inventoryPanel.DOAnchorPos(new UnityEngine.Vector2(129.271f, -275f), 0.5f);
            tweens[1] = inventoryButton.DOAnchorPos(new UnityEngine.Vector2(-60f, 57f), 0.5f);
            tweens[2] = stayButton.DOAnchorPos(new UnityEngine.Vector2(52f,158f), 0.5f);
            tweens[3] = drawNumberCardButton.DOAnchorPos(new UnityEngine.Vector2(52f,57f), 0.5f);
        }
    }
}
