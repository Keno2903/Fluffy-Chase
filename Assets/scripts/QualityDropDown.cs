using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QualityDropDown : MonoBehaviour
{

    public Dropdown dropdown;

    public static QualityDropDown instance;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    public void Set()
    {
        if(CurrentData.instance.language == "English")
        {
            switch (CurrentData.instance.QualityLevel)
            {
                case 0:
                    dropdown.captionText.text = "Low";
                    break;
                case 1:
                    dropdown.captionText.text = "Medium";
                    break;
                case 2:
                    dropdown.captionText.text = "High";
                    break;

            }
            dropdown.options[0].text = "Low";
            dropdown.options[1].text = "Medium";
            dropdown.options[2].text = "High";
        }
        else if(CurrentData.instance.language == "Deutsch")
        {
            switch (CurrentData.instance.QualityLevel)
            {
                case 0:
                    dropdown.captionText.text = "Niedrig";
                    break;
                case 1:
                    dropdown.captionText.text = "Medium";
                    break;
                case 2:
                    dropdown.captionText.text = "Hoch";
                    break;

            }
            dropdown.options[0].text = "Niedrig";
            dropdown.options[1].text = "Medium";
            dropdown.options[2].text = "Hoch";
        }else
        {
            switch (CurrentData.instance.QualityLevel)
            {
                case 0:
                    dropdown.captionText.text = "Faible";
                    break;
                case 1:
                    dropdown.captionText.text = "Moyen";
                    break;
                case 2:
                    dropdown.captionText.text = "Haut";
                    break;

            }
            dropdown.options[0].text = "Faible";
            dropdown.options[1].text = "Moyen";
            dropdown.options[2].text = "Haut";

        }
    }
    private void Start()
    {
        Set();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
