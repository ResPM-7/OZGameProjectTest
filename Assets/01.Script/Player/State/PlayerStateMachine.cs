using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    // 현재 실행 중인 상태
    [field: SerializeField] public float WalkSpeed { get; private set; } = 5f;
    [field: SerializeField] public float RunSpeed { get; private set; } = 7f;
    [field: SerializeField] public float RotationSpeed { get; private set; } = 10f;
    [field: SerializeField] public float JumpForce { get; private set; } = 10f;


    // 다른 상태들이 가져다 쓸 수 있도록 컴포넌트들을 퍼블릭(또는 프로퍼티)으로 열어둠
    public Animator Anim { get; private set; }
    public Rigidbody RB { get; private set; }
    public PlayerInput Input { get; private set; }
    public Transform CameraTransform { get; private set; }

    private IPlayerState currentState;

    public IPlayerState IdleState { get; private set; }
    public IPlayerState MoveState { get; private set; }
    //public IPlayerState JumpState { get; private set; }
    //public IPlayerState AttackState { get; private set; }


    private void Awake()
    {
        Anim = GetComponent<Animator>();
        RB = GetComponent<Rigidbody>();
        Input = GetComponent<PlayerInput>();
        CameraTransform = Camera.main.transform;

        IdleState = new PlayerIdleState(this);
        IdleState = new PlayerMoveState(this);
    }

    private void Start()
    {
        // 시작할 때 기본 상태(Idle)로 진입
        ChangeState(IdleState);
    }

    private void Update()
    {
        currentState?.HandleInput();
        currentState?.LogicUpdate();
    }

    private void FixedUpdate()
    {
        currentState?.PhysicsUpdate();
    }

    // 상태 교체
    public void ChangeState(IPlayerState newState)
    {
        currentState?.Exit();       // 기존 상태 종료
        currentState = newState;    // 상태 교체
        currentState?.Enter();      // 새 상태 시작
    }
}