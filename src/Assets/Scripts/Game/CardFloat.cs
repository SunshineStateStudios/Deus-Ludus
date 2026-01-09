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

    void Start() {
        amplitude = Random.Range(4000,10000)/1000;
        frequency = Random.Range(3000,4000)/1000;

        deltaTime = Random.Range(0,3600);
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += Time.deltaTime;

        // X & Z coordinates: points of contention.
        gameObject.transform.localRotation = Quaternion.Euler(
            Mathf.Sin(deltaTime * frequency) * amplitude,
            0f,
            Mathf.Cos(deltaTime * frequency) * amplitude
        );
     }
}
