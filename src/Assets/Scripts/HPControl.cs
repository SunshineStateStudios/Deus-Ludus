using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPControl
{
    private List<GameObject> HP;

    public void InstantiateMothafucka()
    {
        HP = new List<GameObject>();

        for (int i = 1; i <= 20; i++)
        {
            HP.Add(GameObject.Find("HP" + i.ToString()));
            Debug.Log("andrew, wake, up");
        }
    }

    public IEnumerator HPanimation(bool didyouWin)
    {
        Debug.Log("YAAAAAAAAAAAAAYYYYYYY");
        if (didyouWin)
        {
            Debug.Log("won");
            for (int i = 0; i < 20; i++) //play animation for winning
            {
                GameObject HP_Card = HP[i];
                HPVisual HP_VisualScript = HP_Card.GetComponent<HPVisual>();
                HP_VisualScript.flipOrNot();
                yield return new WaitForSeconds(.2f); 
            }
        } else {
            Debug.Log("lost");
            for (int i = HP.Count - 1; i == 0; i--) //play animation for losingS
            {
                GameObject HP_Card = HP[i];
                HPVisual HP_VisualScript = HP_Card.GetComponent<HPVisual>();
                HP_VisualScript.flipOrNot();
                yield return new WaitForSeconds(.2f); 
            }
        }
        
    }

}
