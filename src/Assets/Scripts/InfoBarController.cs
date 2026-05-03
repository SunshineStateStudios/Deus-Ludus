using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using Unity.VisualScripting;

public class InfoBarController : MonoBehaviour
{
    public GameObject yourTotalInfo;
    public GameObject yourStatsInfo;
    public GameObject oppTotalInfo;
    public GameObject oppStatsInfo;
    public GameObject drawInfo;
    public GameObject stayInfo;
    public GameObject healthInfo;
    public GameObject roundWaveInfo;

    public GameObject drawButton;

    public static bool infoBarsEnabled = true;
    private Coroutine Coroutines;
////////////////////////////////////////////

    void Update()
    {
        if (!drawButton.activeInHierarchy)
        {
            drawInfo.gameObject.SetActive(false);
        }
    }

////////////////////////////////////////////
    public void yourTotalShow()
    {
        Coroutines = StartCoroutine(yourTotalShowDelay());
    }
    public IEnumerator yourTotalShowDelay()
    {
        if(infoBarsEnabled)
        {
        yield return new WaitForSeconds(0.5f);
        yourTotalInfo.SetActive(true);
        }
        yield return new WaitForSeconds(0);
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
        if(infoBarsEnabled)
        {
        yield return new WaitForSeconds(0.5f);
        yourStatsInfo.SetActive(true);
        }
        yield return new WaitForSeconds(0);
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
        if(infoBarsEnabled)
        {
        yield return new WaitForSeconds(0.5f);
        oppTotalInfo.SetActive(true);
        }
        yield return new WaitForSeconds(0);
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
        if(infoBarsEnabled)
        {
        yield return new WaitForSeconds(0.5f);
        oppStatsInfo.SetActive(true);
        }
        yield return new WaitForSeconds(0);
    }
    public void oppStatsHide()
    {
        StopCoroutine(Coroutines);
        oppStatsInfo.SetActive(false);
    }

    /////////////////////////////////////////////////
    
    public void drawButtonShow()
    {
        Coroutines = StartCoroutine(drawButtonShowDelay());
    }
    public IEnumerator drawButtonShowDelay()
    {
        if(infoBarsEnabled)
        {
        yield return new WaitForSeconds(0.5f);
        drawInfo.SetActive(true);
        }
        yield return new WaitForSeconds(0);
    }
    public void drawButtonHide()
    {
        StopCoroutine(Coroutines);
        drawInfo.SetActive(false);
    }
    
    /////////////////////////////////////////////////
    
    public void stayButtonShow()
    {
        Coroutines = StartCoroutine(stayButtonShowDelay());
    }
    public IEnumerator stayButtonShowDelay()
    {
        if(infoBarsEnabled)
        {
        yield return new WaitForSeconds(0.5f);
        stayInfo.SetActive(true);
        }
        yield return new WaitForSeconds(0);
    }
    public void stayButtonHide()
    {
        StopCoroutine(Coroutines);
        stayInfo.SetActive(false);
    }

    ////////////////////////////////////////////////

    public void healthShow()
    {
        Coroutines = StartCoroutine(healthShowDelay());
    }
    public IEnumerator healthShowDelay()
    {
        if(infoBarsEnabled)
        {
        yield return new WaitForSeconds(0.5f);
        healthInfo.SetActive(true);
        }
        yield return new WaitForSeconds(0);
    }
    public void healthHide()
    {
        StopCoroutine(Coroutines);
        healthInfo.SetActive(false);
    }

    ////////////////////////////////////////////////
    
    public void roundWaveShow()
    {
        Coroutines = StartCoroutine(roundWaveShowDelay());
    }
    public IEnumerator roundWaveShowDelay()
    {
        if(infoBarsEnabled)
        {
        yield return new WaitForSeconds(0.5f);
        roundWaveInfo.SetActive(true);
        }
        yield return new WaitForSeconds(0);
    }
    public void roundWaveHide()
    {
        StopCoroutine(Coroutines);
        roundWaveInfo.SetActive(false);
    }
}
