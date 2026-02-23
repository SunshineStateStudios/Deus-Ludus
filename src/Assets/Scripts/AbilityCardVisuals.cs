using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCardVisuals : MonoBehaviour
{
    public GameObject canvas;
    private WhiteFlash whiteOverlayScript;

    void Start()
    {
        whiteOverlayScript = GameObject.Find("Canvas/WhiteOverlay").GetComponent<WhiteFlash>();
        whiteOverlayScript.Flash(new Color(1f,1f,1f,0.5f));
    }

    void OnMouseEnter() {
        canvas.SetActive(true);
    }

    void OnMouseExit() {
        canvas.SetActive(false);
    }
}
