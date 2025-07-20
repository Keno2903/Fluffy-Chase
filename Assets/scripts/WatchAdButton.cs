using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WatchAdButton : MonoBehaviour
{

    private Button myButton;

    // Start is called before the first frame update
    void Start()
    {
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(ShowRewardAd);
    }

    private void Update()
    {
    }

    private void ShowRewardAd()
    {
     //   AdManager.instane.UserChoseToWatchAd();
    }
    
}
