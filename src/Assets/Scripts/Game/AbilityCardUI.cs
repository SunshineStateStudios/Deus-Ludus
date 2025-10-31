/*

Makes it so that the player can execute ability cards during their turn.

Written by plexinator-9000.

*/

using UnityEngine;
using UnityEngine.UI;

public class AbilityCardUI : MonoBehaviour
{
    private Button button_ui;

    public int index = 0; // Refers to the index that corresponds to the correct ability card.

    void Start()
    {
        button_ui = gameObject.GetComponent<Button>();
        button_ui.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        GameObject gameManagerObj = GameObject.Find("GameManager");
        GameManager gameManagerScript = gameManagerObj.GetComponent<GameManager>();
        gameManagerScript.DrawAbilityCard(index, 1);
        Destroy(gameObject);
    }
}
