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

        GameManager.instance.SetChasePlayerState(true);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!navMeshAgent.pathPending && navMeshAgent.hasPath)
        {
            // Only trigger kill if path is valid AND we're actually close enough
            if (navMeshAgent.remainingDistance > 0 && navMeshAgent.remainingDistance <= aiController.hitRange)
            {
                aiController.enemyReachedPlayer = true;
                // aiController.StopChase();
                GameManager.instance.KillPlayer();
                // Debug.Log("Player caught");
            }
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navMeshAgent.speed = aiController.enemyWalkSpeed;
        GameManager.instance.SetChasePlayerState(false);
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
