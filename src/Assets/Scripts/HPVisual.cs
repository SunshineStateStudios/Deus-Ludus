using UnityEngine;

public class HPVisual : MonoBehaviour
{
    public int id;
    int before;
    int after;
    private Animator animator;

    void Start()
    {
        GameManager manager = GameObject.Find("GameManager").GetComponent<GameManager>();

        animator = GetComponent<Animator>();
        before = manager.ply1.TotalHealth(manager.BlackjackThreshold, false);
    }
    public void flipOrNot()
    {
        if (after >= id){
            if (before < id){
                //flip to show gain
            }
        }
        if (after <= id){
            if (before > id){
                //flip to show loss
            }
        }
    }
}
