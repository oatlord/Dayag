using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchState : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if (animator.gameObject.GetComponent<CharacterController>() != null && animator.gameObject.GetComponent<PlayerController>() != null)
        {
            CharacterController characterController = animator.gameObject.GetComponent<CharacterController>();
            PlayerController playerController = animator.gameObject.GetComponent<PlayerController>();

            Debug.Log("Player is crouching");

            if (animator.GetBool("IsMoving"))
            {
                Vector3 Direction = new(playerController.movement.ReadValue<Vector2>().x, 0, playerController.movement.ReadValue<Vector2>().y);

                characterController.Move(Direction * Time.deltaTime * playerController.crouchSpeed);
                animator.gameObject.transform.rotation = Quaternion.Lerp(animator.gameObject.transform.rotation, Quaternion.LookRotation(Direction), Time.deltaTime * playerController.turnSpeed);
                // characterController.Move(new Vector3(playerController.movement.ReadValue<Vector2>().x, 0, playerController.movement.ReadValue<Vector2>().y) * Time.deltaTime * playerController.crouchSpeed);
            }
        }
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
