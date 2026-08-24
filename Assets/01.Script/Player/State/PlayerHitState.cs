using UnityEngine;

public class PlayerHitState : PlayerBaseState
{
    private float hitDuration = 0.5f; // 경직 시간 (애니메이션 길이에 맞춰 조절)
    private float timer;

    public PlayerHitState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.RB.linearVelocity = new Vector3(0, stateMachine.RB.linearVelocity.y, 0); // 이동 불가

        // 기존 ResetCombo() 역할
        stateMachine.ComboStep = 0;
        stateMachine.Anim.SetInteger(stateMachine.AnimAttackCount, 0);
        stateMachine.Anim.ResetTrigger(stateMachine.AnimAttack);

        stateMachine.Anim.SetTrigger(stateMachine.AnimHit);
        timer = 0f;
    }

    public override void LogicUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= hitDuration)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
}