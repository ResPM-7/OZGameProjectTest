using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    // 현재 실행 중인 상태
    [field: SerializeField] public float WalkSpeed { get; private set; } = 5f;
    [field: SerializeField] public float RunSpeed { get; private set; } = 7f;
    [field: SerializeField] public float RotationSpeed { get; private set; } = 10f;
    [field: SerializeField] public float JumpForce { get; private set; } = 10f;
    [field: SerializeField] public float JumpDelay { get; private set; } = 0.4f;

    [Header("Combat Settings")] 
    [HideInInspector] public int ComboStep = 0;
    [HideInInspector] public bool CanNextAttack = false;

    public readonly int AnimMove = Animator.StringToHash("Speed");
    public readonly int AnimJump = Animator.StringToHash("Jump");
    public readonly int AnimIsGround = Animator.StringToHash("IsGrounded");
    public readonly int AnimAttack = Animator.StringToHash("Attack");
    public readonly int AnimAttackCount = Animator.StringToHash("AttackCount");
    public readonly int AnimHit = Animator.StringToHash("Hit");

    // 다른 상태들이 가져다 쓸 수 있도록 컴포넌트들을 프로퍼티으로 열어둠
    public Animator Anim { get; private set; }
    public Rigidbody RB { get; private set; }
    public PlayerInput Input { get; private set; }
    public Transform CameraTransform { get; private set; }

    public IPlayerState currentState { get; private set; }

    public IPlayerState IdleState { get; private set; }
    public IPlayerState MoveState { get; private set; }
    public IPlayerState JumpState { get; private set; }
    public IPlayerState LandingState { get; private set; }
    public IPlayerState AttackState { get; private set; }
    public IPlayerState HitState { get; private set; }


    private void Awake()
    {
        Anim = GetComponent<Animator>();
        RB = GetComponent<Rigidbody>();
        Input = GetComponent<PlayerInput>();
        CameraTransform = Camera.main.transform;

        IdleState = new PlayerIdleState(this);
        MoveState = new PlayerMoveState(this);
        JumpState = new PlayerJumpState(this);
        LandingState = new PlayerLandingState(this);
        AttackState = new PlayerAttackState(this);
        HitState = new PlayerHitState(this);
    }

    private void Start()
    {
        // 시작할 때 Idle
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && currentState == JumpState)
        {
            ChangeState(LandingState);
        }
    }

    public void TakeHit()
    {
        ChangeState(HitState);
    }

    public void ResetCombo()
    {
        ComboStep = 0;
        Anim.SetInteger(AnimAttackCount, 0);
        Anim.ResetTrigger(AnimAttack); // 혹시 모를 트리거 누적 방지

        // 공격이 끝났으므로 Idle 상태로 자연스럽게 복귀
        if (currentState == AttackState)
        {
            ChangeState(IdleState);
        }
    }
}