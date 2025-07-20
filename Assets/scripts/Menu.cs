using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    public Text coinTextInMenu;

    public Text BoostPotionTextMenu;

    public Text InvisiblePotiontextMenu;

    public static Menu instance;


    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Set();
    }

    public void Set()
    {
        coinTextInMenu.text = CurrentData.instance.coins.ToString();
        BoostPotionTextMenu.text = CurrentData.instance.boostPotion.ToString();
        InvisiblePotiontextMenu.text = CurrentData.instance.inivisiblePotion.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
