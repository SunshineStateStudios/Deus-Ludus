using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WhiteFlash : MonoBehaviour
{
    private Image img;
    private Tween flashTween;

    void Awake()
    {
        img = GetComponent<Image>();
    }

    public void Flash()
    {
        Flash(new Color(1f,1f,1f,1f), new Color(0f,0f,0f,0f));
    }

    public void Flash(Color colour)
    {
        Flash(colour, new Color(0f,0f,0f,0f));
    }

    public void Flash(
        Color colour,
        Color restColour
    )
    {
        flashTween?.Kill();

        img.color = colour;
        flashTween = img.DOColor(restColour, 1.5f);
    }
}
