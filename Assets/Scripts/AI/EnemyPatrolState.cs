using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : StateMachineBehaviour
{
    // float timer;
    private List<Transform> waypoints;
    // private Transform m_CurrentWaypoint;
    private NavMeshAgent navMeshAgent;
    private AIController aiController;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<Renderer>().material = animator.gameObject.GetComponent<AIController>().patrolMaterial;
        aiController = animator.GetComponent<AIController>();
        // waypoints = aiController.waypoints;

        // timer = 0;
        navMeshAgent = animator.GetComponent<NavMeshAgent>();

        if (navMeshAgent.isStopped == true)
        {
            navMeshAgent.isStopped = false;
        }

        navMeshAgent.SetDestination(aiController.m_currentWaypoint.position);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navMeshAgent.SetDestination(aiController.m_currentWaypoint.position);

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            aiController.MoveWaypoint();
            animator.SetBool("IsPatrolling", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navMeshAgent.isStopped = true;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
