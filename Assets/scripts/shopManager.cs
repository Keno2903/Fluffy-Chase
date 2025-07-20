using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopManager : MonoBehaviour
{

    int index_ = 1;


    int count = 0;

    public static shopManager instance;

    public Transform Rect;

    public bool swipedRight;

    Touch touch;

    shopSwipeButton button;

    float h2;

    float v2;

    bool touchDown;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (var shopPanel in GetComponentsInChildren<ShopPanel>())
        {
            shopPanel.Index = index_;
            index_ += 1;
        }

        if(instance != null)
        {
            Destroy(gameObject);
        }else
        {
            instance = this;
        }

        if (GameManager.instance.MorePotionsButtonClicked)
        {
            swipedRight = true;
        }

        button = FindObjectOfType<shopSwipeButton>();

    }

    private void Update()
    {
        if (Input.touchCount == 0)
            return;

        touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Moved && !touchDown)
        {
            touchDown = true;

            h2 = touch.deltaPosition.x;
            v2 = touch.deltaPosition.y;

            Debug.Log("v2 is: " + v2);
            Debug.Log("h2 is: " + h2);

            if (h2 > 20 && swipedRight)
            {
                swipedRight = false;
                button.SwipeLeft();
            }
            else if(h2 < -40 && v2 < 50 && v2 > -50 && !swipedRight)
            {
                swipedRight = true;
                button.SwipeRight();
            }

            h2 = 0;
            v2 = 0;
        }else if(touch.phase == TouchPhase.Ended)
        {
            touchDown = false;
            h2 = 0;
            v2 = 0;
        }
    }


    private void Start()
    {
        if(GameManager.instance.MoreCoinsButtonClicked)
        {
            FindObjectOfType<shopSwipeButton>().SwipeRight();
            FindObjectOfType<shopSwipeButton>().SwipeRight();
        }else if (GameManager.instance.MorePotionsButtonClicked)
        {
            FindObjectOfType<shopSwipeButton>().SwipeRight();
        }
        UpdateUI();
    }

    public void BuyInvisiblePotion()
    {
        if (CurrentData.instance.sound)
        {
            AudioManager.instance.Play("Button");
        }
        if (CurrentData.instance.coins < 5)
            return;
        CurrentData.instance.inivisiblePotion++;
        CurrentData.instance.coins -= 5;
        AudioManager.instance.Play("Cash");
        CurrentData.instance.safeData();
        Menu.instance.Set();
    }

    public void BuyBoostPotion()
    {
        if (CurrentData.instance.sound)
        {
            AudioManager.instance.Play("Cash");
        }
        if (CurrentData.instance.coins < 2)
            return;
        CurrentData.instance.boostPotion++;
        CurrentData.instance.coins -= 2;
        AudioManager.instance.Play("Button");
        CurrentData.instance.safeData();
        Menu.instance.Set();
    }

    public void UpdateUI()
    {
        StartCoroutine(UpdateShopUI());
    }
    public IEnumerator UpdateShopUI()
    {
        yield return new WaitForEndOfFrame();
        foreach (var shopPanel in GetComponentsInChildren<ShopPanel>())
        {
            if (CurrentData.instance.animalsBought[count] == shopPanel.Index)
            {
                if (CurrentData.instance.language == "English")
                {
                    shopPanel.buttonText.text = "bought";
                }else if(CurrentData.instance.language == "Deutsch")
                {
                    shopPanel.buttonText.text = "gekauft";
                    Debug.Log(shopPanel.buttonText.text);
                }else if(CurrentData.instance.language == "French")
                {
                    shopPanel.buttonText.text = "acheté";
                }
            }else  {
                if (CurrentData.instance.language == "English")
                {
                    shopPanel.buttonText.text = "Buy";
                }
                else if (CurrentData.instance.language == "Deutsch")
                {
                    shopPanel.buttonText.text = "Kaufen";
                }
                else if (CurrentData.instance.language == "French")
                {
                    shopPanel.buttonText.text = "acheter";
                }
            }
            count += 1;
        }
        if (count >= CurrentData.instance.animalsBought.Length - 1)
        {
            count = 0;
        }
    }
}
