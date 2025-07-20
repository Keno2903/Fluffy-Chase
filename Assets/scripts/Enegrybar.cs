using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enegrybar : MonoBehaviour
{

    public Slider mySlider;

    public Gradient gradient;

    public void setValue(float value)
    {
        mySlider.value = value;
        mySlider.fillRect.GetComponent<Image>().color = gradient.Evaluate(mySlider.normalizedValue);
    }

    // Start is called before the first frame update
    void Start()
    {
        mySlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
