using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalSpawner : MonoBehaviour
{

    // This script switches the Animals in the ChooseAnimal Screen so that the user will be able to select the animal he wants

    public List<GameObject> animals = new List<GameObject>();

    public GameObject animal;

    public int animalIndex;

    public Text text;

    public Text selectText;

    public Text SpeedOfAnimal;

    public Text EnduranceOfAnimal;

    public Image lockImage;

    [SerializeField]
    public Transform rotation;

    Touch touch;

    float h2;

    bool touchDown;

    // Start is called before the first frame update
    void Start()
    {
        if (CurrentData.instance.animalsBought[animalIndex] != animalIndex + 1)
        {
            lockImage.enabled = true;
        }
        else
        {
            lockImage.enabled = false;
        }

        if (animalIndex == CurrentData.instance.currentAnimal)
        {
            if(CurrentData.instance.language == "English")
            {
                selectText.text = "Selected";
            }
            else if(CurrentData.instance.language == "French")
            {
                selectText.text = "sélectionné";
            }
            else if(CurrentData.instance.language == "Deutsch")
            {
                selectText.text = "Ausgewaehlt";
            }
            
        }
        else
        {
            if (CurrentData.instance.language == "English")
            {
                selectText.text = "Select";
            }
            else if (CurrentData.instance.language == "French")
            {
                selectText.text = "sélectionner";
            }
            else if (CurrentData.instance.language == "Deutsch")
            {
                selectText.text = "Auswählen";
            }
        }
        animal = Instantiate(animals[animalIndex], transform.position, rotation.rotation);

        StartCoroutine(showNameForFirstAnimal());
        
    }

    // Update is called once per frame
    void Update()
    {
        animal.transform.Rotate(0, 0.7f, 0);
        rotation.Rotate(0, 0.7f, 0);
        animal.GetComponent<player>().thirdPersonCamera.SetActive(false);
        animal.GetComponent<player>().topCamera.SetActive(false);

        EnduranceOfAnimal.text = animals[animalIndex].GetComponent<player>().animal.endurance.ToString();
        SpeedOfAnimal.text = animals[animalIndex].GetComponent<player>().animal.speed.ToString();

        if (Input.touchCount == 0)
            return;

        touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Moved && !touchDown)
        {
            touchDown = true;
            h2 = touch.deltaPosition.x / 30 * CurrentData.instance.Sensitivity / 4;

            if (h2 < 0)
                swipeRight();
            else
                swipeLeft();

            h2 = 0;
        }else if(touch.phase == TouchPhase.Ended)
        {
            touchDown = false;
            h2 = 0;
        }

    }
    IEnumerator showNameForFirstAnimal()
    {
        // This has to be implemted, because the first name of the first animal is not shown, due to the instantiation taking too long;
        yield return new WaitForEndOfFrame();
        showNameOfAnimal();
    }
    public void swipeRight()
    {
        AudioManager.instance.Play("Button");
        if (animalIndex == animals.Count - 1)
            return;

        Destroy(animal);
        animalIndex += 1;
        showNameOfAnimal();
        animal = Instantiate(animals[animalIndex], transform.position, rotation.rotation);

        if(CurrentData.instance.sound)
        animal.GetComponent<player>().animalSound.Play();

        if (CurrentData.instance.animalsBought[animalIndex] != animalIndex + 1)
        {
            lockImage.enabled = true;
        }
        else
        {
            lockImage.enabled = false;
        }

        if (animalIndex == CurrentData.instance.currentAnimal)
        {
            if (CurrentData.instance.language == "English")
            {
                selectText.text = "Selected";
            }
            else if (CurrentData.instance.language == "French")
            {
                selectText.text = "selectionne";
            }
            else if (CurrentData.instance.language == "Deutsch")
            {
                selectText.text = "Ausgewählt";
            }

        }
        else
        {
            if (CurrentData.instance.language == "English")
            {
                selectText.text = "Select";
            }
            else if (CurrentData.instance.language == "French")
            {
                selectText.text = "selectionner";
            }
            else if (CurrentData.instance.language == "Deutsch")
            {
                selectText.text = "Auswählen";
            }
        }

    }

    public void swipeLeft()
    {
        AudioManager.instance.Play("Button");
        if (animalIndex == 0)
            return;
        Destroy(animal);

        animalIndex -= 1;
        showNameOfAnimal();
        animal = Instantiate(animals[animalIndex], transform.position, rotation.rotation);

        if (CurrentData.instance.sound)
            animal.GetComponent<player>().animalSound.Play();

        if (animalIndex == CurrentData.instance.currentAnimal)
        {
            if (CurrentData.instance.language == "English")
            {
                selectText.text = "Selected";
            }
            else if (CurrentData.instance.language == "French")
            {
                selectText.text = "sélectionné";
            }
            else if (CurrentData.instance.language == "Deutsch")
            {
                selectText.text = "Ausgewählt";
            }

        }
        else
        {
            if (CurrentData.instance.language == "English")
            {
                selectText.text = "Select";
            }
            else if (CurrentData.instance.language == "French")
            {
                selectText.text = "sélectionner";
            }
            else if (CurrentData.instance.language == "Deutsch")
            {
                selectText.text = "Auswählen";
            }
        }

        if (CurrentData.instance.animalsBought[animalIndex] != animalIndex + 1)
        {
            lockImage.enabled = true;
        }else
        {
            lockImage.enabled = false;
        }

    }

    public void selectAnimal()
    {
        AudioManager.instance.Play("Button");
        if (CurrentData.instance.animalsBought[animalIndex] != animalIndex + 1)
        {
            return;
        }

        if(CurrentData.instance.sound)
        animal.GetComponent<player>().animalSound.Play();

        animal.GetComponent<Animator>().SetInteger("animation", 3);
        StartCoroutine(animateAnimal());
        CurrentData.instance.currentAnimal = animalIndex;
        if (animalIndex == CurrentData.instance.currentAnimal)
        {
            if (CurrentData.instance.language == "English")
            {
                selectText.text = "Selected";
            }
            else if (CurrentData.instance.language == "French")
            {
                selectText.text = "sélectionné";
            }
            else if (CurrentData.instance.language == "Deutsch")
            {
                selectText.text = "Ausgewählt";
            }

        }
        else
        {
            if (CurrentData.instance.language == "English")
            {
                selectText.text = "Select";
            }
            else if (CurrentData.instance.language == "French")
            {
                selectText.text = "sélectionner";
            }
            else if (CurrentData.instance.language == "Deutsch")
            {
                selectText.text = "Auswählen";
            }
        }
        CurrentData.instance.safeData();
    }
    private void showNameOfAnimal()
    {
        switch(animals[animalIndex].name)
        {
            case "White Alpaca":
               text.text = "Pop";
                break;
            case "Black Alpaca":
                text.text = "Ellon";
                break;
            case "White Cat":
                text.text = "Cosmo"; 
                break;
            case "Striped Cat":
                text.text = "Samy"; 
                break;
            case "Chick":
                text.text = "Chester"; 
                break;
            case "Chicken":
                text.text = "Poki";
                break;
            case "Cow":
                text.text = "Rusty"; 
                break;
            case "Brown Dog":
                text.text = "Gismo";
                break;
            case "Striped Dog":
                text.text = "Wallow";
                break;
            case "Yellow Duck":
                text.text = "Twinkle"; 
                break;
            case "Brown Duck":
                text.text = "Quinky";
                break;
            case "Brown Goat":
                text.text = "Loop"; 
                break;
            case "Striped Goat":
                text.text = "Milo"; 
                break;
            case "Black Goat":
                text.text = "Boo"; 
                break;
            case "Brown Horse":
                text.text = "Dexter"; 
                break;
            case "White Horse":
                text.text = "Brad"; 
                break;
            case "Pig":
                text.text = "Honky";
                break;
            case "White Rabbit":
                text.text = "Scooby";  
                break;
            case "Brown Rabbit":
                text.text = "Simba"; 
                break;
            case "Pink Sheep":
                text.text = "Pu"; 
                break;
            case "White Sheep":
                text.text = "Kiba";
                break;
            case "Black Sheep":
                text.text = "Rocko";
                break;
            default:
                break;
        }
    }

    IEnumerator animateAnimal()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        // Animal is selected and jumps, this function puts the animator back to zero so that the animal stops jumping
        animal.GetComponent<Animator>().SetInteger("animation", 0);
    }
}
