using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    private readonly int speedHash = Animator.StringToHash("Speed");

    private Vector3 targetMoveDir;
    private float currentSpeed;

    public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        
    }

    public override void LogicUpdate()
    {
        // 1. 상태 전환 검사
        if (stateMachine.Input.MoveInput.magnitude == 0)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }

        //점프 입력 
        /*
        if (stateMachine.Input.IsJumpPressed)
        {
            stateMachine.Input.ConsumeJump();
            stateMachine.ChangeState(new PlayerJumpState(stateMachine));
            return;
        }
        */

        // 3. 카메라를 기준으로 한 이동 방향 계산
        Vector2 moveInput = stateMachine.Input.MoveInput;
        Vector3 lookForward = new Vector3(stateMachine.CameraTransform.forward.x, 0f, stateMachine.CameraTransform.forward.z).normalized;
        Vector3 lookRight = new Vector3(stateMachine.CameraTransform.right.x, 0f, stateMachine.CameraTransform.right.z).normalized;

        targetMoveDir = (lookForward * moveInput.y) + (lookRight * moveInput.x);

        // 4. 달리기 입력(IsRunning)에 따른 속도 결정 및 애니메이션 적용
        currentSpeed = stateMachine.Input.IsRunning ? stateMachine.RunSpeed : stateMachine.WalkSpeed;
        stateMachine.Anim.SetFloat(speedHash, currentSpeed);
    }

    public override void PhysicsUpdate()
    {
        // 물리 연산은 반드시 PhysicsUpdate(FixedUpdate)에서 처리하여 덜덜거림(Jittering) 방지

        // 1. 캐릭터 회전
        if (targetMoveDir.magnitude > 0)
        {
            Quaternion viewRot = Quaternion.LookRotation(targetMoveDir.normalized);
            stateMachine.RB.MoveRotation(Quaternion.Lerp(stateMachine.RB.rotation, viewRot, Time.fixedDeltaTime * stateMachine.RotationSpeed));
        }

        // 2. 캐릭터 이동 (y축 속도는 기존 중력값을 그대로 유지)
        Vector3 targetVelocity = targetMoveDir * currentSpeed;
        targetVelocity.y = stateMachine.RB.linearVelocity.y;

        stateMachine.RB.linearVelocity = targetVelocity;
    }
}