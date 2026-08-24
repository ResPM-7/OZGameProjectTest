using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        // Idle 진입 시 속도 애니메이션을 0으로 설정하고 정지
        stateMachine.Anim.SetFloat(stateMachine.AnimMove, 0f);
        stateMachine.RB.linearVelocity = new Vector3(0, stateMachine.RB.linearVelocity.y, 0);
    }

    public override void LogicUpdate()
    {
        if (stateMachine.Input.MoveInput.magnitude > 0)
        {
            stateMachine.ChangeState(stateMachine.MoveState);
        }
        else if (stateMachine.Input.IsJumpPressed)
        {
            stateMachine.Input.ConsumeJump();
            stateMachine.ChangeState(stateMachine.JumpState);
        }
        else if (stateMachine.Input.IsAttackPressed)
        {
            stateMachine.Input.ConsumeAttack();
            stateMachine.ChangeState(stateMachine.AttackState);
        }
    }
}