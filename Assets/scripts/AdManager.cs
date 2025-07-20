using System;
using UnityEngine;
//using GoogleMobileAds.Api;
//using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdManager : MonoBehaviour
{
    /*
    // Banner Id = ca-app-pub-1367076576699755/6733484023
    // Interstitial Id = ca-app-pub-1367076576699755/2330176911
    // Reward Id = ca-app-pub-1367076576699755/4937827256

    // Test Banner Id = ca-app-pub-3940256099942544/2934735716
    // Test Interstitial Id = ca-app-pub-3940256099942544/4411468910
    // Test Reward Id = ca-app-pub-3940256099942544/1712485313



    private BannerView bannerView;

    private RewardedAd rewardedAd;

    public int coinsForReward = 30;

    public static AdManager instane;

    bool bannerPlaying;

    private InterstitialAd interstitial;

    [SerializeField]
    float timerForReward;

    bool rewardAdSeen;

    [SerializeField]
    public Button buttonForReward;

    [SerializeField]
    float maxTimer = 300;

    private void Awake()
    {
        if(instane != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instane = this;
        }

        DontDestroyOnLoad(this);
    }


    public void Start()
    {
        MobileAds.Initialize(initStatus => { });
        CreateAndLoadRewardedAd();
        SceneManager.sceneLoaded += OnSceneLoaded;
        timerForReward = maxTimer;
    }

    private void Update()
    {
        if(rewardAdSeen)
        {
            timerForReward -= Time.deltaTime;

            if (timerForReward <= 0)
            {
                // Player is allowed to see an Ad again
                rewardAdSeen = false;
                buttonForReward.enabled = true;
                buttonForReward.interactable = true;
                buttonForReward.image.color = new Color(1, 1, 1, 1);
                foreach (Image i in buttonForReward.GetComponentsInChildren<Image>())
                {
                    i.color = new Color(1, 1, 1, 1);
                }
                timerForReward = maxTimer;
            }
        }
    }

    public void CreateAndLoadRewardedAd()
    {

        #if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";
        #elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/1712485313";
        #else
        string adUnitId = "unexpected_platform";
        #endif

        this.rewardedAd = new RewardedAd(adUnitId);

        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }


    public void UserChoseToWatchAd()
    {
        
        if (this.rewardedAd.IsLoaded() && !rewardAdSeen)
        {
            rewardAdSeen = true;
            buttonForReward.image.color = new Color(1, 1, 1, 0.5f);
            foreach(Image i in buttonForReward.GetComponentsInChildren<Image>())
            {
                i.color = new Color(1, 1, 1, 0.5f);
            }
            this.rewardedAd.Show();
            foreach (AudioSource s in AudioManager.instance.GetComponentsInChildren<AudioSource>())
            {
                s.volume = 0;
            }
        }
    }

    public void DestroyBanner()
    {
        bannerView.Destroy();
    }


    #region EventsForRewardAd
    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        this.CreateAndLoadRewardedAd();
        foreach (AudioSource s in AudioManager.instance.GetComponentsInChildren<AudioSource>())
        {
            s.volume = 1;
        }

        AudioManager.instance.Play("Cash");
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }
    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.Message);
    }
    public void HandleUserEarnedReward(object sender, Reward args)
    {
        CurrentData.instance.coins += coinsForReward;
        Menu.instance.Set();
        foreach (AudioSource s in AudioManager.instance.GetComponentsInChildren<AudioSource>())
        {
            s.volume = 1;
        }

        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);

    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);
    }


    #endregion

    public void RequestBanner()
    {

        #if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";
        #elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/2934735716";
        #else
        string adUnitId = "unexpected_platform";
        #endif

        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (GameManager.instance.tutorial)
            return;

        if(scene.name == "ChooseLevel" || scene.name == "Settings" || scene.name == "Showcase Level" || scene.name == "Shop")
        {
            if(!bannerPlaying)
            {
                RequestBanner();
                bannerPlaying = true;
            }
        }else
        {
            if (bannerPlaying)
            {
                bannerView.Destroy();
                bannerPlaying = false;
            }
               
        }

        if(scene.name == "Menu")
        {
            buttonForReward = FindObjectOfType<WatchAdButton>().GetComponent<Button>();

            if (rewardAdSeen)
            {
                foreach (Image i in buttonForReward.GetComponentsInChildren<Image>())
                {
                    i.color = new Color(1, 1, 1, 0.5f);
                }
            }
        }
    }

    // Interstitial

    private void RequestInterstitial()
    {
    #if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
    #elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
    #else
        string adUnitId = "unexpected_platform";
    #endif

        if (interstitial != null)
            interstitial.Destroy();

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitial.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;


        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }

    public void showInterstitial()
    {
        RequestInterstitial();
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        if(interstitial.IsLoaded())
        {
            this.interstitial.Show();
            foreach (AudioSource s in AudioManager.instance.GetComponentsInChildren<AudioSource>())
            {
                s.volume = 0;
            }
        }
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
        foreach (AudioSource s in AudioManager.instance.GetComponentsInChildren<AudioSource>())
        {
            s.volume = 1;
        }
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }
    */
}