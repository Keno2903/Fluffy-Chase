using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MenuCamera : MonoBehaviour
{

    public List<waypoint> points;
    public NavMeshAgent agent;
    public bool hasTarget = true;

    public Vector3 lastPos;

    public Vector3 target;

    public float waittimer = 4f;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        foreach (var waypoint in FindObjectsOfType<waypoint>())
        {
            points.Add(waypoint);

        }
        StartCoroutine(PlayTheme());
    }
    IEnumerator PlayTheme()
    {
        yield return new WaitForSecondsRealtime(1);
        foreach (AudioSource audio in AudioManager.instance.GetComponents<AudioSource>())
        {
            if (audio.clip.name == "Cartoon" && !audio.isPlaying)
                AudioManager.instance.Play("Theme");
        }

    }
    public void Walk()
    {
        if (!hasTarget)
        {
            waittimer = 4;
            int randomPoint = Random.Range(0, points.Count - 1);

            agent.SetDestination(points[randomPoint].transform.position);

            target = points[randomPoint].transform.position;

            hasTarget = true;

        }
        if (agent.remainingDistance <= agent.stoppingDistance && hasTarget) {
            waittimer -= Time.deltaTime;
            if (waittimer <= 0)
            hasTarget = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        Walk();

        if(agent.remainingDistance > 1)
        {
            animator.SetInteger("animation", 2);

        }else
        {
            animator.SetInteger("animation", 0);
        }
    }
}
