using UnityEngine;

public class PlayerLandingState : PlayerBaseState
{
    private float delayTimer; // 기존 코루틴을 대체할 타이머

    public PlayerLandingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Anim.SetBool(stateMachine.AnimIsGround, true);
        stateMachine.RB.linearVelocity = new Vector3(0, stateMachine.RB.linearVelocity.y, 0); //착지 시 이동 불가
        delayTimer = 0f;
    }

    public override void LogicUpdate()
    {
        delayTimer += Time.deltaTime;

        // 딜레이 시간이 끝나면 Idle로 복귀
        if (delayTimer >= stateMachine.JumpDelay)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public override void Exit()
    {
        stateMachine.Anim.SetBool(stateMachine.AnimJump, false);
    }
}
