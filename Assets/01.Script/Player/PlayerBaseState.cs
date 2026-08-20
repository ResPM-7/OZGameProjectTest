
public interface IPlayerState
{
    void Enter();           // 상태 진입 시 호출 (Start, Awake처럼 1회)
    void HandleInput();     // 입력 처리
    void LogicUpdate();     // 매 프레임 업데이트 (Update)
    void PhysicsUpdate();   // 물리 업데이트
    void Exit();
}

public abstract class PlayerBaseState : IPlayerState
{
    protected PlayerStateMachine stateMachine;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }         // 상태 진입 시 1회 호출
    public virtual void HandleInput() { }   // 입력 처리
    public virtual void LogicUpdate() { }   // 매 프레임 업데이트 (Update)
    public virtual void PhysicsUpdate() { } // 물리 업데이트
    public virtual void Exit() { }          // 상태를 빠져나갈 때 1회 호출
}