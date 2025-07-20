using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_IPHONE
using Unity.Notifications.iOS;
#endif


public class Notification : MonoBehaviour
{

    string HeaderForNotification1;
    string HeaderForNotification2;

    string BodyForNotification1;
    string BodyForNotification2;


    public static Notification instance;

    void Awake()
    {
        instance = this;

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        CheckLangauge();
    }

    public void CheckLangauge()
    {

        switch (CurrentData.instance.language)
        {
            case "English":
                HeaderForNotification1 = "Come back!";
                HeaderForNotification2 = "Daily Reward Available!";
                BodyForNotification1 = "The carrots are not going to find themselves!";
                BodyForNotification2 = "Go get your free reward!";
                break;
            case "Deutsch":
                HeaderForNotification1 = "Komm zurück!";
                HeaderForNotification2 = "Tägliche Belohnung verfügbar!";
                BodyForNotification1 = "Die Karroten finden sich nicht von allein!";
                BodyForNotification2 = "Hol dir deine Belohnung!";
                break;
            case "French":
                HeaderForNotification1 = "Reviens!";
                HeaderForNotification2 = "Récompense quotidienne disponible!";
                BodyForNotification1 = "Les carottes ne vont pas se retrouver!";
                BodyForNotification2 = "Allez chercher votre récompense gratuite!";
                break;
        }
    }

    IEnumerator RequestAuthorization()
    {
        var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge;
        using (var req = new AuthorizationRequest(authorizationOption, true))
        {
            while (!req.IsFinished)
            {
                yield return null;
            };

            string res = "\n RequestAuthorization:";
            res += "\n finished: " + req.IsFinished;
            res += "\n granted :  " + req.Granted;
            res += "\n error:  " + req.Error;
            res += "\n deviceToken:  " + req.DeviceToken;
            Debug.Log(res);
        }
    }

    public void sendNotification()
    {
        var timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = new TimeSpan(5, 0, 0),
            Repeats = false
        };

        var notification = new iOSNotification()
        {
            // You can specify a custom identifier which can be used to manage the notification later.
            // If you don't provide one, a unique string will be generated automatically.
            Identifier = "_notification_01",
            Title = HeaderForNotification1,
            Body = BodyForNotification1,
            Subtitle = "",
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = timeTrigger,
        };

        iOSNotificationCenter.ScheduleNotification(notification);
    }

    public void sendNotificationForReward()
    {
        var timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = new TimeSpan(24, 0, 0),
            Repeats = false
        };

        var notification = new iOSNotification()
        {
            // You can specify a custom identifier which can be used to manage the notification later.
            // If you don't provide one, a unique string will be generated automatically.
            Identifier = "_notification_01",
            Title = HeaderForNotification2,
            Body = BodyForNotification2,
            Subtitle = "",
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = timeTrigger,
        };

        iOSNotificationCenter.ScheduleNotification(notification);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            sendNotification();
            sendNotificationForReward();
        }    
    }
}
