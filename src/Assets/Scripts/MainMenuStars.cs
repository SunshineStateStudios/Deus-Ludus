using Unity.VisualScripting;
using UnityEngine;

public class MainMenuStars : MonoBehaviour
{
    public float maxY = 2165f;
    public float minY = -989;
    public float speed = 10f;
    public float currentYPos = 0f;
    private bool goingUp = true;
    void Update()
    {
        if (goingUp)
        {
        currentYPos += speed;
        this.gameObject.transform.position = new Vector3(735, currentYPos, 0);
        if(currentYPos > maxY)
        {
            this.gameObject.transform.position = new Vector3(735, maxY, 0);
            //currentYPos = -989;
            goingUp = false;
        }
        } else
        {
        currentYPos -= speed;
        this.gameObject.transform.position = new Vector3(735, currentYPos, 0);
        if(currentYPos < minY)
        {
            this.gameObject.transform.position = new Vector3(735, maxY, 0);
            //currentYPos = -989;
            goingUp = true;
        } 
        }
    }
}
