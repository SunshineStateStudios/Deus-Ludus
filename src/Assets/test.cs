using UnityEngine;
using UnityEngine.InputSystem;

public class MouseTest : MonoBehaviour
{
    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Mouse click detected");
        }
    }
}
