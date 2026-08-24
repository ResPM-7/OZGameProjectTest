using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{

    private Vector3 targetMoveDir;
    private float currentSpeed;

    public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void LogicUpdate()
    {
        if (stateMachine.Input.MoveInput.magnitude == 0)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }

        if (stateMachine.Input.IsJumpPressed)
        {
            stateMachine.Input.ConsumeJump();
            stateMachine.ChangeState(stateMachine.JumpState);
            return;
        }

        if (stateMachine.Input.IsAttackPressed)
        {
            stateMachine.Input.ConsumeAttack();
            stateMachine.ChangeState(stateMachine.AttackState);
            return;
        }

        // 이동 계산 및 애니메이션 
        currentSpeed = stateMachine.Input.IsRunning ? stateMachine.RunSpeed : stateMachine.WalkSpeed;
        stateMachine.Anim.SetFloat(stateMachine.AnimMove, currentSpeed);

        Vector2 moveInput = stateMachine.Input.MoveInput;
        Vector3 lookFwd = new Vector3(stateMachine.CameraTransform.forward.x, 0f, stateMachine.CameraTransform.forward.z).normalized;
        Vector3 lookRight = new Vector3(stateMachine.CameraTransform.right.x, 0f, stateMachine.CameraTransform.right.z).normalized;
        targetMoveDir = (lookFwd * moveInput.y) + (lookRight * moveInput.x);
    }

    public override void PhysicsUpdate()
    {
        if (targetMoveDir.magnitude > 0)
        {
            Quaternion viewRot = Quaternion.LookRotation(targetMoveDir.normalized);
            stateMachine.RB.MoveRotation(Quaternion.Lerp(stateMachine.RB.rotation, viewRot, Time.fixedDeltaTime * stateMachine.RotationSpeed));
        }
        Vector3 targetVel = targetMoveDir * currentSpeed;
        targetVel.y = stateMachine.RB.linearVelocity.y;
        stateMachine.RB.linearVelocity = targetVel;
    }
}