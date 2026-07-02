using UnityEngine;

public class PlayerAttackBehaviour : StateMachineBehaviour
{
    private PlayerMovement player;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null) { player = animator.GetComponent<PlayerMovement>(); }

        player.SetNextAttackReady(false);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= 0.6f)
        {
            player.SetNextAttackReady(true);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player != null)
        {
            if (!animator.GetNextAnimatorStateInfo(layerIndex).IsTag("Attack"))
            {
                player.ResetCombo();
            }
        }
    }
}
