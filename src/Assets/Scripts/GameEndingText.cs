using UnityEngine;
using System.Collections;
using TMPro; // Include TMP namespace

public class GameEndingText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float timePerChar = 0.05f;

    public void runRevealText()
    {
        StartCoroutine(RevealText());
    }
    public IEnumerator RevealText()
    {
        text.maxVisibleCharacters = 0;
        text.ForceMeshUpdate();

        int totalChars = text.textInfo.characterCount;
        
        for (int i = 0; i <= totalChars; i++)
        {
            text.maxVisibleCharacters = i;
            yield return new WaitForSeconds(timePerChar);
        }
    }
}
