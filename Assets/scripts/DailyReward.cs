using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class DailyReward : MonoBehaviour
{
    public static DailyReward instance;

    public bool hasReward;

    public GameObject DailyRewardPanel;

    private void Awake()
    {
            instance = this;   
    }

    private void Start()
    {
        DailyRewardPanel.gameObject.SetActive(false);
        StartCoroutine(Setup());
    }
    public void getReward()
    {
        CurrentData.instance.lastDailyReward = DateTime.Now;
        Notification.instance.sendNotificationForReward();
        CurrentData.instance.safeData();
    }


    public IEnumerator Setup()
    {
        yield return new WaitForSecondsRealtime(1);
        if (GameManager.instance.diff.TotalDays >= 1 && !GameManager.instance.tutorial && !GameManager.instance.hadRewardToday)
        {
            // Next Reward available
            // enable button
            DailyRewardPanel.gameObject.SetActive(true);
            hasReward = true;
            GameManager.instance.hadRewardToday = true;

            foreach(Button b in FindObjectsOfType<Button>())
            {
                if(b.GetComponent<rewardChest>() == null)
                b.enabled = false;
            }
        }

        else
        {
            DailyRewardPanel.gameObject.SetActive(false);
            hasReward = false;

        }
    }
}
