using UnityEngine;

public class PlayerAttackBehaviour : StateMachineBehaviour
{
    private PlayerStateMachine player;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null) { player = animator.GetComponent<PlayerStateMachine>(); }

        player.CanNextAttack = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= 0.6f)
        {
            player.CanNextAttack = true;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player != null)
        {
            // 다음 애니메이션이 "Attack" 태그가 아니라면 (즉, 연타가 끊겼다면)
            if (!animator.GetNextAnimatorStateInfo(layerIndex).IsTag("Attack"))
            {
                player.ResetCombo(); // FSM에게 콤보 초기화 및 Idle 상태 복귀 명령
            }
        }
    }
}