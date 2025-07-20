using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arrow : MonoBehaviour
{

    RectTransform rt;

    public GameObject farmer;

    farmer f;

    Image i;

    void Start()
    {
        rt = GetComponent<RectTransform>();
        f = GetComponentInParent<farmer>();
        i = GetComponent<Image>();
    }

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        rt.GetComponent<Image>().enabled = false;
        if (!f.isChasing)
            return;
        i.enabled = true;
        Vector3 objScreenPos = Camera.main.WorldToScreenPoint(farmer.transform.position);
        rt.position = objScreenPos;
        Vector3 dirToTarget = (farmer.transform.position - Camera.main.transform.position).normalized;

        
        if (objScreenPos.x > Screen.width)
        {
            i.enabled = true;
            rt.position = new Vector3(Screen.width - rt.sizeDelta.x, rt.position.y, rt.position.z);
        }
        if (objScreenPos.x < 0)
        {
            i.enabled = true;
            rt.position = new Vector3(0 + rt.sizeDelta.x * 1.8f, rt.position.y, rt.position.z);
        }
        if (objScreenPos.y > Screen.height && objScreenPos.z > 3)
        {
            i.enabled = true;
            rt.position = new Vector3(rt.position.x, Screen.height - rt.sizeDelta.y, rt.position.z);
        }
        if (objScreenPos.y < 0)
        {
            i.enabled = true;
            rt.position = new Vector3(rt.position.x, 0 + rt.sizeDelta.y, rt.position.z);
        }
        if (Vector3.Angle(Camera.main.transform.forward, dirToTarget) > 180 / 2)
        {
            i.enabled = true;
            rt.position = new Vector3(rt.position.x, 0 + rt.sizeDelta.y, rt.position.z);
        }

        if(objScreenPos.x > 0 && objScreenPos.x < Screen.width && objScreenPos.y > 0 && objScreenPos.y < Screen.height)
        {
            i.enabled = false;
        }
    }
    
        /* Scripts handeling the rotation of the arrow

        public void PointArrow(farmer farmer_, player player_)
        {
            Vector3 dirToTarget = (farmer_.transform.position - Camera.main.transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < 180 / 2)
            {
                // Get the position of the object in screen space
                Vector3 objScreenPos = Camera.main.WorldToScreenPoint(farmer_.transform.position);

                // Get the directional vector between your arrow and the object
                Vector3 dir = (objScreenPos - rt.position).normalized;

                float angle = Mathf.Rad2Deg * Mathf.Atan2(dir.y, dir.x);

                Debug.Log(angle);

                rt.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            else
            {
                var targetPosLocal = Camera.main.transform.InverseTransformPoint(farmer_.transform.position);
                var targetAngle = -Mathf.Atan2(-targetPosLocal.x, -targetPosLocal.y) * Mathf.Rad2Deg - 90;
                rt.eulerAngles = new Vector3(0, 0, targetAngle);
            }
        }

        public void PointArrow1()
        {
            // Get the position of the object in screen space
            Vector3 objScreenPos = Camera.main.WorldToScreenPoint(farmer.transform.position);
            rt.localScale = new Vector3(1, 1, 1);
            // Get the directional vector between your arrow and the object
            Vector3 dir = (objScreenPos - Camera.main.transform.position).normalized;

            float angle = Mathf.Rad2Deg * Mathf.Atan2(dir.y, dir.x);

            rt.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            Vector3 objScreenPos2 = Camera.main.WorldToScreenPoint(farmer.transform.position);
            if(objScreenPos2.z < - 1)
            {
                rt.localScale = new Vector3(-1, 1, 1);
            }
        }
        */
}


