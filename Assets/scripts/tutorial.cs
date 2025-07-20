using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class tutorial : MonoBehaviour
{

    enum statesOfTutorial
    {
        menu, selectLevel, LevelshowcaserScreen, InLevel, shop, animals
    }

    #region texts
    [SerializeField]
    autoTyping[] textsMenu = new autoTyping[4];
    [SerializeField]
    autoTyping[] textsSelectLevel = new autoTyping[4];
    [SerializeField]
    autoTyping[] textsLevelshowcaserScreen = new autoTyping[4];
    [SerializeField]
    autoTyping[] textsInLevel = new autoTyping[4];
    [SerializeField]
    autoTyping[] textsshop = new autoTyping[4];
    [SerializeField]
    autoTyping[] textsanimals = new autoTyping[4];
    [SerializeField]
    autoTyping[] currentTexts;
    #endregion

    #region pointerTargets
    [SerializeField]
    GameObject[] currentTargets = new GameObject[5]; 
    #endregion

    statesOfTutorial tutorialState;

    public bool textFinished = true;

    public int state = 0;

    public static tutorial instace;

    bool stateFinished;

    bool stateWasSetToZero;

    [SerializeField]
    public RawImage cowImage;
    // Start is called before the first frame update

    private void Awake()
    {
        if (instace != null)
            Destroy(this);
        else
            instace = this;
    }
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (!GameManager.instance.tutorial)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this);
        cowImage.enabled = true;
        currentTargets = FindObjectOfType<pointersArray>().pointersInScene;
    }


    private void Update()
    {
        if (state == 0)
            checkState();

        handleText();
        handlePointerAndButtons();
    }

    void checkState()
    {
        // Checks in which state the tutorial is in
        switch(SceneManager.GetActiveScene().name)
        {
            case "Menu":
                tutorialState = statesOfTutorial.menu;
                currentTexts = textsMenu;
                break;

            case "Showcase Level":
                tutorialState = statesOfTutorial.LevelshowcaserScreen;
                currentTexts = textsLevelshowcaserScreen;
                break;

            case "Level1":
                tutorialState = statesOfTutorial.InLevel;
                currentTexts = textsInLevel;
                break;

            case "Shop":
                tutorialState = statesOfTutorial.shop;
                currentTexts = textsshop;
                break;

            case "ChooseLevel":
                tutorialState = statesOfTutorial.selectLevel;
                currentTexts = textsSelectLevel;
                break;

            case "ChooseAnimal":
                tutorialState = statesOfTutorial.animals;
                currentTexts = textsanimals;
                break;
            default:
                break;
        }
    }

    void handleText()
    {
        currentTargets = FindObjectOfType<pointersArray>().pointersInScene;

        if (Input.GetMouseButtonDown(0) && stateWasSetToZero || state == 0 && textFinished)
        {
            if (state == currentTexts.Length || !stateWasSetToZero && tutorialState != statesOfTutorial.menu)
                return;

            AudioManager.instance.Stop("Text");
            textFinished = false;
            currentTexts[state].Type();

            if (state != 0)
                currentTexts[state - 1].GetComponent<Text>().enabled = false;

            if (state != currentTexts.Length)
                state++;

            if (currentTargets.Length == 0)
                return;
            
            for(int i = 0; i < currentTargets.Length; i++)
            {
                if(currentTargets[i] != null)
                {
                    if (i == state - 1)
                    {
                        currentTargets[i].GetComponent<Image>().enabled = true;
                    }
                    else
                    {
                        currentTargets[i].GetComponent<Image>().enabled = false;
                    }
                }
            }
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!GameManager.instance.tutorial)
            return;
        StartCoroutine(onSceneChanged());
    }

    IEnumerator onSceneChanged()
    {
        state = 0;
        yield return new WaitForSecondsRealtime(1);
        stateWasSetToZero = true;
        stateFinished = false;
    }

    void  handlePointerAndButtons()
    {
        if (textFinished && state >= currentTexts.Length && state != 0)
        {
            stateFinished = true;
            if(SceneManager.GetActiveScene().name == "Level1")
            {
                StartCoroutine(EndTutorial());
            }
            

        }
        else
            stateFinished = false;


        if (!stateFinished)
        {
            foreach (Button button in FindObjectsOfType<Button>())
            {
                button.enabled = false;
            }
        }
        else
        {
            if (SceneManager.GetActiveScene().name == "Level1")
                return;

            GameObject button = GameObject.FindWithTag("tutorialButton");
            button.GetComponent<Button>().enabled = true;
        }

    }

    IEnumerator EndTutorial()
    {
        yield return new WaitForSecondsRealtime(2);
        GameManager.instance.tutorial = false;
        Destroy(this);
        FindObjectOfType<Joystick>().enabled = true;
        cowImage.enabled = false;
        GameManager.instance.tutorial = false;
        CurrentData.instance.safeData();
        foreach (Text text in GetComponentsInChildren<Text>())
        {
            text.enabled = false;
        }
    }
}
