using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    NavMeshAgent navMeshAgent;

    public Transform[] patrolPoints;

    [SerializeField]
    int currentPatrolPoint = 0;

    public float maxWaitTime = 2f, waitCounter, chaseRange;
 
    [SerializeField]
    Animator anim;

    private void Start()
    {
        waitCounter = maxWaitTime;
    }

    public enum AIStates
    {
        isIdle,
        isPatrolling,
        isHunting
    }
    
    public AIStates currentState;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }


    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, 
            PlayerController.instance.transform.position);

        switch (currentState)
        {
            case AIStates.isIdle:

                anim.SetBool("isMoving", false);

                if (waitCounter > 0)
                {
                    waitCounter -= Time.deltaTime;
                }
                else 
                {
                    currentState = AIStates.isPatrolling;
                    navMeshAgent.SetDestination(patrolPoints[currentPatrolPoint].position);
                }

                if (distanceToPlayer <= chaseRange)
                {
                    currentState = AIStates.isHunting;
                }

                break;

            case AIStates.isPatrolling:

                if (navMeshAgent.remainingDistance <= 0.2f)
                {
                    currentPatrolPoint++;

                    if (currentPatrolPoint >= patrolPoints.Length)
                    {
                        currentPatrolPoint = 0;
                    }

                    currentState = AIStates.isIdle;
                    waitCounter = maxWaitTime;
                }

                if (distanceToPlayer <= chaseRange) 
                {
                    currentState = AIStates.isHunting;
                }

                anim.SetBool("isMoving", true);

                break;

            case AIStates.isHunting:

                navMeshAgent.SetDestination(PlayerController.instance.transform.position);

                //TODO attack the player

                if (distanceToPlayer > chaseRange) 
                {
                    currentState = AIStates.isIdle;
                    waitCounter = maxWaitTime;
                    navMeshAgent.velocity = Vector3.zero;
                    navMeshAgent.SetDestination(transform.position);
                }
                break;
        }

        //navMeshAgent.SetDestination(patrolPoints[currentPatrolPoint].position);
        

        
    }
}
