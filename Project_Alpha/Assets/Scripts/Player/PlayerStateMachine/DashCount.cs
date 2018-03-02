using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class DashCount : MonoBehaviour
{
    [ReadOnly]
    public PlayerControlsStateMachine plrCntrlStateMcn;
    [ReadOnly]
    public Slider countDownDashBar;
    [ReadOnly]
    public Text textDashBar;

    public int maxNumberOfDash = 4;
    public float dashRechargeTime = 2f;

    [ReadOnly]
    public float actualNumberOfDash;

    private bool inRecharging = false;

    private float timeToUpdateTheSlider = .1f;
    //private bool doTheUpdate = false;

    private void Awake()
    {
        countDownDashBar.value = dashRechargeTime;
        actualNumberOfDash = maxNumberOfDash;
    }
    //color = new Color(color.r, color.g, color.b);
    void Update ()
    {
        textDashBar.text = ((int)actualNumberOfDash).ToString() + "/" + maxNumberOfDash.ToString();

        if (!inRecharging)
        {
            if(actualNumberOfDash < maxNumberOfDash)
            {
                inRecharging = true;
                StartCoroutine(Recharge());
            }
        }

        if(actualNumberOfDash <= 0)
        {
            //player can't dash
            plrCntrlStateMcn.SendMessage("HaveSomeDashLeft", false);
        }
        else
        {
            //player can dash
            plrCntrlStateMcn.SendMessage("HaveSomeDashLeft", true);
        }

        if(inRecharging)
        {
            UpdateSlider();
        }
    }

    private void RemoveOneCharge()
    {
        if(actualNumberOfDash-1 <= 0)
        {
            actualNumberOfDash = 0;
        }
        else
        {
            actualNumberOfDash-=1;
        }
    }
    
    private void UpdateSlider()
    {
        countDownDashBar.value += (1/dashRechargeTime) * Time.deltaTime;
    }

    IEnumerator Recharge()
    {
        countDownDashBar.value = 0;
        yield return new WaitForSeconds(dashRechargeTime);
        inRecharging = false;
        if (actualNumberOfDash+1 >= maxNumberOfDash)
        {
            actualNumberOfDash = maxNumberOfDash;
        }
        else
        {
            actualNumberOfDash+=1;
        }
    }
}
