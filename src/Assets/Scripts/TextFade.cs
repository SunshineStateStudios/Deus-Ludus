using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TextFade : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string Contents;

    private GameObject infoTextObj;
    private TMP_Text infoTextTxt;
    private Animator infoTextAnimator;

    void Start()
    {
        infoTextObj = GameObject.Find("Canvas/InfoText");
        infoTextTxt = infoTextObj.GetComponent<TMP_Text>();
        infoTextAnimator = infoTextObj.GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        infoTextTxt.text = Contents;
        infoTextAnimator.Play("InfoText_In", 0, 0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoTextAnimator.Play("InfoText_Out", 0, 0);
    }
}