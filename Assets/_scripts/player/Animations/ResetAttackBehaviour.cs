using UnityEngine;

// This script was written by AI. I never used the Interface StateMachineBehaviour. But All in all
// it has some specific functions like the OnStateExit that triggers when the State would change.
// This Script is located on the Punch state itself in the section Behaviour.
//
// So all this script dose is telling the PlayerController it can do calculation for movement again therefor the Run-Animation
// (if he is running) can fire again. 

public class ResetAttackBehaviour : StateMachineBehaviour
{
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerController playerController = animator.GetComponentInParent<PlayerController>();
        
        if (playerController != null)
        {
            playerController.EndAttack();
        }
    }
}