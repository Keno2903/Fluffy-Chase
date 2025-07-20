using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask1;
    public LayerMask obstacleMask2;

    public float chaseTime = 4;

    [SerializeField]
    public float maxChaseTime = 4;

    public float hearRadiusWhenPlayerisSprinting;
    public float hearRadiusWhenPlayerisWalking;

    player player;

    farmer farmer; 

    private void Start()
    {
        chaseTime = maxChaseTime;
        player = FindObjectOfType<player>();
        farmer = GetComponentInParent<farmer>();
    }

    private void Update()
    {
        FindVisibleTargets();
        HearForPlayer();
    }

    void HearForPlayer()
    {
        float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);

        if(distance <= hearRadiusWhenPlayerisSprinting && player.GetComponent<player>().isSprinting && !player.GetComponent<player>().invisible && !GetComponentInParent<farmer>().playerInField)
        {
            // farmer can hear the player since the Player is sprinting
            farmer.isChasing = true;
        }else if(distance <= hearRadiusWhenPlayerisWalking && !player.GetComponent<player>().invisible && !GetComponentInParent<farmer>().playerInField)
        {
            // farmer can hear the player since player is so near, he does not have to sprint to be heard
            farmer.isChasing = true;
        }
    }

    void FindVisibleTargets()
    {

        if (farmer.playerInField || player.invisible)
        {
            if(farmer.isChasing)
            {
                farmer.agent.SetDestination(transform.position);
                
                farmer.isChasing = false;
                
                farmer.hasTarget = true;
               
            }
        }

        if (farmer.playerInField || GameManager.instance.tutorial)
            return;


        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        // When Distance is too big
        if (farmer.isChasing)
        {
            if (Vector3.Distance(this.gameObject.transform.position, player.transform.position) > 25)
            {
                chaseTime -= Time.deltaTime;
                if (chaseTime <= 0)
                {
                    farmer.isChasing = false;
                    farmer.hasTarget = false;
                    chaseTime = maxChaseTime;
                    if (player.farmersThatAreChasingPlayer == null)
                        return;

                    player.farmersThatAreChasingPlayer.Remove(farmer);
                    if (player.farmersThatAreChasingPlayer.Count == 0)
                    {
                        AudioManager.instance.Stop("Chase");
                        FindObjectOfType<GameUI>().chaseImage.enabled = false;
                    }
                }
            }
        }


        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                // Target is in ViewAngle
                float distance = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, distance, obstacleMask1) && !Physics.Raycast(transform.position, dirToTarget, distance, obstacleMask2))
                {
                    // Target is in sight
                    chaseTime = maxChaseTime;
                    if (farmer.playerInField || player.invisible)
                        return;

                    farmer.isChasing = true;

                } else
                {
                    // Hello
                    // Target escaped sight
                    chaseTime -= Time.deltaTime;
                    if (chaseTime <= 0)
                    {
                        // Farmer is supposed to chase player after not seeing him for a certain amount of time when chasing him, however, when the player stands still, the farmer should keep on chasing even if chaseTimer reaches zero
                        //Only stop music and visual feedback if farmer is not hunted by any farmer
                        if (player.v2 < 0.5f && Vector3.Distance(this.gameObject.transform.position, player.transform.position) < 7)
                            return;

                        farmer.isChasing = false;
                        farmer.hasTarget = false;
                        chaseTime = maxChaseTime;
                        if (player.farmersThatAreChasingPlayer == null)
                            return;

                        player.farmersThatAreChasingPlayer.Remove(farmer);
                        if (player.farmersThatAreChasingPlayer.Count == 0)
                        {
                            AudioManager.instance.Stop("Chase");
                            AudioManager.instance.Play("Stealth");
                            FindObjectOfType<GameUI>().chaseImage.enabled = false;
                        }
                    }
                }
            }
        }
    }
    public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;

        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, hearRadiusWhenPlayerisSprinting);
        Gizmos.DrawWireSphere(gameObject.transform.position, hearRadiusWhenPlayerisWalking);
    }
}
