using System.Collections;
using UnityEngine;

public class HPVisual : MonoBehaviour
{
    public int id;
    private int before;
    private int after;
    public GameObject gameManagerObject;
    private Animator animator;
    private GameManager manager;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(.1f);
        manager = gameManagerObject.GetComponent<GameManager>();
        before = manager.ply1.TotalHealth(manager.BlackjackThreshold, false);
        after = before;
    }

    public IEnumerator flipOrNot()
    {
        if (after >= id){
            if (before < id){
                //flip to show gain
                animator.SetBool("playerHP", true);
                yield return new WaitForSeconds(.5f);
                animator.SetBool("YouSide", true);
            }
        }
        if (after <= id){
            if (before > id){
                //flip to show loss
                yield return new WaitForSeconds(.5f);
            }
        }
    }
}
