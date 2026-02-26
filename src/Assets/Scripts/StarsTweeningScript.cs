using UnityEngine;
using DG.Tweening;
using System.Collections;

public class StarsTweeningScript : MonoBehaviour
{
    public Vector3 endingPos;

    private Vector3 startingPos;
    private float AmountOfTimeForTween = 60f;
    private bool finished = true;

    IEnumerator Move()
    {
        finished = false;
        transform.DOMove(endingPos, AmountOfTimeForTween).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(AmountOfTimeForTween);
        transform.DOMove(startingPos, AmountOfTimeForTween).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(AmountOfTimeForTween);
        finished = true;
    }

    void Start()
    {
        startingPos = transform.position;
        StartCoroutine(Move());
    }

    void Update()
    {
        if (finished)
        {
            StartCoroutine(Move());
        }
    }
}
