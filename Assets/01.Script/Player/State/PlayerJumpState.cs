using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private Vector3 targetMoveDir;
    private float currentSpeed;

    public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Anim.SetBool(stateMachine.AnimJump, true);
        stateMachine.Anim.SetBool(stateMachine.AnimIsGround, false);
        stateMachine.RB.AddForce(Vector3.up * stateMachine.JumpForce, ForceMode.Impulse);
    }

    public override void LogicUpdate()
    {
        //점프 중에도 공중 이동 방향을 계산함
        currentSpeed = stateMachine.Input.IsRunning ? stateMachine.RunSpeed : stateMachine.WalkSpeed;
        Vector2 moveInput = stateMachine.Input.MoveInput;
        Vector3 lookFwd = new Vector3(stateMachine.CameraTransform.forward.x, 0f, stateMachine.CameraTransform.forward.z).normalized;
        Vector3 lookRight = new Vector3(stateMachine.CameraTransform.right.x, 0f, stateMachine.CameraTransform.right.z).normalized;
        targetMoveDir = (lookFwd * moveInput.y) + (lookRight * moveInput.x);
    }

    public override void PhysicsUpdate()
    {
        // 공중 이동 물리 연산 적용
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