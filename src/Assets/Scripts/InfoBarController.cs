using UnityEngine;

public class InfoBarCotroller : MonoBehaviour
{
    public RectTransform infoBar;
    public Vector2 offset;
    public Canvas canvas;

    void Update()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.worldCamera,
            out pos
        );

        infoBar.anchoredPosition = pos + offset;
    }
}
