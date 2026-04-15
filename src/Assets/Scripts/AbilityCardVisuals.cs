using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCardVisuals : MonoBehaviour
{
    private GameObject canvas;

    void Start()
    {
        canvas = transform.Find("Canvas").gameObject;
        canvas.SetActive(false);
    }

    void OnMouseEnter() {
        canvas.SetActive(true);
    }

    void OnMouseExit() {
        canvas.SetActive(false);
    }
}
