using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldCamera : MonoBehaviour
{
    float h2;

    [SerializeField]
    Image rt;

    public bool inField;

    player player;

    Camera cam; 
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        player = FindObjectOfType<player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.inspectingFarm)
        {
            if (Input.touchCount > 0 && Input.touchCount < 2)
            {
                Touch touch;
                touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    h2 = touch.deltaPosition.x / 30 * CurrentData.instance.Sensitivity / 4;
                    transform.Rotate(0, h2, 0);
                }
            }
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                if (difference > 0 && cam.fieldOfView < 40)
                    return;
                else if (difference < 0 && cam.fieldOfView > 90)
                        return;
                else
                cam.fieldOfView -= difference / 70;
                
                }
            }

        if (inField)
        {
            PointAndMoveArrow();
        } else
        {
            rt.GetComponent<Image>().enabled = false;
        }
        
    }


    void PointAndMoveArrow()
    {
        rt.GetComponent<Image>().enabled = true;
        // Get the position of the object in screen space
        Vector3 objScreenPos = Camera.main.WorldToScreenPoint(FindObjectOfType<player>().transform.position);
        rt.transform.position = new Vector3(objScreenPos.x, objScreenPos.y + 50, objScreenPos.z);
    }
}
