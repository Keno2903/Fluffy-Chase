using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

public class TutorialMovie : MonoBehaviour
{
    [SerializeField]
    GameObject cow;

    [SerializeField]

    GameObject farmer;

    NavMeshAgent agent;
    NavMeshAgent agent2;

    Animator myAnimator;
    Animator myAnimator2;

    [SerializeField]
    Transform waypoint;

    public CinemachineDollyCart cart;
    public Animator[] an = new Animator[4];
    // Start is called before the first frame update
    void Start()
    {
        /*agent = cow.GetComponent<NavMeshAgent>();
        agent2 = farmer.GetComponent<NavMeshAgent>();
        myAnimator = cow.GetComponent<Animator>();
        myAnimator2 = farmer.GetComponent<Animator>();

        agent.SetDestination(waypoint.position);
       */
    }

    // Update is called once per frame
    void Update()
    {
        //agent2.SetDestination(cow.transform.position);
        //myAnimator.SetInteger("animation", 2);
          //  myAnimator.SetFloat("Head_Horizontal_f", 0, 0f, Time.deltaTime);
       if(Input.GetAxis("Vertical") > 0)
        {
            cart.m_Speed += Time.deltaTime;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            cart.m_Speed -= Time.deltaTime;
        }else
        {
            
        }

       if(Input.GetKeyDown(KeyCode.Space))
        {
            foreach(Animator i in an)
            {
                int n = Random.Range(0, 3);
                i.SetInteger("Animation_int", n);
            }
        }
    }
}
