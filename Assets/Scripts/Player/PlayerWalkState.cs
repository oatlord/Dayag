using UnityEngine;

public class PlayerWalkState : StateMachineBehaviour
{
    private Animator animator;

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

            Vector3 Direction = new(playerController.movement.ReadValue<Vector2>().x, 0, playerController.movement.ReadValue<Vector2>().y);

            characterController.Move(Direction * Time.deltaTime * playerController.moveSpeed);
            animator.gameObject.transform.rotation = Quaternion.Lerp(animator.gameObject.transform.rotation, Quaternion.LookRotation(Direction), Time.deltaTime * playerController.turnSpeed);
            // Turn object in direction of movement
            // animator.gameObject.transform.LookAt(animator.gameObject.transform.position + Direction);
        }
        // {
        //     // PlayerController playerController = animator.gameObject.GetComponent<PlayerController>();
        //     // playerController.PlayerWalk();
        // }
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
