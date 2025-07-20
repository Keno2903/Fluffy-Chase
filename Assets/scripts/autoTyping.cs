using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class autoTyping  : MonoBehaviour
{

	public float letterPause = 0.2f;

	public string message; 

	public Text myText;

	// Use this for initialization
	void Start()
	{
		myText = GetComponent<Text>();
		myText.enabled = false;
	}

    public void Type()
    {
		StartCoroutine(TypeText());
    }

    private void Update()
    {
        
    }


    IEnumerator TypeText()
	{
		yield return new WaitForSeconds(0.1f);
		AudioManager.instance.Play("Text");
		myText.enabled = true;
		message = myText.text;
		myText.text = "";
		int count = 0;
		foreach (char letter in message.ToCharArray())
		{
			myText.text += letter;
			count++;
			yield return 0;
			yield return new WaitForEndOfFrame();

            if(count == message.ToCharArray().Length)
            {
				if (tutorial.instace != null)
				{
					tutorial.instace.textFinished = true;
				}
				else
				{
                    if(SceneManager.GetActiveScene().name == "CowExplaining")
					FindObjectOfType<CowExplanation>().textFinished = true;
				}


			}

		}      
        AudioManager.instance.Stop("Text");
	}
}