using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{

    public int Index;

    public int costForItemOrAnimal;

    public Text buttonText;

    public Text priceText;

    Button buyButton;

    // At which Level can you buy the animal
    public int levelOfAvailabilty;

    public Image lockImage;

    public Text AvailableAtText;

    bool available; 

    // Start is called before the first frame update
    void Start()
    {

        priceText.text = costForItemOrAnimal.ToString();
        buyButton = GetComponentInChildren<Button>();
        buyButton.onClick.AddListener(buy);

        if(CurrentData.instance.currentLevel + 1 >= levelOfAvailabilty)
        {
            // You can buy the Animal
            AvailableAtText.enabled = false;
            lockImage.enabled = false;
            available = true;
        }else
        {
            AvailableAtText.enabled = true;
            if(CurrentData.instance.language == "Deutsch")
            {
                AvailableAtText.text = "Verfügbar ab Level" + " " + levelOfAvailabilty.ToString();
            }
            else if(CurrentData.instance.language == "French")
            {
                AvailableAtText.text = "Disponible au niveau" + " " + levelOfAvailabilty.ToString();
            }
            else if(CurrentData.instance.language == "English")
            {
                AvailableAtText.text = "Available to buy at Level" + " " + levelOfAvailabilty.ToString();
            }
            lockImage.enabled = true;
            available = false;
        }
    }

   
    // Update is called once per frame
    void Update()
    {
       
    }

    

    public void buy()
    {
        if (CurrentData.instance.sound)
        {
            AudioManager.instance.Play("Button");
        }
        if (costForItemOrAnimal <= CurrentData.instance.coins && CurrentData.instance.animalsBought[Index - 1] != Index && available)
        {
            //buy the animal
            if (CurrentData.instance.sound)
            {
                AudioManager.instance.Play("Cash");
            }
            CurrentData.instance.coins -= costForItemOrAnimal;
            CurrentData.instance.animalsBought[Index - 1] = Index;
            shopManager.instance.UpdateUI();
            FindObjectOfType<Menu>().Set();
            CurrentData.instance.safeData();
        }else
        {
            Debug.Log("Not enough money or Animal not available");
        }
    }

}
