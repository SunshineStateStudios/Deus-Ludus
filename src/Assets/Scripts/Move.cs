/*

Script for slowly moving the stars in the main menu up & down.
Written by plexinator-9000.

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float downRange = -340.0f;
    public float upRange = -206.0f;

    private bool movingDownward = true;

    void Update()
    {
        if (movingDownward) {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);

            if (transform.position.y <= downRange) {
                movingDownward = false;
            }
        } else {
            transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);

            if (transform.position.y >= upRange) {
                movingDownward = true;
            }
        }
    }
}