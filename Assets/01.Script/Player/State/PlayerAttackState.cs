using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        //공격 진입 시 이동 멈춤
        stateMachine.RB.linearVelocity = new Vector3(0, stateMachine.RB.linearVelocity.y, 0);

        //콤보 시작 로직
        if (stateMachine.ComboStep == 0) stateMachine.ComboStep = 1;

        stateMachine.Anim.SetInteger(stateMachine.AnimAttackCount, stateMachine.ComboStep);
        stateMachine.Anim.SetTrigger(stateMachine.AnimAttack);
        stateMachine.CanNextAttack = false;
    }

    public override void LogicUpdate()
    {
        // 연속 공격 입력 처리
        if (stateMachine.Input.IsAttackPressed && stateMachine.CanNextAttack)
        {
            stateMachine.Input.ConsumeAttack();
            if (stateMachine.ComboStep < 3)
            {
                stateMachine.ComboStep++;
                stateMachine.Anim.SetInteger(stateMachine.AnimAttackCount, stateMachine.ComboStep);
                stateMachine.Anim.SetTrigger(stateMachine.AnimAttack);
                stateMachine.CanNextAttack = false;
            }
        }
    }
}
