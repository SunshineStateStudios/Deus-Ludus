using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public GameObject inventoryObj;
    public GameObject gameManager;

    private bool panelHidden = true;
    private GameManager gameManagerScript;
    private Animator inventoryAnimator;

    void Start()
    {
        gameManagerScript = gameManager.GetComponent<GameManager>();
        inventoryAnimator = inventoryObj.GetComponent<Animator>();
    }

    public void CallbackStay()
    {
        if (!panelHidden) return;
        StartCoroutine(gameManagerScript.EndRound(true));
    }

    public void CallbackDrawNumberCard()
    {
        if (!panelHidden) return;
        StartCoroutine(gameManagerScript.EndRound(false));
    }

    public void ResolvePanel()
    {
        panelHidden = !panelHidden;
        string animToPlay = "In";
        if (!panelHidden) animToPlay = "Out";

        inventoryAnimator.Play(animToPlay, 0, 0);
    }
}
