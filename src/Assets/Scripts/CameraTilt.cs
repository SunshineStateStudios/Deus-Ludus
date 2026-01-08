/*

Tilts the camera left/right and up/down with the cursor.

Written by plexinator-9000.

*/

using UnityEngine;

public class CameraTilt : MonoBehaviour
{
    public float sensitivity = 20f;
    public float maxTilt = 10f;

    private Quaternion originalRotation;

    void Start()
    {
        originalRotation = transform.localRotation;
    }

    void Update()
    {
        float mouseX = (Input.mousePosition.x / Screen.width) * 2f - 1f;
        float mouseY = (Input.mousePosition.y / Screen.height) * 2f - 1f;

        float tiltX = -mouseY * maxTilt * sensitivity;
        float tiltY = mouseX * maxTilt * sensitivity;

        Quaternion tiltRotation = Quaternion.Euler(tiltX, tiltY, 0f);
        transform.rotation = originalRotation * tiltRotation;
    }
}