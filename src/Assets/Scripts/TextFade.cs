using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class TextFade : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI hoverText;
    [SerializeField] GameObject image;
    [SerializeField] float fadeDuration = 0.2f;

    Tween tween;
    Tween imageTween;
    RawImage rawImage;

    void Awake()
    {
        rawImage = image.GetComponent<RawImage>();
        hoverText.alpha = 0f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Fade(1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Fade(0f);
    }

    void Fade(float targetAlpha)
    {
        tween?.Kill();
        imageTween?.Kill();

        if (targetAlpha == 0f)
        {
            imageTween = rawImage.DOColor(Color.white, fadeDuration);
        } else
        {
            imageTween = rawImage.DOColor(new Color(66f,255f,66f), fadeDuration);
        }
        tween = hoverText.DOFade(targetAlpha, fadeDuration);
    }
}