using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class getMorePotions : MonoBehaviour
{

    public Button myButton;

    // Start is called before the first frame update
    void Start()
    {
        myButton = GetComponent<Button>();

        myButton.onClick.AddListener(getMorePotionsNow);
    }


    public void getMorePotionsNow()
    {
        GameManager.instance.openSceneSlowly("Shop");
        GameManager.instance.MorePotionsButtonClicked = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}