using Unity.VisualScripting;
using UnityEngine;

public class MainMenuStars : MonoBehaviour
{
    public float maxY = 3154f;
    public float minY = 0;
    public float speed = 10f;
    public float currentYPos = 0f;
    private bool goingUp = true;
    void Update()
    {
        if (goingUp)
        {
        currentYPos += speed;
        transform.position = new Vector3(735, currentYPos, 0);
        if(currentYPos > maxY)
        {
            transform.position = new Vector3(735, maxY, 0);
            //currentYPos = -989;
            goingUp = false;
        }
        } else
        {
        currentYPos -= speed;
        transform.position = new Vector3(735, currentYPos, 0);
        if(currentYPos < minY)
        {
            transform.position = new Vector3(735, maxY, 0);
            //currentYPos = -989;
            goingUp = true;
        } 
        }
    }
}
