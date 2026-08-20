using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    private readonly int speedHash = Animator.StringToHash("Speed");

    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        // Idle 진입 시 속도 애니메이션을 0으로 설정하고 정지
        stateMachine.Anim.SetFloat(speedHash, 0f);
        stateMachine.RB.linearVelocity = new Vector3(0, stateMachine.RB.linearVelocity.y, 0);
    }

    public override void LogicUpdate()
    {
        // 1. 이동 키가 눌렸는가? -> Move 상태로 전환
        if (stateMachine.Input.MoveInput.magnitude > 0)
        {
            stateMachine.ChangeState(stateMachine.MoveState);
            return;
        }

        // 2. 공격 키가 눌렸는가? -> Attack 상태로 전환
        if (stateMachine.Input.IsAttackPressed)
        {
            stateMachine.Input.ConsumeAttack();
            // stateMachine.ChangeState(new PlayerAttackState(stateMachine));
        }
    }
}