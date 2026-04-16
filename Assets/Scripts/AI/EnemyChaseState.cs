using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : StateMachineBehaviour
{
    private Transform player;
    private NavMeshAgent navMeshAgent;
    private AIController aiController;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = animator.GetComponent<NavMeshAgent>();
        aiController = animator.GetComponent<AIController>();

        if (navMeshAgent.isStopped)
        {
            navMeshAgent.isStopped = false;
        }

        navMeshAgent.speed = aiController.enemyChaseSpeed;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navMeshAgent.SetDestination(player.position);

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            aiController.StopChase();
            Debug.Log("Player caught");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navMeshAgent.speed = aiController.enemyWalkSpeed;
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
