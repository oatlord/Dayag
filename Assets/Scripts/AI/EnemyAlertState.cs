using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAlertState : StateMachineBehaviour
{
    NavMeshAgent navMeshAgent;
    AIController aiController;
    GameObject go;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aiController = animator.gameObject.GetComponent<AIController>();
        go = animator.gameObject;

        go.GetComponent<Renderer>().material = aiController.alertMaterial;
        navMeshAgent = animator.gameObject.GetComponent<NavMeshAgent>();

        if (navMeshAgent.isStopped == false)
        {
            navMeshAgent.isStopped = true;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        go.transform.rotation = Quaternion.Lerp(go.transform.rotation, Quaternion.LookRotation(aiController.ReturnPlayerPosition() - go.transform.position), Time.deltaTime * aiController.turnSpeed);
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
