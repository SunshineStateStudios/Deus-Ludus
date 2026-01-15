using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Numerics;

public class CanvasManager : MonoBehaviour
{
    public GameObject drawNumberCardButtonObj;
    public GameObject stayButtonObj;
    public GameObject inventoryButtonObj;
    public GameObject inventoryPanelObj;

    private RectTransform drawNumberCardButton;
    private RectTransform stayButton;
    private RectTransform inventoryButton;
    private RectTransform inventoryPanel;


    private bool panelHidden = true;
    private Tween[] tweens = new Tween[4];
    void Start()
    {
        drawNumberCardButton = drawNumberCardButtonObj.GetComponent<RectTransform>();
        stayButton = stayButtonObj.GetComponent<RectTransform>();
        inventoryButton = inventoryButtonObj.GetComponent<RectTransform>();
        inventoryPanel = inventoryPanelObj.GetComponent<RectTransform>();
    }

    public void CallbackStay()
    {
        if (!panelHidden) return;
    }

    public void CallbackDrawNumberCard()
    {
        if (!panelHidden) return;
    }

    public void ResolvePanel()
    {
        panelHidden = !panelHidden;

        foreach (Tween tween in tweens)
        {
            tween.Kill();
        }

        if (!panelHidden)
        {
            tweens[0] = inventoryPanel.DOAnchorPos(new UnityEngine.Vector2(-120f, 0f), 0.5f);
            tweens[1] = inventoryButton.DOAnchorPos(new UnityEngine.Vector2(-297f, 63f), 0.5f);
            tweens[2] = stayButton.DOAnchorPos(new UnityEngine.Vector2(-242f,156), 0.5f);
            tweens[3] = drawNumberCardButton.DOAnchorPos(new UnityEngine.Vector2(-242f,63), 0.5f);
        } else
        {
            tweens[0] = inventoryPanel.DOAnchorPos(new UnityEngine.Vector2(133.2f, 0f), 0.5f);
            tweens[1] = inventoryButton.DOAnchorPos(new UnityEngine.Vector2(-63.4f, 63f), 0.5f);
            tweens[2] = stayButton.DOAnchorPos(new UnityEngine.Vector2(66,156), 0.5f);
            tweens[3] = drawNumberCardButton.DOAnchorPos(new UnityEngine.Vector2(66,63), 0.5f);
        }
    }
}
