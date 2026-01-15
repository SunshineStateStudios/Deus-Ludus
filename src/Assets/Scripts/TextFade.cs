using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class TextFade : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI hoverText;
    [SerializeField] float fadeDuration = 0.2f;

    Tween tween;

    void Awake()
    {
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
        tween = hoverText.DOFade(targetAlpha, fadeDuration);
    }
}