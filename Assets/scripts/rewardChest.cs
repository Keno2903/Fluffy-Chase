using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rewardChest : MonoBehaviour
{
    public Sprite openChest;

    Animator myAnimator;

    public Sprite coinBag;

    public Sprite boostPotion;

    public Sprite invisiblePotion;

    public Image wonItem;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        wonItem.gameObject.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openTheChest()
    {
        foreach(rewardChest i in FindObjectsOfType<rewardChest>())
        {
            i.GetComponent<Button>().enabled = false;
        }

        myAnimator.Play("Opening" + gameObject.name);
        AudioManager.instance.Play("Button");

        foreach (var chest in FindObjectsOfType<rewardChest>())
        {
            if(chest != this)
            {
                chest.myAnimator.Play("Faint" + chest.gameObject.name);
            }
        }

        StartCoroutine(DisplayItem());
        FindObjectOfType<DailyReward>().getReward();
    }


    IEnumerator DisplayItem()
    {
        yield return new WaitForSecondsRealtime(1);
        int random = Random.Range(0, 3);

        AudioManager.instance.Play("Cash");
        if (random == 0)
        {
            wonItem.sprite = coinBag;
            wonItem.transform.localScale = new Vector3(1.3f, 1, 1);
            int coins = Random.Range(5, 12);
            switch (CurrentData.instance.language)
            {
                case "Deutsch":
                    wonItem.GetComponentInChildren<Text>().text = coins.ToString() + "x Münzen";
                    break;
                case "English":
                    wonItem.GetComponentInChildren<Text>().text = coins.ToString() + "x coins";
                    break;
                case "French":
                    wonItem.GetComponentInChildren<Text>().text = coins.ToString() + "x or";
                    break;
            }
            
            CurrentData.instance.coins += coins;
        }else if(random == 1)
        {
            wonItem.transform.localScale = new Vector3(1f, 1, 1);
            switch (CurrentData.instance.language)
            {
                case "Deutsch":
                    wonItem.GetComponentInChildren<Text>().text = "1x Trank";
                    break;
                case "English":
                    wonItem.GetComponentInChildren<Text>().text = "1x Potion";
                    break;
                case "French":
                    wonItem.GetComponentInChildren<Text>().text = "1x Potion";
                    break;
            }
            wonItem.sprite = boostPotion;
            CurrentData.instance.boostPotion++;
        }else
        {
            wonItem.transform.localScale = new Vector3(1f, 1, 1);
            switch (CurrentData.instance.language)
            {
                case "Deutsch":
                    wonItem.GetComponentInChildren<Text>().text = "1x Trank";
                    break;
                case "English":
                    wonItem.GetComponentInChildren<Text>().text = "1x Potion";
                    break;
                case "French":
                    wonItem.GetComponentInChildren<Text>().text = "1x Potion";
                    break;
            }
            wonItem.sprite = invisiblePotion;
            CurrentData.instance.inivisiblePotion++;
        }
        wonItem.gameObject.SetActive(true);
        FindObjectOfType<Menu>().Set();
        CurrentData.instance.safeData();
        StartCoroutine(Close());
    }


    IEnumerator Close()
    {
        yield return new WaitForSecondsRealtime(2);
        DailyReward.instance.DailyRewardPanel.SetActive(false);

        foreach(Button b in FindObjectsOfType<Button>())
        {
            b.enabled = true;
        }
    }


}
