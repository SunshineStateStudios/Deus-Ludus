/*

Gives the object a floating effect.

Written by plexinator-9000.

*/

using UnityEngine;

public class CardFloat : MonoBehaviour
{
    public float amplitude;
    public float frequency;

    private float deltaTime;
    private Vector3 originalPosition;

    void Start() {
        amplitude = Random.Range(4000,10000)/1000;
        frequency = Random.Range(3000,4000)/1000;

        deltaTime = Random.Range(0,3600);
        originalPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += Time.deltaTime;

        // X & Z coordinates: points of contention.
        gameObject.transform.eulerAngles = new Vector3(
            Mathf.Sin(deltaTime * frequency) * amplitude,
            0,
            Mathf.Cos(deltaTime * frequency) * amplitude
        );

        // Make object go up and down
        gameObject.transform.position = originalPosition + new Vector3(0,Mathf.Sin(deltaTime * (frequency/2)) * amplitude/20,0);
    }

    void OnMouseEnter() {
        amplitude += 1;
        frequency += 8;
    }

    void OnMouseExit() {
        amplitude -= 1;
        frequency -= 8;
    }
}
