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
        Debug.Log("it workin 1");
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(.1f);
        manager = gameManagerObject.GetComponent<GameManager>();
        before = manager.ply1.TotalHealth(manager.BlackjackThreshold, false);
        after = before;
        Debug.Log("it workin 2");
    }

    public IEnumerator flipOrNot()
    {
        after = manager.ply1.TotalHealth(manager.BlackjackThreshold, false);
        animator.enabled = true;
        if (after >= id){
            if (before < id){           //flip to show gain
                animator.SetBool("playerHP", true);
                yield return new WaitForSeconds(.5f);
                animator.SetBool("YouSide", true);
                before = after;
            }
        }
        if (after <= id){
            if (before > id){           //flip to show loss
                animator.SetFloat("AnimSpeed", -1.0f);
                animator.SetBool("playerHP", true);
                yield return new WaitForSeconds(.5f);
                animator.SetBool("OppSide", true);
                animator.SetFloat("AnimSpeed", 1.0f);
                before = after;
            }
        }
        animator.enabled = false;
    }
}
