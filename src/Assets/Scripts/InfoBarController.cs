using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoBarController : MonoBehaviour
{
    public GameObject yourTotalInfo;
    public GameObject yourStatsInfo;
    public GameObject oppTotalInfo;
    public GameObject oppStatsInfo;
    private Coroutine Coroutines;
    public void yourTotalShow()
    {
        Coroutines = StartCoroutine(yourTotalShowDelay());
    }
    public IEnumerator yourTotalShowDelay()
    {
        yield return new WaitForSeconds(0.5f);
        yourTotalInfo.SetActive(true);
    }
    public void yourTotalHide()
    {
        StopCoroutine(Coroutines);
        yourTotalInfo.SetActive(false);
    }

/// //////////////////////////////////////////////////
    public void yourStatsShow()
    {
        Coroutines = StartCoroutine(yourStatsShowDelay());
    }
    public IEnumerator yourStatsShowDelay()
    {
        yield return new WaitForSeconds(0.5f);
        yourStatsInfo.SetActive(true);
    }
    public void yourStatsHide()
    {
        StopCoroutine(Coroutines);
        yourStatsInfo.SetActive(false);
    }

/////////////////////////////////////////////////
    public void oppTotalShow()
    {
        Coroutines = StartCoroutine(oppTotalShowDelay());
    }
    public IEnumerator oppTotalShowDelay()
    {
        yield return new WaitForSeconds(0.5f);
        oppTotalInfo.SetActive(true);
    }
    public void oppTotalHide()
    {
        StopCoroutine(Coroutines);
        oppTotalInfo.SetActive(false);
    }

//////////////////////////////////////////////
    public void oppStatsShow()
    {
        Coroutines = StartCoroutine(oppStatsShowDelay());
    }
    public IEnumerator oppStatsShowDelay()
    {
        yield return new WaitForSeconds(0.5f);
        oppStatsInfo.SetActive(true);
    }
    public void oppStatsHide()
    {
        StopCoroutine(Coroutines);
        oppStatsInfo.SetActive(false);
    }
}
