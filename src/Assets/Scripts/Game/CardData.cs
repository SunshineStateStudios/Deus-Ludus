/*

Generic class for a number card.

Written by plexinator-9000.

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardData : MonoBehaviour
{
    public int value;
    public string suit;
    public bool isFromEnemy;

    public CardData(int value, string suit)
    {
        this.value = value;
        this.suit = suit;
    }

    void Start() {
	Animator animator = GetComponent<Animator>();

	if (isFromEnemy) {
		animator.Play("cardtossv3-enemy", 0, 0f);
	} else {
		animator.Play("ANIM-Card-Toss-v03 0", 0, 0f);
	}
    }
}
