using UnityEngine;

public class AbilityCardUIScript : MonoBehaviour
{
    public int index;
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void Draw()
    {
        gameManager.DrawAbilityCard(1, index);
    }
}
