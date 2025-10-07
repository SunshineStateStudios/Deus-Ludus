/*

Controls game loop, using other managers to do so.
Stores player & enemy cards.

Written by plexinator-9000.

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> playerInventory = new List<GameObject>(); // Ability cards that are currently in the hands of the player.
    public List<GameObject> enemyInventory = new List<GameObject>(); // Same as playerInventory, but for the enemy.

    public List<GameObject> placedAbilityCards_Player = new List<GameObject>(); // Currently drawn & active ability cards by the player.
    public List<GameObject> placedAbilityCards_Enemy = new List<GameObject>(); // Same as placedAbilityCards_Player, but for the enemy.

    public List<GameObject> placedNumberCards_Player = new List<GameObject>(); // Currently drawn number cards by the player.
    public List<GameObject> placedNumberCards_Enemy = new List<GameObject>(); // Same as placedNumberCards_Player, but for the enemy,

    // Start is called before the first frame update
    void Start()
    {

    }

    public void DrawNumberCard(int player)
    {

    }

    public void DrawAbilityCard(int player)
    {

    }

    public void EndTurn(int player)
    {

    }
    
}
